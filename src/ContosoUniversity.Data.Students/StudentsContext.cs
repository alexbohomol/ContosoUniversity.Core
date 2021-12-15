namespace ContosoUniversity.Data.Students;

using Microsoft.EntityFrameworkCore;

public class StudentsContext : DbContext
{
    private const string Schema = "std";

    public StudentsContext(DbContextOptions<StudentsContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
    }
}