namespace ContosoUniversity.Data.Courses.Reads;

using Application.Contracts.Repositories.ReadOnly.Projections;

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