using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Data.Departments.Migrations
{
    public partial class remove_pk_courseassignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment",
                columns: new[] { "InstructorId", "CourseId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseAssignment",
                schema: "dpt",
                table: "CourseAssignment",
                column: "InstructorId");
        }
    }
}
