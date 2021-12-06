namespace ContosoUniversity.Data.Courses;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Writes;

/// <summary>
///     https://docs.microsoft.com/uk-ua/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class CoursesContextDesignTime : IDesignTimeDbContextFactory<CoursesRwContext>
{
    public CoursesRwContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoursesRwContext>();

        optionsBuilder.UseSqlServer(DesignTimeSupport.ConnectionString);

        return new CoursesRwContext(optionsBuilder.Options);
    }
}