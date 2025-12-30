using ContosoUniversity.Data;

using HealthChecks.UI.Client;

using MediatR;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

using Students.Api.Models;
using Students.Core;
using Students.Core.Handlers.Commands;
using Students.Data.Reads;
using Students.Data.Writes;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDataInfrastructure();
builder.Services.AddStudentsSchemaReads();
builder.Services.AddStudentsSchemaWrites();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
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

app.MapGet("/api/students/{externalId:guid}", async (
        Guid externalId,
        [FromServices] IStudentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetById(externalId, cancellationToken));

app.MapGet("/api/students/enrolled/groups", async (
        [FromServices] IStudentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetEnrollmentDateGroups(cancellationToken));

app.MapGet("/api/students/enrolled", async (
        [FromQuery] Guid[] courseIds,
        [FromServices] IStudentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetStudentsEnrolledForCourses(courseIds, cancellationToken));

app.MapPost("/api/students/search", async (
        [FromBody] SearchModel searchModel,
        [FromServices] IStudentsRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.Search(
        searchModel.SearchRequest,
        searchModel.OrderRequest,
        searchModel.PageRequest,
        cancellationToken));

//Read-Write

app.MapPost("/api/students", async (
    [FromBody] CreateStudentRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var student = await mediator.Send(
        new CreateStudentCommand(
            request.EnrollmentDate,
            request.LastName,
            request.FirstName),
        cancellationToken);

    return Results.Created(
        $"/api/students/{student.ExternalId}",
        new CreateStudentResponse(
            student.ExternalId,
            student.EnrollmentDate,
            student.LastName,
            student.FirstName));
});

app.MapPut("/api/students/{externalId:guid}", async (
    Guid externalId,
    [FromBody] UpdateStudentRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var student = await mediator.Send(
        new EditStudentCommand
        {
            EnrollmentDate = request.EnrollmentDate,
            ExternalId = externalId,
            FirstName = request.FirstName,
            LastName = request.LastName
        },
        cancellationToken);

    return Results.Ok(new UpdateStudentResponse(
        student.EnrollmentDate,
        student.LastName,
        student.FirstName,
        student.ExternalId));
});

app.MapDelete("/api/students/{externalId:guid}", async (
    Guid externalId,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Send(
        new DeleteStudentCommand(externalId),
        cancellationToken);

    return Results.NoContent();
});

await app.RunAsync();
