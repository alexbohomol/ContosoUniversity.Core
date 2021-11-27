using System.Threading.Tasks;
using ContosoUniversity.Data.Seed;
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

        await host.EnsureDataLayer();

        await host.RunAsync();
    }
}