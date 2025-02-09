using ContosoUniversity.Data;

using MassTransit;

using Students.Data.Writes;
using Students.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDataInfrastructure();
builder.Services.AddStudentsSchemaWrites();
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CourseDeletedEventHandler>()
        .Endpoint(cfg => cfg.Name = "course-deleted-event-handler-students");

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});

var host = builder.Build();
host.Run();
