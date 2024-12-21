using ContosoUniversity.Data;

using Departments.Core;
using Departments.Data.Reads;
using Departments.Data.Writes;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDataInfrastructure();
builder.Services.AddDepartmentsSchemaReads();
builder.Services.AddDepartmentsSchemaWrites();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
    // cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // ...
}

HealthCheckOptions checkOptions = new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
};
app.UseHealthChecks("/health/readiness", checkOptions);
app.UseHealthChecks("/health/liveness", checkOptions);

//Read-Only

// Departments

app.MapGet("/api/departments/names", async (
        [FromServices] IDepartmentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetDepartmentNamesReference(cancellationToken));

app.MapGet("/api/departments/{externalId:guid}", async (
        Guid externalId,
        [FromServices] IDepartmentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetById(externalId, cancellationToken));

app.MapGet("/api/departments", async (
        [FromServices] IDepartmentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetAll(cancellationToken));

// Instructors

app.MapGet("/api/instructors/names", async (
        [FromServices] IInstructorsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetInstructorNamesReference(cancellationToken));

app.MapGet("/api/instructors/{externalId:guid}", async (
        Guid externalId,
        [FromServices] IInstructorsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetById(externalId, cancellationToken));

app.MapGet("/api/instructors", async (
        [FromServices] IInstructorsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetAll(cancellationToken));

//Read-Write

app.Run();
