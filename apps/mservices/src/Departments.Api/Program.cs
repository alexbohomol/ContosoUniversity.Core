using ContosoUniversity.Data;

using Departments.Api.Models;
using Departments.Core;
using Departments.Core.Handlers.Commands;
using Departments.Data.Reads;
using Departments.Data.Writes;

using HealthChecks.UI.Client;

using MediatR;

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

app.MapPost("/api/departments", async (
    [FromBody] CreateDepartmentRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var department = await mediator.Send(
        new CreateDepartmentCommand(
            request.Name,
            request.Budget,
            request.StartDate,
            request.AdministratorId),
        cancellationToken);

    return Results.Created(
        $"/api/departments/{department.ExternalId}",
        new CreateDepartmentResponse(
            department.ExternalId,
            department.Name,
            department.Budget,
            department.StartDate,
            department.AdministratorId));
});

app.MapPut("/api/departments/{externalId:guid}", async (
    Guid externalId,
    [FromBody] EditDepartmentRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var department = await mediator.Send(
        new EditDepartmentCommand(
            request.Name,
            request.Budget,
            request.StartDate,
            request.AdministratorId,
            externalId,
            request.RowVersion),
        cancellationToken);

    return Results.Ok(new EditDepartmentResponse(
        department.ExternalId,
        department.Name,
        department.Budget,
        department.StartDate,
        department.AdministratorId));
});

app.MapDelete("/api/departments/{externalId:guid}", async (
    Guid externalId,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Send(
        new DeleteDepartmentCommand(externalId),
        cancellationToken);

    return Results.NoContent();
});

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

app.MapPost("/api/instructors", async (
    [FromBody] CreateInstructorRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var instructor = await mediator.Send(
        new CreateInstructorCommand(
            request.LastName,
            request.FirstName,
            request.HireDate,
            request.SelectedCourses,
            request.Location),
        cancellationToken);

    return Results.Created(
        $"/api/instructors/{instructor.ExternalId}",
        new CreateInstructorResponse(
            instructor.ExternalId,
            instructor.LastName,
            instructor.FirstName,
            instructor.HireDate,
            instructor.CourseAssignments.Select(x => x.CourseId).ToArray()));
});

app.MapPut("/api/instructors/{externalId:guid}", async (
    Guid externalId,
    [FromBody] EditInstructorRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var instructor = await mediator.Send(
        new EditInstructorCommand(
            externalId,
            request.LastName,
            request.FirstName,
            request.HireDate,
            request.SelectedCourses,
            request.Location),
        cancellationToken);

    return Results.Ok(new EditInstructorResponse(
        instructor.ExternalId,
        instructor.LastName,
        instructor.FirstName,
        instructor.HireDate,
        instructor.CourseAssignments.Select(x => x.CourseId).ToArray()));
});

app.MapDelete("/api/instructors/{externalId:guid}", async (
    Guid externalId,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Send(
        new DeleteInstructorCommand(externalId),
        cancellationToken);

    return Results.NoContent();
});

await app.RunAsync();
