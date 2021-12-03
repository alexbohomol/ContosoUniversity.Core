namespace ContosoUniversity.Data;

using System.IO;

using Microsoft.Extensions.Configuration;

public static class DesignTimeSupport
{
    public static string ConnectionString => new ConfigurationBuilder()
        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ContosoUniversity"))
        .AddJsonFile("appsettings.json")
        .Build()
        .GetConnectionString("Courses");
}