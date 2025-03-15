namespace Departments.Worker;

using ContosoUniversity.Messaging.Contracts;

using Core;
using Core.Domain;

using MassTransit;

internal class CourseDeletedEventHandler(
    IInstructorsRwRepository repository,
    ILogger<CourseDeletedEventHandler> logger)
    : IConsumer<CourseDeletedEvent>
{
    public async Task Consume(ConsumeContext<CourseDeletedEvent> context)
    {
        Guid courseId = context.Message.Id;

        logger.LogInformation("Reset instructor assignments for course: {Id}", courseId);

        Instructor[] instructors = await repository.GetAllAssignedToCourses(
            [courseId],
            context.CancellationToken);

        logger.LogInformation(
            "Instructors to be reset assignment: {InstructorIds}.",
            string.Join(", ", instructors.Select(x => x.ExternalId)));

        foreach (Instructor instructor in instructors)
        {
            instructor.ResetCourseAssignment(courseId);
            await repository.Save(instructor, context.CancellationToken);
        }
    }
}
