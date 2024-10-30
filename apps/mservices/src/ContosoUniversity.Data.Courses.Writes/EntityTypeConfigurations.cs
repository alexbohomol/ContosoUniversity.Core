namespace ContosoUniversity.Data.Courses.Writes;

using global::Courses.Core.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class EntityTypeConfigurations : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder
            .Property(x => x.Code)
            .HasConversion(c => (int)c, i => i)
            .HasColumnName("CourseCode")
            .IsRequired();

        builder
            .Property(x => x.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Credits)
            .HasConversion(c => (int)c, i => i)
            .IsRequired();

        builder
            .Property(x => x.DepartmentId);

        builder.ToTable("Course", "crs");
    }
}
