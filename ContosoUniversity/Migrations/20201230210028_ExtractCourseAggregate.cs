using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class ExtractCourseAggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Course_CourseID",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment");

            migrationBuilder.DropIndex(
                name: "IX_Enrollment_CourseID",
                table: "Enrollment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseAssignment",
                table: "CourseAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "CourseAssignment");

            migrationBuilder.EnsureSchema(
                name: "crs");

            migrationBuilder.EnsureSchema(
                name: "dpt");

            migrationBuilder.EnsureSchema(
                name: "std");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Student",
                newSchema: "std");

            migrationBuilder.RenameTable(
                name: "OfficeAssignment",
                newName: "OfficeAssignment",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                name: "Instructor",
                newName: "Instructor",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                newName: "Enrollment",
                newSchema: "std");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Department",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                name: "CourseAssignment",
                newName: "CourseAssignment",
                newSchema: "dpt");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Course",
                newSchema: "crs");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                schema: "crs",
                table: "Course",
                newName: "CourseCode");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseUid",
                schema: "std",
                table: "Enrollment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CourseUid",
                schema: "dpt",
                table: "CourseAssignment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "crs",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueId",
                schema: "crs",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment",
                columns: new[] { "CourseUid", "InstructorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                schema: "crs",
                table: "Course",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseUid",
                schema: "std",
                table: "Enrollment");

            migrationBuilder.DropColumn(
                name: "CourseUid",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                schema: "crs",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Student",
                schema: "std",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "OfficeAssignment",
                schema: "dpt",
                newName: "OfficeAssignment");

            migrationBuilder.RenameTable(
                name: "Instructor",
                schema: "dpt",
                newName: "Instructor");

            migrationBuilder.RenameTable(
                name: "Enrollment",
                schema: "std",
                newName: "Enrollment");

            migrationBuilder.RenameTable(
                name: "Department",
                schema: "dpt",
                newName: "Department");

            migrationBuilder.RenameTable(
                name: "CourseAssignment",
                schema: "dpt",
                newName: "CourseAssignment");

            migrationBuilder.RenameTable(
                name: "Course",
                schema: "crs",
                newName: "Course");

            migrationBuilder.RenameColumn(
                name: "CourseCode",
                table: "Course",
                newName: "CourseID");

            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "Enrollment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "CourseAssignment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseAssignment",
                table: "CourseAssignment",
                columns: new[] { "CourseID", "InstructorID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_CourseID",
                table: "Enrollment",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Course_CourseID",
                table: "CourseAssignment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Course_CourseID",
                table: "Enrollment",
                column: "CourseID",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
