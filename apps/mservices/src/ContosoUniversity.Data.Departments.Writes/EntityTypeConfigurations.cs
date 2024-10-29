namespace ContosoUniversity.Data.Departments.Writes;

using System;

using Domain.Department;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class EntityTypeConfigurations :
    IEntityTypeConfiguration<Instructor>,
    IEntityTypeConfiguration<Department>,
    IEntityTypeConfiguration<CourseAssignment>,
    IEntityTypeConfiguration<OfficeAssignment>
{
    public void Configure(EntityTypeBuilder<CourseAssignment> builder)
    {
        builder.Property(x => x.InstructorId);
        builder.Property(x => x.CourseId);

        builder.HasKey(x => new { x.InstructorId, x.CourseId });

        builder.ToTable("CourseAssignment", ReadWriteContext.Schema);
    }

    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder
            .Property(x => x.Name)
            .HasMaxLength(Department.NameMaxLength)
            .IsRequired();

        builder
            .Property(x => x.Budget)
            .HasColumnType("money")
            .IsRequired();

        builder
            .Property(x => x.StartDate)
            .IsRequired();

        builder
            .Property(x => x.AdministratorId)
            .HasColumnName("InstructorId");

        builder.ToTable("Department", ReadWriteContext.Schema);
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

        builder
            .HasMany(x => x.CourseAssignments)
            .WithOne()
            .HasForeignKey("InstructorId");

        builder.Navigation(x => x.CourseAssignments).AutoInclude();

        builder
            .HasOne(typeof(OfficeAssignment), "_officeAssignment")
            .WithOne()
            .HasForeignKey("OfficeAssignment");

        builder.Navigation("_officeAssignment").AutoInclude();

        builder.ToTable("Instructor", ReadWriteContext.Schema);
    }

    public void Configure(EntityTypeBuilder<OfficeAssignment> builder)
    {
        builder.Property<Guid>("InstructorId");
        builder.HasKey("InstructorId");

        builder
            .Property(x => x.Title)
            .HasMaxLength(OfficeAssignment.TitleMaxLength)
            .IsRequired();

        builder.ToTable("OfficeAssignment", ReadWriteContext.Schema);
    }
}
