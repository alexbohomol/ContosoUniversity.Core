﻿// <auto-generated />
using System;
using ContosoUniversity.Data.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContosoUniversity.Data.Courses.Migrations
{
    using Writes;

    [DbContext(typeof(CoursesRwContext))]
    partial class CoursesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("crs")
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ContosoUniversity.Domain.Course.Course", b =>
                {
                    b.Property<Guid>("ExternalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("Id");

                    b.Property<int>("Code")
                        .HasColumnType("int")
                        .HasColumnName("CourseCode");

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ExternalId");

                    b.ToTable("Course", "crs");
                });
#pragma warning restore 612, 618
        }
    }
}
