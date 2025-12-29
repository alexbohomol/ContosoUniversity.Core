namespace Students.Worker.IntegrationTests;

using ContosoUniversity.Messaging.Contracts;

using Core.Domain;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

using Worker;

public class CourseDeletedEventHandlerTests :
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<TestsConfiguration>,
    IClassFixture<MsSqlContext>,
    IClassFixture<RabbitMqContext>
{
    private const int ProcessingTimeSlot = 1000;

    private readonly ITestHarness _testHarness;
    private readonly DbContextFactory _dbContextFactory;

    public CourseDeletedEventHandlerTests(
        DefaultApplicationFactory<IAssemblyMarker> factory,
        TestsConfiguration config,
        MsSqlContext msSqlContext,
        RabbitMqContext rabbitMqContext)
    {
        factory.RabbitMqConnectionSetterFunction = () => rabbitMqContext.ConnectionString;
        factory.DataSourceSetterFunction = () => msSqlContext.MsSqlDataSource;
        factory.ClientOptions.BaseAddress = config.BaseAddressHttpsUrl;
        _testHarness = factory.TestHarness;
        _dbContextFactory = new DbContextFactory(msSqlContext, config);
    }

    [Fact]
    public async Task NoEnrolledStudents_NoEnrolledStudents()
    {
        // Arrange
        var courseId = Guid.NewGuid();

        // Act
        await _testHarness.Bus.Publish(new CourseDeletedEvent(courseId));
        await Task.Delay(ProcessingTimeSlot);

        // Assert events side effects

        var published = await _testHarness.Published.Any<CourseDeletedEvent>(x => x.Context.Message.Id == courseId);
        var consumed = await _testHarness.Consumed.Any<CourseDeletedEvent>(x => x.Context.Message.Id == courseId);

        published.Should().BeTrue();
        consumed.Should().BeTrue();

        // Assert database side effects

        bool instructorsFoundInDb;
        using (var context = _dbContextFactory.CreateDbContext("Students-RW"))
        {
            instructorsFoundInDb = await context.Set<Student>().AnyAsync(x => x.Enrollments.Any(ca => ca.CourseId == courseId));
        }
        instructorsFoundInDb.Should().BeFalse();
    }

    [Fact]
    public async Task StudentEnrolled_AssignmentReset()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var student = Student.Create("John", "Doe", DateTime.Now);
        student.EnrollCourses([
            new Enrollment(student.ExternalId, courseId, Grade.A),
            new Enrollment(student.ExternalId, Guid.NewGuid(), Grade.B),
            new Enrollment(student.ExternalId, Guid.NewGuid(), Grade.C)
        ]);
        using (var context = _dbContextFactory.CreateDbContext("Students-RW"))
        {
            context.Add(student);
            await context.SaveChangesAsync();
        }

        // Act
        await _testHarness.Bus.Publish(new CourseDeletedEvent(courseId));
        await Task.Delay(ProcessingTimeSlot);

        // Assert events side effects

        var published = await _testHarness.Published.Any<CourseDeletedEvent>(x => x.Context.Message.Id == courseId);
        var consumed = await _testHarness.Consumed.Any<CourseDeletedEvent>(x => x.Context.Message.Id == courseId);

        published.Should().BeTrue();
        consumed.Should().BeTrue();

        // Assert database side effects

        Student studentFromDb;
        using (var context = _dbContextFactory.CreateDbContext("Students-RW"))
        {
            studentFromDb = await context.Set<Student>().SingleAsync(x => x.ExternalId == student.ExternalId);
        }
        studentFromDb.Should().NotBeNull();
        studentFromDb.Enrollments.Should().HaveCount(2);
        studentFromDb.Enrollments.Should().NotContain(ca => ca.CourseId == courseId);
    }
}
