namespace ContosoUniversity.Data;

using System.IO;

using Microsoft.Extensions.Configuration;

public static class DesignTimeSupport
{
    public static IConfigurationRoot ConfigurationRoot => new ConfigurationBuilder()
        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../ContosoUniversity.Mvc"))
        .AddJsonFile("appsettings.Development.json")
        .Build();
}
