namespace ContosoUniversity.Data.Students.Reads;

using global::Students.Core.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Student = global::Students.Core.Projections.Student;

internal class EntityTypeConfigurations :
    IEntityTypeConfiguration<Enrollment>,
    IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.Property(x => x.StudentId);
        builder.Property(x => x.CourseId);
        builder.Property(x => x.Grade);

        builder.HasKey(x => new { x.StudentId, x.CourseId });

        builder.ToTable("Enrollment", ReadOnlyContext.Schema);
    }

    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder.Property(x => x.LastName);
        builder.Property(x => x.FirstName);
        builder.Property(x => x.EnrollmentDate);

        builder
            .HasMany(x => x.Enrollments)
            .WithOne()
            .HasForeignKey("StudentId");

        builder.Navigation(x => x.Enrollments).AutoInclude();

        builder.ToTable("Student", ReadOnlyContext.Schema);
    }
}
