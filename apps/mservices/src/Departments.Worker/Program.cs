using ContosoUniversity.Data;

using Departments.Data.Writes;
using Departments.Worker;

using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDataInfrastructure();
builder.Services.AddDepartmentsSchemaWrites();
builder.Services.AddHostedService<Worker>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CourseDeletedEventHandler>()
        .Endpoint(cfg => cfg.Name = "course-deleted-event-handler-departments");

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});

var host = builder.Build();
host.Run();
