namespace ContosoUniversity.Data.Courses.Writes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

/// <summary>
///     https://docs.microsoft.com/uk-ua/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ReadWriteContext>
{
    public ReadWriteContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();

        optionsBuilder.UseSqlServer(DesignTimeSupport
            .ConfigurationRoot
            .GetConnectionString("Courses-RW"));

        return new ReadWriteContext(optionsBuilder.Options);
    }
}