namespace IntegrationTesting.SharedKernel;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class JsonSerializerOptionsBuilder
{
    public static JsonSerializerOptions HealthChecks
    {
        get
        {
            var options = new JsonSerializerOptions();
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new JsonStringEnumConverter());

            return options;
        }
    }
}
