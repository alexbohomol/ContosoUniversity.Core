namespace ContosoUniversity.Data.Students;

using Domain.Student;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EntityTypeConfigurations :
    IEntityTypeConfiguration<Enrollment>,
    IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.Property(x => x.StudentId);
        builder.Property(x => x.CourseId);
        builder.Property(x => x.Grade);

        builder.HasKey(x => new { x.StudentId, x.CourseId });

        builder.ToTable("Enrollment", StudentsContext.Schema);
    }

    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder
            .Property(x => x.LastName)
            .HasMaxLength(Student.LastNameMaxLength)
            .IsRequired();

        builder
            .Property(x => x.FirstName)
            .HasMaxLength(Student.FirstNameMaxLength)
            .IsRequired();

        builder
            .Property(x => x.EnrollmentDate)
            .IsRequired();

        builder
            .HasMany(x => x.Enrollments)
            .WithOne()
            .HasForeignKey("StudentId");

        builder.Navigation(x => x.Enrollments).AutoInclude();

        builder.ToTable("Student", StudentsContext.Schema);
    }
}