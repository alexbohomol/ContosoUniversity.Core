namespace ContosoUniversity.Data.Courses;

using Domain.Course;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntityTypeConfigurations : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property<int>("Id");

        builder.HasKey("Id");
        
        builder
            .Property(x => x.Code)
            .HasConversion(x => x, x => x)
            .HasColumnName("CourseCode");

        builder
            .Property(x => x.Title)
            .HasMaxLength(50);

        builder
            .Property(x => x.Credits)
            .HasConversion(x => x, x => x);

        builder.Property(x => x.DepartmentId);
        
        builder.Property(x => x.ExternalId);
        
        builder.ToTable("Course", "crs");
    }
}