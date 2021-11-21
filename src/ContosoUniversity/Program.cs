namespace ContosoUniversity;

using Data.Seed;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        IWebHost host = CreateWebHostBuilder(args).Build();
        host.EnsureDataLayer();
        host.Run();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}