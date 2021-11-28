namespace ContosoUniversity;

using System.Threading.Tasks;

using Data.Courses;
using Data.Departments;
using Data.Students;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

public static class Program
{
    public static async Task Main(string[] args)
    {
        IWebHost host = WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();

        await host.EnsureCoursesSchema();
        await host.EnsureStudentsSchema();
        await host.EnsureDepartmentsSchema();

        await host.RunAsync();
    }
}