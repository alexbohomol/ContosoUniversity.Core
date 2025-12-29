namespace Departments.Worker.IntegrationTests;

using ContosoUniversity.Messaging.Contracts;

using Core.Domain;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

public class CourseDeletedEventHandlerTests :
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<TestsConfiguration>,
    IClassFixture<MsSqlContext>,
    IClassFixture<RabbitMqContext>
{
    private const int ProcessingTimeSlot = 500;

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
    public async Task NoAssignedInstructors_NoAssignedInstructors()
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
        using (var context = _dbContextFactory.CreateDbContext("Departments-RW"))
        {
            instructorsFoundInDb = await context.Set<Instructor>().AnyAsync(x => x.CourseAssignments.Any(ca => ca.CourseId == courseId));
        }
        instructorsFoundInDb.Should().BeFalse();
    }

    [Fact]
    public async Task InstructorAssigned_AssignmentReset()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var instructor = Instructor.Create("Doe", "John", DateTime.Today);
        instructor.AssignCourses([courseId, Guid.NewGuid(), Guid.NewGuid()]);
        using (var context = _dbContextFactory.CreateDbContext("Departments-RW"))
        {
            context.Add(instructor);
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

        Instructor instructorFromDb;
        using (var context = _dbContextFactory.CreateDbContext("Departments-RW"))
        {
            instructorFromDb = await context.Set<Instructor>().SingleAsync(x => x.ExternalId == instructor.ExternalId);
        }
        instructorFromDb.Should().NotBeNull();
        instructorFromDb.CourseAssignments.Should().HaveCount(2);
        instructorFromDb.CourseAssignments.Should().NotContain(ca => ca.CourseId == courseId);
    }
}
