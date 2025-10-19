using ContosoUniversity.Data;

using HealthChecks.UI.Client;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using Students.Data.Writes;
using Students.Worker;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDataInfrastructure();
builder.Services.AddStudentsSchemaWrites();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CourseDeletedEventHandler>()
        .Endpoint(cfg => cfg.Name = "course-deleted-event-handler-students");

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

HealthCheckOptions checkOptions = new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
};
app.MapHealthChecks("/health/readiness", checkOptions);
app.MapHealthChecks("/health/liveness", checkOptions);

await app.RunAsync();
