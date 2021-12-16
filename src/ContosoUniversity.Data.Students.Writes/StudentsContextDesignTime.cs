namespace ContosoUniversity.Data.Students.Writes;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
///     https://docs.microsoft.com/uk-ua/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class StudentsContextDesignTime : IDesignTimeDbContextFactory<ReadWriteContext>
{
    public ReadWriteContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();

        optionsBuilder.UseSqlServer(DesignTimeSupport.ConnectionString);

        return new ReadWriteContext(optionsBuilder.Options);
    }
}