namespace ContosoUniversity.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class DecoupleDepartmentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Course_Department_DepartmentID",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropIndex(
                "IX_Course_DepartmentID",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropColumn(
                "DepartmentID",
                schema: "crs",
                table: "Course");

            migrationBuilder.RenameColumn(
                "DepartmentID",
                schema: "dpt",
                table: "Department",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                "UniqueId",
                schema: "dpt",
                table: "Department",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                "DepartmentUid",
                schema: "crs",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "UniqueId",
                schema: "dpt",
                table: "Department");

            migrationBuilder.DropColumn(
                "DepartmentUid",
                schema: "crs",
                table: "Course");

            migrationBuilder.RenameColumn(
                "Id",
                schema: "dpt",
                table: "Department",
                newName: "DepartmentID");

            migrationBuilder.AddColumn<int>(
                "DepartmentID",
                schema: "crs",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                "IX_Course_DepartmentID",
                schema: "crs",
                table: "Course",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                "FK_Course_Department_DepartmentID",
                schema: "crs",
                table: "Course",
                column: "DepartmentID",
                principalSchema: "dpt",
                principalTable: "Department",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}