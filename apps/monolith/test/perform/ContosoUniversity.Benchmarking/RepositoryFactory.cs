namespace ContosoUniversity.Benchmarking;

using Data.Courses.Reads;
using Data.Courses.Writes;

using Microsoft.EntityFrameworkCore;

public static class RepositoryFactory
{
    internal static (ReadOnlyRepository repository, ReadOnlyContext context) CreateCoursesRo()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadOnlyContext>();
        optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1477;Initial Catalog=ContosoUniversity;User ID=courses_ro;Password=coursesRO-P@$$w0rd;Multiple Active Result Sets=True;Trust Server Certificate=True");

        var context = new ReadOnlyContext(optionsBuilder.Options);
        var repository = new ReadOnlyRepository(context);

        return (repository, context);
    }

    internal static (ReadWriteRepository repository, ReadWriteContext context) CreateCoursesRw()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReadWriteContext>();
        optionsBuilder.UseSqlServer("Data Source=127.0.0.1,1477;Initial Catalog=ContosoUniversity;User ID=courses_rw;Password=coursesRW-P@$$w0rd;Multiple Active Result Sets=True;Trust Server Certificate=True");

        var context = new ReadWriteContext(optionsBuilder.Options);
        var repository = new ReadWriteRepository(context);

        return (repository, context);
    }
}
