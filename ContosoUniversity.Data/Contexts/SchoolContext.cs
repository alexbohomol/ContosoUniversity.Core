namespace ContosoUniversity.Data.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }

        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment", DbSchemas.Student);
            modelBuilder.Entity<Student>().ToTable("Student", DbSchemas.Student);

            modelBuilder.Entity<Department>().ToTable("Department", DbSchemas.Department);
            modelBuilder.Entity<Instructor>().ToTable("Instructor", DbSchemas.Department);
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment", DbSchemas.Department);
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment", DbSchemas.Department)
                .HasKey(c => new {c.CourseExternalId, c.InstructorId});
        }

        private static class DbSchemas
        {
            public static readonly string Department = "dpt";
            public static readonly string Student = "std";
        }
    }
}