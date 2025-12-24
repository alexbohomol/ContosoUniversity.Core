using ContosoUniversity.Data;

using Courses.Data.Writes;

using HealthChecks.UI.Client;

using MassTransit;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDataInfrastructure();
builder.Services.AddCoursesSchemaWrites();
builder.Services.AddMassTransit(x =>
{
    // x.AddConsumer<DepartmentDeletedEventHandler>()
    //     .Endpoint(cfg => cfg.Name = "department-deleted-event-handler-courses");

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
