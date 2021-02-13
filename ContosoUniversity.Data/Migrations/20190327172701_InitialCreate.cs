namespace ContosoUniversity.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Course",
                table => new
                {
                    CourseID = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Credits = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Course", x => x.CourseID); });

            migrationBuilder.CreateTable(
                "Student",
                table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(nullable: true),
                    FirstMidName = table.Column<string>(nullable: true),
                    EnrollmentDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Student", x => x.ID); });

            migrationBuilder.CreateTable(
                "Enrollment",
                table => new
                {
                    EnrollmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CourseID = table.Column<int>(nullable: false),
                    StudentID = table.Column<int>(nullable: false),
                    Grade = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollment", x => x.EnrollmentID);
                    table.ForeignKey(
                        "FK_Enrollment_Course_CourseID",
                        x => x.CourseID,
                        "Course",
                        "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Enrollment_Student_StudentID",
                        x => x.StudentID,
                        "Student",
                        "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Enrollment_CourseID",
                "Enrollment",
                "CourseID");

            migrationBuilder.CreateIndex(
                "IX_Enrollment_StudentID",
                "Enrollment",
                "StudentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Enrollment");

            migrationBuilder.DropTable(
                "Course");

            migrationBuilder.DropTable(
                "Student");
        }
    }
}