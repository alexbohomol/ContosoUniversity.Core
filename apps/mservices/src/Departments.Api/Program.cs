using ContosoUniversity.Data;

using Departments.Core;
using Departments.Data.Reads;
using Departments.Data.Writes;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;

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

//Read-Write

app.Run();
