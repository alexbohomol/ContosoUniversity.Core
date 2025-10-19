namespace ContosoUniversity.Mvc;

using System.Globalization;

using Application;

using Data;
using Data.Courses.Reads;
using Data.Courses.Writes;
using Data.Departments.Reads;
using Data.Departments.Writes;
using Data.Students.Reads;
using Data.Students.Writes;

using Filters;

using FluentValidation;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Middleware;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args, string[] hostUrls = null) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();

                if (hostUrls is not null)
                {
                    webBuilder.UseUrls(hostUrls);
                }
            });
}

internal class Startup(IWebHostEnvironment env)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddHealthChecks();

        services.AddDataInfrastructure();
        services.AddCoursesSchemaReads();
        services.AddCoursesSchemaWrites();
        services.AddStudentsSchemaReads();
        services.AddStudentsSchemaWrites();
        services.AddDepartmentsSchemaReads();
        services.AddDepartmentsSchemaWrites();

        services.AddControllersWithViews();

        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddValidatorsFromAssemblyContaining<IApplicationLayerMarker>();
        services.AddScoped(typeof(FillModelState<>));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IApplicationLayerMarker).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddProblemDetails();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (env.IsDevelopment())
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
