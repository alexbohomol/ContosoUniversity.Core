using ContosoUniversity.Messaging.Contracts;

using MassTransit;

using Students.Core;
using Students.Core.Domain;

internal partial class CourseDeletedEventHandler(
    IStudentsRwRepository repository,
    ILogger<CourseDeletedEventHandler> logger)
    : IConsumer<CourseDeletedEvent>
{
    public async Task Consume(ConsumeContext<CourseDeletedEvent> context)
    {
        Guid courseId = context.Message.Id;

        LogEntryPoint(courseId);

        Student[] students = await repository.GetStudentsEnrolledForCourses(
            [courseId],
            context.CancellationToken);

        var ids = string.Join(", ", students.Select(x => x.ExternalId));
        LogStudentsFound(ids);

        foreach (Student student in students)
        {
            student.WithdrawCourses([courseId]);
            await repository.Save(student, context.CancellationToken);
        }
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Withdraw enrolled students for course: {Id}")]
    private partial void LogEntryPoint(Guid id);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Students to be withdrawn from course: {Ids}")]
    private partial void LogStudentsFound(string ids);
}
