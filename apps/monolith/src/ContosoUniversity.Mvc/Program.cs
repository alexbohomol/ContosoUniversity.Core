using System.Globalization;

using ContosoUniversity.Application;
using ContosoUniversity.Data;
using ContosoUniversity.Data.Courses.Reads;
using ContosoUniversity.Data.Courses.Writes;
using ContosoUniversity.Data.Departments.Reads;
using ContosoUniversity.Data.Departments.Writes;
using ContosoUniversity.Data.Students.Reads;
using ContosoUniversity.Data.Students.Writes;
using ContosoUniversity.Mvc.Filters;
using ContosoUniversity.Mvc.Middleware;

using FluentValidation;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console()
//     .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console();
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddHealthChecks();

builder.Services.AddDataInfrastructure();
builder.Services.AddCoursesSchemaReads();
builder.Services.AddCoursesSchemaWrites();
builder.Services.AddStudentsSchemaReads();
builder.Services.AddStudentsSchemaWrites();
builder.Services.AddDepartmentsSchemaReads();
builder.Services.AddDepartmentsSchemaWrites();

builder.Services.AddControllersWithViews();

builder.Services.AddValidatorsFromAssemblyContaining<ContosoUniversity.Mvc.IAssemblyMarker>();
builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
builder.Services.AddScoped(typeof(FillModelState<>));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(IAssemblyMarker).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    /*
     * The default HSTS value is 30 days. You may want to change this
     * for production scenarios, see https://aka.ms/aspnetcore-hsts.
     */
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

// https://stackoverflow.com/a/60245525/19518138
// https://itecnote.com/tecnote/c-force-locale-with-asp-net-core/
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/select-language-culture
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new[] { new CultureInfo("en-US") },
    FallBackToParentCultures = false
});

app.UseRouting();

app.UseAuthorization();

HealthCheckOptions checkOptions = new()
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
};
app.UseHealthChecks("/health/readiness", checkOptions);
app.UseHealthChecks("/health/liveness", checkOptions);

app.UseExceptionHandler();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();
