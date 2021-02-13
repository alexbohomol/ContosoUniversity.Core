﻿// <auto-generated />

namespace ContosoUniversity.Migrations
{
    using System;

    using Data.Contexts;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    [DbContext(typeof(SchoolContext))]
    internal class SchoolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("ContosoUniversity.Models.Course", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<int>("CourseCode")
                    .HasColumnType("int");

                b.Property<int>("Credits")
                    .HasColumnType("int");

                b.Property<Guid>("DepartmentExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("ExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Title")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Course", "crs");
            });

            modelBuilder.Entity("ContosoUniversity.Models.CourseAssignment", b =>
            {
                b.Property<Guid>("CourseExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("InstructorId")
                    .HasColumnType("int");

                b.HasKey("CourseExternalId", "InstructorId");

                b.HasIndex("InstructorId");

                b.ToTable("CourseAssignment", "dpt");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Department", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<decimal>("Budget")
                    .HasColumnType("money");

                b.Property<Guid>("ExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<int?>("InstructorId")
                    .HasColumnType("int");

                b.Property<string>("Name")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("rowversion");

                b.Property<DateTime>("StartDate")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("InstructorId");

                b.ToTable("Department", "dpt");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Enrollment", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<Guid>("CourseExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<int?>("Grade")
                    .HasColumnType("int");

                b.Property<int>("StudentId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("StudentId");

                b.ToTable("Enrollment", "std");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Instructor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<Guid>("ExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("FirstMidName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("FirstName");

                b.Property<DateTime>("HireDate")
                    .HasColumnType("datetime2");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Instructor", "dpt");
            });

            modelBuilder.Entity("ContosoUniversity.Models.OfficeAssignment", b =>
            {
                b.Property<int>("InstructorID")
                    .HasColumnType("int");

                b.Property<string>("Location")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("InstructorID");

                b.ToTable("OfficeAssignment", "dpt");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Student", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .UseIdentityColumn();

                b.Property<DateTime>("EnrollmentDate")
                    .HasColumnType("datetime2");

                b.Property<Guid>("ExternalId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("FirstMidName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)")
                    .HasColumnName("FirstName");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.HasKey("Id");

                b.ToTable("Student", "std");
            });

            modelBuilder.Entity("ContosoUniversity.Models.CourseAssignment", b =>
            {
                b.HasOne("ContosoUniversity.Models.Instructor", null)
                    .WithMany("CourseAssignments")
                    .HasForeignKey("InstructorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("ContosoUniversity.Models.Department", b =>
            {
                b.HasOne("ContosoUniversity.Models.Instructor", "Administrator")
                    .WithMany()
                    .HasForeignKey("InstructorId");

                b.Navigation("Administrator");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Enrollment", b =>
            {
                b.HasOne("ContosoUniversity.Models.Student", "Student")
                    .WithMany("Enrollments")
                    .HasForeignKey("StudentId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Student");
            });

            modelBuilder.Entity("ContosoUniversity.Models.OfficeAssignment", b =>
            {
                b.HasOne("ContosoUniversity.Models.Instructor", "Instructor")
                    .WithOne("OfficeAssignment")
                    .HasForeignKey("ContosoUniversity.Models.OfficeAssignment", "InstructorID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Instructor");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Instructor", b =>
            {
                b.Navigation("CourseAssignments");

                b.Navigation("OfficeAssignment");
            });

            modelBuilder.Entity("ContosoUniversity.Models.Student", b => { b.Navigation("Enrollments"); });
#pragma warning restore 612, 618
        }
    }
}