namespace ContosoUniversity.Data.Courses.Contexts
{
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class CoursesContext : DbContext
    {
        public CoursesContext(DbContextOptions<CoursesContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course", "crs");
        }
    }
}