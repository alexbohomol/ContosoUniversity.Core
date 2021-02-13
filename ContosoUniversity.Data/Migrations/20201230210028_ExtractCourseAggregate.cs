namespace ContosoUniversity.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ExtractCourseAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_CourseAssignment_Course_CourseID",
                "CourseAssignment");

            migrationBuilder.DropForeignKey(
                "FK_Enrollment_Course_CourseID",
                "Enrollment");

            migrationBuilder.DropIndex(
                "IX_Enrollment_CourseID",
                "Enrollment");

            migrationBuilder.DropPrimaryKey(
                "PK_CourseAssignment",
                "CourseAssignment");

            migrationBuilder.DropPrimaryKey(
                "PK_Course",
                "Course");

            migrationBuilder.DropColumn(
                "CourseID",
                "Enrollment");

            migrationBuilder.DropColumn(
                "CourseID",
                "CourseAssignment");

            migrationBuilder.EnsureSchema(
                "crs");

            migrationBuilder.EnsureSchema(
                "dpt");

            migrationBuilder.EnsureSchema(
                "std");

            migrationBuilder.RenameTable(
                "Student",
                newName: "Student",
                newSchema: "std");

            migrationBuilder.RenameTable(
                "OfficeAssignment",
                newName: "OfficeAssignment",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                "Instructor",
                newName: "Instructor",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                "Enrollment",
                newName: "Enrollment",
                newSchema: "std");

            migrationBuilder.RenameTable(
                "Department",
                newName: "Department",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                "CourseAssignment",
                newName: "CourseAssignment",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                "Course",
                newName: "Course",
                newSchema: "crs");

            migrationBuilder.RenameColumn(
                "CourseID",
                schema: "crs",
                table: "Course",
                newName: "CourseCode");

            migrationBuilder.AddColumn<Guid>(
                "CourseUid",
                schema: "std",
                table: "Enrollment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                "CourseUid",
                schema: "dpt",
                table: "CourseAssignment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                    "Id",
                    schema: "crs",
                    table: "Course",
                    type: "int",
                    nullable: false,
                    defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                "UniqueId",
                schema: "crs",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment",
                columns: new[] {"CourseUid", "InstructorID"});

            migrationBuilder.AddPrimaryKey(
                "PK_Course",
                schema: "crs",
                table: "Course",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.DropPrimaryKey(
                "PK_Course",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropColumn(
                "CourseUid",
                schema: "std",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                "CourseUid",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.DropColumn(
                "Id",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropColumn(
                "UniqueId",
                schema: "crs",
                table: "Course");

            migrationBuilder.RenameTable(
                "Student",
                "std",
                "Student");

            migrationBuilder.RenameTable(
                "OfficeAssignment",
                "dpt",
                "OfficeAssignment");

            migrationBuilder.RenameTable(
                "Instructor",
                "dpt",
                "Instructor");

            migrationBuilder.RenameTable(
                "Enrollment",
                "std",
                "Enrollment");

            migrationBuilder.RenameTable(
                "Department",
                "dpt",
                "Department");

            migrationBuilder.RenameTable(
                "CourseAssignment",
                "dpt",
                "CourseAssignment");

            migrationBuilder.RenameTable(
                "Course",
                "crs",
                "Course");

            migrationBuilder.RenameColumn(
                "CourseCode",
                "Course",
                "CourseID");

            migrationBuilder.AddColumn<int>(
                "CourseID",
                "Enrollment",
                "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                "CourseID",
                "CourseAssignment",
                "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                "PK_CourseAssignment",
                "CourseAssignment",
                new[] {"CourseID", "InstructorID"});

            migrationBuilder.AddPrimaryKey(
                "PK_Course",
                "Course",
                "CourseID");

            migrationBuilder.CreateIndex(
                "IX_Enrollment_CourseID",
                "Enrollment",
                "CourseID");

            migrationBuilder.AddForeignKey(
                "FK_CourseAssignment_Course_CourseID",
                "CourseAssignment",
                "CourseID",
                "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Enrollment_Course_CourseID",
                "Enrollment",
                "CourseID",
                "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}