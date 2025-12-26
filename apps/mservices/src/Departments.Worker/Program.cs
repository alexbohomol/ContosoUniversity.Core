using ContosoUniversity.Data;

using Departments.Data.Writes;

using HealthChecks.UI.Client;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDataInfrastructure();
builder.Services.AddDepartmentsSchemaWrites();

builder.Services.AddOptions<RabbitMqTransportOptions>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CourseDeletedEventHandler>()
        .Endpoint(cfg => cfg.Name = "course-deleted-event-handler-departments");

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
