namespace ContosoUniversity.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ComplexDataModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                "LastName",
                "Student",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "FirstName",
                "Student",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                "Title",
                "Course",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "DepartmentID",
            //    table: "Course",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.CreateTable(
                "Instructor",
                table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Instructor", x => x.ID); });

            migrationBuilder.CreateTable(
                "CourseAssignment",
                table => new
                {
                    InstructorID = table.Column<int>(nullable: false),
                    CourseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAssignment", x => new {x.CourseID, x.InstructorID});
                    table.ForeignKey(
                        "FK_CourseAssignment_Course_CourseID",
                        x => x.CourseID,
                        "Course",
                        "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_CourseAssignment_Instructor_InstructorID",
                        x => x.InstructorID,
                        "Instructor",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Department",
                table => new
                {
                    DepartmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Budget = table.Column<decimal>("money", nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    InstructorID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentID);
                    table.ForeignKey(
                        "FK_Department_Instructor_InstructorID",
                        x => x.InstructorID,
                        "Instructor",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.Sql("INSERT INTO dbo.Department (Name, Budget, StartDate) VALUES ('Temp', 0.00, GETDATE())");
            // Default value for FK points to department created above, with
            // defaultValue changed to 1 in following AddColumn statement.

            migrationBuilder.AddColumn<int>(
                "DepartmentID",
                "Course",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                "OfficeAssignment",
                table => new
                {
                    InstructorID = table.Column<int>(nullable: false),
                    Location = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeAssignment", x => x.InstructorID);
                    table.ForeignKey(
                        "FK_OfficeAssignment_Instructor_InstructorID",
                        x => x.InstructorID,
                        "Instructor",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Course_DepartmentID",
                "Course",
                "DepartmentID");

            migrationBuilder.CreateIndex(
                "IX_CourseAssignment_InstructorID",
                "CourseAssignment",
                "InstructorID");

            migrationBuilder.CreateIndex(
                "IX_Department_InstructorID",
                "Department",
                "InstructorID");

            migrationBuilder.AddForeignKey(
                "FK_Course_Department_DepartmentID",
                "Course",
                "DepartmentID",
                "Department",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Course_Department_DepartmentID",
                "Course");

            migrationBuilder.DropTable(
                "CourseAssignment");

            migrationBuilder.DropTable(
                "Department");

            migrationBuilder.DropTable(
                "OfficeAssignment");

            migrationBuilder.DropTable(
                "Instructor");

            migrationBuilder.DropIndex(
                "IX_Course_DepartmentID",
                "Course");

            migrationBuilder.DropColumn(
                "DepartmentID",
                "Course");

            migrationBuilder.AlterColumn<string>(
                "LastName",
                "Student",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                "FirstName",
                "Student",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                "Title",
                "Course",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}