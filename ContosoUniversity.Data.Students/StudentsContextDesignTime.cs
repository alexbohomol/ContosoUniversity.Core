namespace ContosoUniversity.Data.Students
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    /// <summary>
    /// https://docs.microsoft.com/uk-ua/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
    /// </summary>
    public class StudentsContextDesignTime : IDesignTimeDbContextFactory<StudentsContext>
    {
        public StudentsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StudentsContext>();
            
            optionsBuilder.UseSqlServer(DesignTimeSupport.ConnectionString);
            
            return new StudentsContext(optionsBuilder.Options);
        }
    }
}