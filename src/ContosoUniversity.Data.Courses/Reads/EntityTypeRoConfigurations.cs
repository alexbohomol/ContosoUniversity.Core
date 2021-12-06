namespace ContosoUniversity.Data.Courses.Reads;

using Domain.Course;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntityTypeRoConfigurations : IEntityTypeConfiguration<CourseReadModel>
{
    public void Configure(EntityTypeBuilder<CourseReadModel> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder
            .Property(x => x.Code)
            .HasColumnName("CourseCode");

        builder
            .Property(x => x.Title);

        builder
            .Property(x => x.Credits);

        builder
            .Property(x => x.DepartmentId);

        builder.ToTable("Course", "crs");
    }
}