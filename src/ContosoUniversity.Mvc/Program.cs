﻿namespace ContosoUniversity.Mvc;

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

        services.AddCoursesSchemaReads(configuration.GetConnectionString("Courses"));
        services.AddCoursesSchemaWrites(configuration.GetConnectionString("Courses"));
        services.AddStudentsSchemaReads(configuration.GetConnectionString("Students"));
        services.AddStudentsSchemaWrites(configuration.GetConnectionString("Students"));
        services.AddDepartmentsSchemaReads(configuration.GetConnectionString("Departments"));
        services.AddDepartmentsSchemaWrites(configuration.GetConnectionString("Departments"));

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

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });

        if (app.Environment.IsDevelopment())
        {
            await services.EnsureCoursesSchema();
            await services.EnsureStudentsSchema();
            await services.EnsureDepartmentsSchema();
        }
        else
        {
            // require database created (and migrated?)
        }

        await app.RunAsync();
    }
}