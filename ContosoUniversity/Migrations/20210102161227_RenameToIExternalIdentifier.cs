using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class RenameToIExternalIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseUid",
                schema: "std",
                table: "Enrollment",
                newName: "CourseExternalId");

            migrationBuilder.RenameColumn(
                name: "UniqueId",
                schema: "dpt",
                table: "Department",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "CourseUid",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "CourseExternalId");

            migrationBuilder.RenameColumn(
                name: "UniqueId",
                schema: "crs",
                table: "Course",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "DepartmentUid",
                schema: "crs",
                table: "Course",
                newName: "DepartmentExternalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseExternalId",
                schema: "std",
                table: "Enrollment",
                newName: "CourseUid");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                schema: "dpt",
                table: "Department",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                name: "CourseExternalId",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "CourseUid");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                schema: "crs",
                table: "Course",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                name: "DepartmentExternalId",
                schema: "crs",
                table: "Course",
                newName: "DepartmentUid");
        }
    }
}
