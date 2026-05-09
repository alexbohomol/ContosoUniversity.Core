using ContosoUniversity.Messaging.Contracts;

using Courses.Core;
using Courses.Core.Domain;
using Courses.Core.Handlers.Commands;

using MassTransit;

using MediatR;

internal partial class DepartmentDeletedEventHandler(
    IMediator mediator,
    ICoursesRwRepository repository,
    ILogger<DepartmentDeletedEventHandler> logger)
    : IConsumer<DepartmentDeletedEvent>
{
    public async Task Consume(ConsumeContext<DepartmentDeletedEvent> context)
    {
        LogEntryPoint(context.Message.DepartmentId);

        Course[] courses = await repository.GetByDepartmentId(
            context.Message.DepartmentId,
            context.CancellationToken);

        var commands = courses.Select(x => new DeleteCourseCommand(x.ExternalId));
        /*
         * TODO: Optimize with parallel processing later.
         * https://github.com/alexbohomol/ContosoUniversity.Core/issues/135#issuecomment-3692942088
         */
        foreach (var command in commands)
        {
            await mediator.Send(command, context.CancellationToken);
        }
        // var tasks = commands.Select(x => mediator.Send(x, context.CancellationToken));
        // await Task.WhenAll(tasks);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Removing courses for deleted department: {DepartmentId}")]
    private partial void LogEntryPoint(Guid departmentId);
}
