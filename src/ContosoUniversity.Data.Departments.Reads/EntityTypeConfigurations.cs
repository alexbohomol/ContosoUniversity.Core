namespace ContosoUniversity.Data.Departments.Reads;

using System;

using Domain.Department;
using Domain.Instructor;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class EntityTypeConfigurations :
    IEntityTypeConfiguration<InstructorReadModel>,
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

        builder.Property(x => x.Name);
        builder.Property(x => x.Budget);
        builder.Property(x => x.StartDate);

        builder
            .Property(x => x.AdministratorId)
            .HasColumnName("InstructorId");

        builder.ToTable("Department", ReadOnlyContext.Schema);
    }

    public void Configure(EntityTypeBuilder<InstructorReadModel> builder)
    {
        builder
            .HasKey(x => x.ExternalId);

        builder
            .Property(x => x.ExternalId)
            .HasColumnName("Id");

        builder.Property(x => x.FirstName);
        builder.Property(x => x.LastName);
        builder.Property(x => x.HireDate);

        builder
            .HasMany(typeof(CourseAssignment), "_courseAssignments")
            .WithOne()
            .HasForeignKey("InstructorId");

        builder.Navigation("_courseAssignments").AutoInclude();

        builder
            .HasOne(typeof(OfficeAssignment), "_officeAssignment")
            .WithOne()
            .HasForeignKey("OfficeAssignment");

        builder.Navigation("_officeAssignment").AutoInclude();

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