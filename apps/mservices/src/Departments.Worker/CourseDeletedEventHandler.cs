using ContosoUniversity.Messaging.Contracts;

using Departments.Core;
using Departments.Core.Domain;

using MassTransit;

internal partial class CourseDeletedEventHandler(
    IInstructorsRwRepository repository,
    ILogger<CourseDeletedEventHandler> logger)
    : IConsumer<CourseDeletedEvent>
{
    public async Task Consume(ConsumeContext<CourseDeletedEvent> context)
    {
        Guid courseId = context.Message.Id;

        LogEntryPoint(courseId);

        Instructor[] instructors = await repository.GetAllAssignedToCourses(
            [courseId],
            context.CancellationToken);

        var ids = string.Join(", ", instructors.Select(x => x.ExternalId));
        LogInstructorsFound(ids);

        foreach (Instructor instructor in instructors)
        {
            instructor.ResetCourseAssignment(courseId);
            await repository.Save(instructor, context.CancellationToken);
        }
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Reset instructor assignments for course: {Id}")]
    private partial void LogEntryPoint(Guid id);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Instructors having assignments to reset: {Ids}")]
    private partial void LogInstructorsFound(string ids);
}
