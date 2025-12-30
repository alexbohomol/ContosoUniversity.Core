namespace Courses.Worker.IntegrationTests;

using ContosoUniversity.Messaging.Contracts;

using Core.Domain;

using FluentAssertions;

using IntegrationTesting.SharedKernel;

using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

public class DepartmentDeletedEventHandlerTests :
    IClassFixture<DefaultApplicationFactory<IAssemblyMarker>>,
    IClassFixture<TestsConfiguration>,
    IClassFixture<MsSqlContext>,
    IClassFixture<RabbitMqContext>
{
    private const int ProcessingTimeSlot = 500;

    private readonly ITestHarness _testHarness;
    private readonly DbContextFactory _dbContextFactory;

    public DepartmentDeletedEventHandlerTests(
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
    public async Task NoRelatedCourses_NoEventsIssued()
    {
        // Arrange
        var departmentId = Guid.NewGuid();

        // Act
        await _testHarness.Bus.Publish(new DepartmentDeletedEvent(departmentId));
        await Task.Delay(ProcessingTimeSlot);

        // Assert events side effects
        var published = await _testHarness.Published.Any<DepartmentDeletedEvent>(x => x.Context.Message.DepartmentId == departmentId);
        var consumed = await _testHarness.Consumed.Any<DepartmentDeletedEvent>(x => x.Context.Message.DepartmentId == departmentId);
        var nextEvents = _testHarness.Published.Select<CourseDeletedEvent>();

        published.Should().BeTrue();
        consumed.Should().BeTrue();
        nextEvents.Should().BeEmpty();

        // Assert database side effects

        bool coursesFoundInDb;
        using (var context = _dbContextFactory.CreateDbContext("Courses-RW"))
        {
            coursesFoundInDb = await context.Set<Course>().AnyAsync(x => x.DepartmentId == departmentId);
        }
        coursesFoundInDb.Should().BeFalse();
    }

    [Fact]
    public async Task RelatedCourseExists_CourseDeleted_EventIssued()
    {
        // Arrange
        var course = Course.Create(1001, "Test Course 1", 3, Guid.NewGuid());
        using (var context = _dbContextFactory.CreateDbContext("Courses-RW"))
        {
            context.Add(course);
            await context.SaveChangesAsync();
        }

        // Act
        await _testHarness.Bus.Publish(new DepartmentDeletedEvent(course.DepartmentId));
        await Task.Delay(ProcessingTimeSlot);

        // Assert events side effects

        var published = await _testHarness.Published.Any<DepartmentDeletedEvent>(x => x.Context.Message.DepartmentId == course.DepartmentId);
        var consumed = await _testHarness.Consumed.Any<DepartmentDeletedEvent>(x => x.Context.Message.DepartmentId == course.DepartmentId);
        var nextEvents = _testHarness.Published.Select<CourseDeletedEvent>(x => x.Context.Message.Id == course.ExternalId).ToArray();

        published.Should().BeTrue();
        consumed.Should().BeTrue();
        nextEvents.Length.Should().Be(1);
        nextEvents.Select(x => x.Context.Message.Id).Should().BeEquivalentTo([
            course.ExternalId
        ], options => options.WithoutStrictOrdering());

        // Assert database side effects

        bool coursesFoundInDb;
        using (var context = _dbContextFactory.CreateDbContext("Courses-RW"))
        {
            coursesFoundInDb = await context.Set<Course>().AnyAsync(x => x.DepartmentId == course.DepartmentId);
        }
        coursesFoundInDb.Should().BeFalse();
    }

    [Fact]
    public async Task RelatedCoursesExist_CoursesDeleted_EventsIssued()
    {
        // Arrange
        var departmentId = Guid.NewGuid();
        var course1 = Course.Create(1001, "Test Course 1", 3, departmentId);
        var course2 = Course.Create(1002, "Test Course 2", 3, departmentId);
        var course3 = Course.Create(1003, "Test Course 3", 3, departmentId);
        using (var context = _dbContextFactory.CreateDbContext("Courses-RW"))
        {
            context.AddRange(course1, course2, course3);
            await context.SaveChangesAsync();
        }

        // Act
        await _testHarness.Bus.Publish(new DepartmentDeletedEvent(departmentId));
        await Task.Delay(ProcessingTimeSlot);

        // Assert events side effects

        var published = await _testHarness.Published.Any<DepartmentDeletedEvent>(x => x.Context.Message.DepartmentId == departmentId);
        var consumed = await _testHarness.Consumed.Any<DepartmentDeletedEvent>(x => x.Context.Message.DepartmentId == departmentId);
        var nextEvents = _testHarness.Published.Select<CourseDeletedEvent>(x => new[]
        {
            course1.ExternalId,
            course2.ExternalId,
            course3.ExternalId
        }.Contains(x.Context.Message.Id)).ToArray();

        published.Should().BeTrue();
        consumed.Should().BeTrue();
        nextEvents.Length.Should().Be(3);
        nextEvents.Select(x => x.Context.Message.Id).Should().BeEquivalentTo([
            course1.ExternalId,
            course2.ExternalId,
            course3.ExternalId
        ], options => options.WithoutStrictOrdering());

        // Assert database side effects

        bool coursesFoundInDb;
        using (var context = _dbContextFactory.CreateDbContext("Courses-RW"))
        {
            coursesFoundInDb = await context.Set<Course>().AnyAsync(x => x.DepartmentId == departmentId);
        }
        coursesFoundInDb.Should().BeFalse();
    }
}
