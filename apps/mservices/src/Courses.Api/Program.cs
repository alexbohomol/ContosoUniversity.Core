using ContosoUniversity.Data;

using Courses.Core;
using Courses.Core.Handlers.Commands;
using Courses.Data.Reads;
using Courses.Data.Writes;

using HealthChecks.UI.Client;

using MediatR;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();
builder.Services.AddDataInfrastructure();
builder.Services.AddCoursesSchemaReads();
builder.Services.AddCoursesSchemaWrites();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
    // cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

var app = builder.Build();

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

app.MapGet("/api/courses",
    async (
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetAll(cancellationToken));

app.MapGet("/api/courses/{externalId:guid}",
    async (
        Guid externalId,
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetById(externalId, cancellationToken));

app.MapGet("/api/courses/department/{departmentExternalId:guid}",
    async (
        Guid departmentExternalId,
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetByDepartmentId(departmentExternalId, cancellationToken));

app.MapGet("/api/courses/existsByCourseCode/{courseCode:int}",
    async (
        int courseCode,
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.ExistsCourseCode(courseCode, cancellationToken));

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/parameter-binding?view=aspnetcore-9.0#bind-arrays-and-string-values-from-headers-and-query-strings
app.MapGet("/api/courses/title",
    async (
        [FromQuery] Guid[] entityIds,
        [FromServices] ICoursesRoRepository repository,
        CancellationToken cancellationToken)
    => await repository.GetCourseTitlesReference(entityIds, cancellationToken));

//Read-Write

app.MapPost("/api/courses", async (
    [FromBody] CreateCourseRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Send(
        new CreateCourseCommand(
            request.CourseCode,
            request.Title,
            request.Credits,
            request.DepartmentId),
        cancellationToken);
});

app.MapPut("/api/courses/{externalId:guid}", async (
    Guid externalId,
    [FromBody] UpdateCourseRequest request,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Send(
        new EditCourseCommand(
            externalId,
            request.Title,
            request.Credits,
            request.DepartmentId),
        cancellationToken);
});

app.MapDelete("/api/courses/{externalId:guid}", async (
    Guid externalId,
    [FromServices] IMediator mediator,
    CancellationToken cancellationToken) =>
{
    await mediator.Send(
        new DeleteCourseCommand(externalId),
        cancellationToken);
});

app.MapPut("/api/courses/credits/update",
    async (
        UpdateCoursesCreditsRequest request,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken)
    => await mediator.Send(
        new UpdateCoursesCreditsCommand(request.Multiplier),
        cancellationToken));

await app.RunAsync();

public record CreateCourseRequest(
    int CourseCode,
    string Title,
    int Credits,
    Guid DepartmentId);

public record UpdateCourseRequest(
    string Title,
    int Credits,
    Guid DepartmentId);

public record UpdateCoursesCreditsRequest(int Multiplier);
