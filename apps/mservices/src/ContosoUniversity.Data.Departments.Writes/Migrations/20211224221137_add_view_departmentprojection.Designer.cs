﻿// <auto-generated />
using System;
using ContosoUniversity.Data.Departments.Writes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContosoUniversity.Data.Departments.Writes.Migrations
{
    [DbContext(typeof(ReadWriteContext))]
    [Migration("20211224221137_add_view_departmentprojection")]
    partial class add_view_departmentprojection
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dpt")
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ContosoUniversity.Domain.Department.Department", b =>
                {
                    b.Property<Guid>("ExternalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<Guid?>("AdministratorId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("InstructorId");

                    b.Property<decimal>("Budget")
                        .HasColumnType("money");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ExternalId");

                    b.ToTable("Department", "dpt");
                });

            modelBuilder.Entity("ContosoUniversity.Domain.Instructor.CourseAssignment", b =>
                {
                    b.Property<Guid>("InstructorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("InstructorId", "CourseId");

                    b.ToTable("CourseAssignment", "dpt");
                });

            modelBuilder.Entity("ContosoUniversity.Domain.Instructor.Instructor", b =>
                {
                    b.Property<Guid>("ExternalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ExternalId");

                    b.ToTable("Instructor", "dpt");
                });

            modelBuilder.Entity("ContosoUniversity.Domain.Instructor.OfficeAssignment", b =>
                {
                    b.Property<Guid>("InstructorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("InstructorId");

                    b.ToTable("OfficeAssignment", "dpt");
                });

            modelBuilder.Entity("ContosoUniversity.Domain.Instructor.CourseAssignment", b =>
                {
                    b.HasOne("ContosoUniversity.Domain.Instructor.Instructor", null)
                        .WithMany("CourseAssignments")
                        .HasForeignKey("InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContosoUniversity.Domain.Instructor.OfficeAssignment", b =>
                {
                    b.HasOne("ContosoUniversity.Domain.Instructor.Instructor", null)
                        .WithOne("_officeAssignment")
                        .HasForeignKey("ContosoUniversity.Domain.Instructor.OfficeAssignment", "InstructorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ContosoUniversity.Domain.Instructor.Instructor", b =>
                {
                    b.Navigation("CourseAssignments");

                    b.Navigation("_officeAssignment");
                });
#pragma warning restore 612, 618
        }
    }
}
