namespace ContosoUniversity.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RenameToIExternalIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "CourseUid",
                schema: "std",
                table: "Enrollment",
                newName: "CourseExternalId");

            migrationBuilder.RenameColumn(
                "UniqueId",
                schema: "dpt",
                table: "Department",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                "CourseUid",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "CourseExternalId");

            migrationBuilder.RenameColumn(
                "UniqueId",
                schema: "crs",
                table: "Course",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                "DepartmentUid",
                schema: "crs",
                table: "Course",
                newName: "DepartmentExternalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "CourseExternalId",
                schema: "std",
                table: "Enrollment",
                newName: "CourseUid");

            migrationBuilder.RenameColumn(
                "ExternalId",
                schema: "dpt",
                table: "Department",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                "CourseExternalId",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "CourseUid");

            migrationBuilder.RenameColumn(
                "ExternalId",
                schema: "crs",
                table: "Course",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                "DepartmentExternalId",
                schema: "crs",
                table: "Course",
                newName: "DepartmentUid");
        }
    }
}