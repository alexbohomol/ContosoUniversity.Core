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
            .HasConversion(c => (int)c, i => i)
            .HasColumnName("CourseCode")
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasMaxLength(50);

        builder
            .Property(x => x.Credits)
            .HasConversion(c => (int)c, i => i)
            .IsRequired();

        builder
            .Property(x => x.DepartmentId)
            .HasColumnName("DepartmentExternalId");

        builder.Property(x => x.ExternalId);

        builder.ToTable("Course", "crs");
    }
}