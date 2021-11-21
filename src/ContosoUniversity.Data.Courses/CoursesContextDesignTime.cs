namespace ContosoUniversity.Data.Courses;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

/// <summary>
///     https://docs.microsoft.com/uk-ua/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class CoursesContextDesignTime : IDesignTimeDbContextFactory<CoursesContext>
{
    public CoursesContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CoursesContext>();

        optionsBuilder.UseSqlServer(DesignTimeSupport.ConnectionString);

        return new CoursesContext(optionsBuilder.Options);
    }
}