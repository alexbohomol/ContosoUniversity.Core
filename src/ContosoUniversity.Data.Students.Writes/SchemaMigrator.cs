namespace ContosoUniversity.Data.Students.Writes;

using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

public static class SchemaMigrator
{
    public static async Task<bool> EnsureStudentsSchemaIsAvailable(this IServiceCollection services)
    {
        using IServiceScope scope = services.BuildServiceProvider().CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ReadWriteContext>();

        return await context.Database.CanConnectAsync();
    }
}