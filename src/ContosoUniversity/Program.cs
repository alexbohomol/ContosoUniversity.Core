using System.Threading.Tasks;
using ContosoUniversity.Data.Courses;
using ContosoUniversity.Data.Departments;
using ContosoUniversity.Data.Students;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ContosoUniversity;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var host = WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();

        await host.EnsureCoursesSchema();
        await host.EnsureStudentsSchema();
        await host.EnsureDepartmentsSchema();

        await host.RunAsync();
    }
}