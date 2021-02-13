namespace ContosoUniversity.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class DeclineInheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_CourseAssignment_Instructor_InstructorID",
                "CourseAssignment");

            migrationBuilder.DropForeignKey(
                "FK_Department_Instructor_InstructorID",
                "Department");

            migrationBuilder.DropForeignKey(
                "FK_Enrollment_Person_StudentID",
                "Enrollment");

            migrationBuilder.DropForeignKey(
                "FK_OfficeAssignment_Instructor_InstructorID",
                "OfficeAssignment");

            migrationBuilder.DropPrimaryKey(
                "PK_Instructor",
                "Person");

            migrationBuilder.CreateTable(
                "Instructor",
                table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Instructor", x => x.Id); });

            migrationBuilder.Sql(
                @"SET IDENTITY_INSERT [dbo].[Instructor] ON
                GO

                INSERT INTO [dbo].[Instructor] (ID, LastName, FirstName, HireDate)
                SELECT ID, LastName, FirstName, HireDate
                FROM [dbo].[Person]
                WHERE Discriminator = 'Instructor'
                GO

                SET IDENTITY_INSERT [dbo].[Instructor] OFF
                GO

                DELETE FROM [dbo].[Person]
                WHERE [Discriminator] = 'Instructor'
                GO");

            migrationBuilder.DropColumn(
                "HireDate",
                "Person");

            migrationBuilder.DropColumn(
                "Discriminator",
                "Person");

            migrationBuilder.RenameTable(
                "Person",
                newName: "Student");

            migrationBuilder.RenameColumn(
                "ID",
                "Student",
                "Id");

            migrationBuilder.AlterColumn<DateTime>(
                "EnrollmentDate",
                "Student",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_Student",
                "Student",
                "Id");

            migrationBuilder.AddForeignKey(
                "FK_CourseAssignment_Instructor_InstructorID",
                "CourseAssignment",
                "InstructorID",
                "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Department_Instructor_InstructorID",
                "Department",
                "InstructorID",
                "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Enrollment_Student_StudentID",
                "Enrollment",
                "StudentID",
                "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_OfficeAssignment_Instructor_InstructorID",
                "OfficeAssignment",
                "InstructorID",
                "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_CourseAssignment_Instructor_InstructorID",
                "CourseAssignment");

            migrationBuilder.DropForeignKey(
                "FK_Department_Instructor_InstructorID",
                "Department");

            migrationBuilder.DropForeignKey(
                "FK_Enrollment_Student_StudentID",
                "Enrollment");

            migrationBuilder.DropForeignKey(
                "FK_OfficeAssignment_Instructor_InstructorID",
                "OfficeAssignment");

            migrationBuilder.DropTable(
                "Instructor");

            migrationBuilder.DropPrimaryKey(
                "PK_Student",
                "Student");

            migrationBuilder.RenameTable(
                "Student",
                newName: "Person");

            migrationBuilder.RenameColumn(
                "Id",
                "Person",
                "ID");

            migrationBuilder.AlterColumn<DateTime>(
                "EnrollmentDate",
                "Person",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                "HireDate",
                "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                "Discriminator",
                "Person",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                "PK_Person",
                "Person",
                "ID");

            migrationBuilder.AddForeignKey(
                "FK_CourseAssignment_Person_InstructorID",
                "CourseAssignment",
                "InstructorID",
                "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Department_Person_InstructorID",
                "Department",
                "InstructorID",
                "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Enrollment_Person_StudentID",
                "Enrollment",
                "StudentID",
                "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_OfficeAssignment_Person_InstructorID",
                "OfficeAssignment",
                "InstructorID",
                "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}