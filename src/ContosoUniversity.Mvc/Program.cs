namespace ContosoUniversity.Mvc;

using System.Globalization;
using System.Threading.Tasks;

using Application;

using Data.Courses.Reads;
using Data.Courses.Writes;
using Data.Departments.Reads;
using Data.Departments.Writes;
using Data.Students.Reads;
using Data.Students.Writes;

using FluentValidation.AspNetCore;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        IServiceCollection services = builder.Services;

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = _ => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        ConfigurationManager configuration = builder.Configuration;

        services.AddCoursesSchemaReads(configuration);
        services.AddCoursesSchemaWrites(configuration);
        services.AddStudentsSchemaReads(configuration);
        services.AddStudentsSchemaWrites(configuration);
        services.AddDepartmentsSchemaReads(configuration);
        services.AddDepartmentsSchemaWrites(configuration);

        services
            .AddMvc()
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
                fv.RegisterValidatorsFromAssembly(typeof(IApplicationLayerMarker).Assembly);
            });

        services.AddMediatR(typeof(IApplicationLayerMarker).Assembly);

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
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

        app.UseHttpsRedirection();
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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });

        bool isDbAvailable = await services.EnsureCoursesSchemaIsAvailable()
                             && await services.EnsureStudentsSchemaIsAvailable()
                             && await services.EnsureDepartmentsSchemaIsAvailable();

        if (isDbAvailable)
            await app.RunAsync();
    }
}