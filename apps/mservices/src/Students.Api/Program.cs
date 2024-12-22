using ContosoUniversity.Data;
using ContosoUniversity.SharedKernel.Paging;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

using Students.Core;
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

app.Run();

internal record SearchModel(
    SearchRequest SearchRequest,
    OrderRequest OrderRequest,
    PageRequest PageRequest);
