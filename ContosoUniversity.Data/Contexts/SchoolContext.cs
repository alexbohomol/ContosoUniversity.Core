namespace ContosoUniversity.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class SchoolContext : DbContext
    {
        private const string Schema = "dpt";

        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("Department", Schema);
            modelBuilder.Entity<Instructor>().ToTable("Instructor", Schema);
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment", Schema);
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment", Schema)
                .HasKey(c => new {c.CourseExternalId, c.InstructorId});
        }
    }
}