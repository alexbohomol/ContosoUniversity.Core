using ContosoUniversity.Messaging.Contracts;

using MassTransit;

using Students.Core;
using Students.Core.Domain;

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
            "Students to be withdrawn from course: {StudentIds}.",
            string.Join(", ", students.Select(x => x.ExternalId)));

        foreach (Student student in students)
        {
            student.WithdrawCourses([courseId]);
            await repository.Save(student, context.CancellationToken);
        }
    }
}
