namespace Students.Worker;

using ContosoUniversity.Messaging.Contracts;

using Core;
using Core.Domain;

using MassTransit;

internal class CourseDeletedEventHandler(
    IStudentsRwRepository repository,
    ILogger<CourseDeletedEventHandler> logger)
    : IConsumer<CourseDeletedEvent>
{
    public async Task Consume(ConsumeContext<CourseDeletedEvent> context)
    {
        Guid courseId = context.Message.Id;

        logger.LogInformation("Withdraw enrolled students for course: {Id}", courseId);

        Student[] students = await repository.GetStudentsEnrolledForCourses(
            [courseId],
            context.CancellationToken);

        logger.LogInformation(
            "Students to be withdraw from course: {StudentIds}.",
            string.Join(", ", students.Select(x => x.ExternalId)));

        foreach (Student student in students)
        {
            student.WithdrawCourses([courseId]);
            await repository.Save(student, context.CancellationToken);
        }
    }
}
