namespace ContosoUniversity.Mvc.IntegrationTests.HealthCheck;

using System.Text.Json;
using System.Text.Json.Serialization;

internal static class JsonSerializerOptions
{
    public static System.Text.Json.JsonSerializerOptions HealthChecks
    {
        get
        {
            var options = new System.Text.Json.JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }
    }
}
