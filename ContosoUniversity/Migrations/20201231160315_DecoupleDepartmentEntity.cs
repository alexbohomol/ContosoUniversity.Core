using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class DecoupleDepartmentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentID",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_DepartmentID",
                schema: "crs",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                schema: "crs",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                schema: "dpt",
                table: "Department",
                newName: "Id");

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueId",
                schema: "dpt",
                table: "Department",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentUid",
                schema: "crs",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueId",
                schema: "dpt",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DepartmentUid",
                schema: "crs",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dpt",
                table: "Department",
                newName: "DepartmentID");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentID",
                schema: "crs",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Course_DepartmentID",
                schema: "crs",
                table: "Course",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentID",
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
