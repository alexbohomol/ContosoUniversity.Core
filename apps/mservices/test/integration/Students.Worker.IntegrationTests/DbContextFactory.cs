namespace Students.Worker.IntegrationTests;

using Data.Writes;

using IntegrationTesting.SharedKernel;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

internal class DbContextFactory(MsSqlContext msSqlContext, TestsConfiguration config)
{
    public ReadWriteContext CreateDbContext(string name)
    {
        var builder = new SqlConnectionStringBuilder(config.ConnectionString(name))
        {
            DataSource = msSqlContext.MsSqlDataSource
        };

        var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
        optionsBuilder.UseSqlServer(builder.ConnectionString);

        return new ReadWriteContext(optionsBuilder.Options);
    }
}
