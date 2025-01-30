namespace Departments.Worker;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MassTransit;

public class CourseDeletedNotificationHandler(ILogger<CourseDeletedNotificationHandler> logger)
    : IConsumer<CourseDeletedNotification>
{
    public Task Consume(ConsumeContext<CourseDeletedNotification> context)
    {
        var notification = context.Message;

        logger.LogInformation("Reset instructor assignments for course: {Id}", notification.Id);

        return Task.CompletedTask;
    }
}
