namespace ContosoUniversity.Data.Departments.Reads;

using System;

using Domain.Department;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class EntityTypeConfigurations :
    IEntityTypeConfiguration<Instructor>,
    IEntityTypeConfiguration<DepartmentReadModel>,
    IEntityTypeConfiguration<CourseAssignment>,
    IEntityTypeConfiguration<OfficeAssignment>
{
    public void Configure(EntityTypeBuilder<CourseAssignment> builder)
    {
        builder.Property(x => x.InstructorId);
        builder.Property(x => x.CourseId);

        builder.HasKey(x => new { x.InstructorId, x.CourseId });

        builder.ToTable("CourseAssignment", ReadOnlyContext.Schema);
    }

    public void Configure(EntityTypeBuilder<DepartmentReadModel> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder
            .Property(x => x.Name);

        builder
            .Property(x => x.Budget);

        builder
            .Property(x => x.StartDate);

        builder
            .Property(x => x.AdministratorId)
            .HasColumnName("InstructorId");

        builder.ToTable("Department", ReadOnlyContext.Schema);
    }

    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder
            .Property(x => x.FirstName)
            .HasMaxLength(Instructor.FirstNameMaxLength)
            .IsRequired();

        builder
            .Property(x => x.LastName)
            .HasMaxLength(Instructor.LastNameMaxLength)
            .IsRequired();

        builder
            .Property(x => x.HireDate)
            .IsRequired();

        builder.Ignore(x => x.Courses);

        builder
            .HasMany(x => x.Assignments)
            .WithOne()
            .HasForeignKey("InstructorId");

        builder
            .HasOne(x => x.Office)
            .WithOne()
            .HasForeignKey("OfficeAssignment");

        builder.ToTable("Instructor", ReadOnlyContext.Schema);
    }

    public void Configure(EntityTypeBuilder<OfficeAssignment> builder)
    {
        builder.Property<Guid>("InstructorId");
        builder.HasKey("InstructorId");

        builder
            .Property(x => x.Title)
            .HasMaxLength(OfficeAssignment.TitleMaxLength)
            .IsRequired();

        builder.ToTable("OfficeAssignment", ReadOnlyContext.Schema);
    }
}