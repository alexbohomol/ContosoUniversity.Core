namespace ContosoUniversity.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Inheritance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Enrollment_Student_StudentID",
                "Enrollment");

            migrationBuilder.DropIndex("IX_Enrollment_StudentID", "Enrollment");

            migrationBuilder.RenameTable("Instructor", newName: "Person");
            migrationBuilder.AddColumn<DateTime>("EnrollmentDate", "Person", nullable: true);
            migrationBuilder.AddColumn<string>("Discriminator", "Person", nullable: false, maxLength: 128, defaultValue: "Instructor");
            migrationBuilder.AlterColumn<DateTime>("HireDate", "Person", nullable: true);
            migrationBuilder.AddColumn<int>("OldId", "Person", nullable: true);

            // Copy existing Student data into new Person table.
            migrationBuilder.Sql("INSERT INTO dbo.Person (LastName, FirstName, HireDate, EnrollmentDate, Discriminator, OldId) SELECT LastName, FirstName, null AS HireDate, EnrollmentDate, 'Student' AS Discriminator, ID AS OldId FROM dbo.Student");
            // Fix up existing relationships to match new PK's.
            migrationBuilder.Sql("UPDATE dbo.Enrollment SET StudentId = (SELECT ID FROM dbo.Person WHERE OldId = Enrollment.StudentId AND Discriminator = 'Student')");

            // Remove temporary key
            migrationBuilder.DropColumn("OldID", "Person");

            migrationBuilder.DropTable(
                "Student");

            migrationBuilder.CreateIndex(
                "IX_Enrollment_StudentID",
                "Enrollment",
                "StudentID");

            migrationBuilder.AddForeignKey(
                "FK_Enrollment_Person_StudentID",
                "Enrollment",
                "StudentID",
                "Person",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_CourseAssignment_Person_InstructorID",
                "CourseAssignment");

            migrationBuilder.DropForeignKey(
                "FK_Department_Person_InstructorID",
                "Department");

            migrationBuilder.DropForeignKey(
                "FK_Enrollment_Person_StudentID",
                "Enrollment");

            migrationBuilder.DropForeignKey(
                "FK_OfficeAssignment_Person_InstructorID",
                "OfficeAssignment");

            migrationBuilder.DropPrimaryKey(
                "PK_Person",
                "Person");

            migrationBuilder.DropColumn(
                "HireDate",
                "Person");

            migrationBuilder.DropColumn(
                "Discriminator",
                "Person");

            migrationBuilder.RenameTable(
                "Person",
                newName: "Student");

            migrationBuilder.AlterColumn<DateTime>(
                "EnrollmentDate",
                "Student",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                "PK_Student",
                "Student",
                "ID");

            migrationBuilder.CreateTable(
                "Instructor",
                table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Instructor", x => x.ID); });

            migrationBuilder.AddForeignKey(
                "FK_CourseAssignment_Instructor_InstructorID",
                "CourseAssignment",
                "InstructorID",
                "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_Department_Instructor_InstructorID",
                "Department",
                "InstructorID",
                "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_Enrollment_Student_StudentID",
                "Enrollment",
                "StudentID",
                "Student",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_OfficeAssignment_Instructor_InstructorID",
                "OfficeAssignment",
                "InstructorID",
                "Instructor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}