namespace Students.Worker;

using ContosoUniversity.Messaging.Contracts.Notifications;

using MassTransit;

public class CourseDeletedNotificationHandler(ILogger<CourseDeletedNotificationHandler> logger)
    : IConsumer<CourseDeletedEvent>
{
    public Task Consume(ConsumeContext<CourseDeletedEvent> context)
    {
        var notification = context.Message;

        logger.LogInformation("Withdraw enrolled students for course: {Id}", notification.Id);

        return Task.CompletedTask;
    }
}
