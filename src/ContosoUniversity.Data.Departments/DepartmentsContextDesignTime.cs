namespace ContosoUniversity.Data.Departments
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    /// <summary>
    ///     https://docs.microsoft.com/uk-ua/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
    /// </summary>
    public class DepartmentsContextDesignTime : IDesignTimeDbContextFactory<DepartmentsContext>
    {
        public DepartmentsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DepartmentsContext>();

            optionsBuilder.UseSqlServer(DesignTimeSupport.ConnectionString);

            return new DepartmentsContext(optionsBuilder.Options);
        }
    }
}