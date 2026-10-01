namespace ContosoUniversity.Benchmarking.Factories;

using Data.Courses.Reads;
using Data.Courses.Writes;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public static class RepositoryFactory
{
    private static SqlConnectionStringBuilder GetDefaultConnectionStringBuilder() => new()
    {
        DataSource = "127.0.0.1,1477",
        InitialCatalog = "ContosoUniversity",
        MultipleActiveResultSets = true,
        TrustServerCertificate = true
    };

    internal static (ReadOnlyRepository repository, ReadOnlyContext context) CoursesReadOnly()
    {
        var sqlConnectionStringBuilder = GetDefaultConnectionStringBuilder();
        sqlConnectionStringBuilder.UserID = "courses_ro";
        sqlConnectionStringBuilder.Password = "coursesRO-P@$$w0rd";

        var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyContext>();
        optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);

        var context = new ReadOnlyContext(optionsBuilder.Options);
        var repository = new ReadOnlyRepository(context);

        return (repository, context);
    }

    internal static (ReadWriteRepository repository, ReadWriteContext context) CoursesReadWrite()
    {
        var sqlConnectionStringBuilder = GetDefaultConnectionStringBuilder();
        sqlConnectionStringBuilder.UserID = "courses_rw";
        sqlConnectionStringBuilder.Password = "coursesRW-P@$$w0rd";

        var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
        optionsBuilder.UseSqlServer(sqlConnectionStringBuilder.ConnectionString);

        var context = new ReadWriteContext(optionsBuilder.Options);
        var repository = new ReadWriteRepository(context);

        return (repository, context);
    }
}
