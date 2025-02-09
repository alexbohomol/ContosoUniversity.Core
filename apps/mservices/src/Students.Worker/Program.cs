using MassTransit;

using Students.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CourseDeletedNotificationHandler>()
        .Endpoint(cfg => cfg.Name = "course-deleted-event-handler-students");

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});

var host = builder.Build();
host.Run();
