using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoUniversity.Migrations
{
    public partial class TweakSeveralEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorID",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Instructor_InstructorID",
                schema: "dpt",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student_StudentID",
                schema: "std",
                table: "Enrollment");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                schema: "std",
                table: "Enrollment",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "EnrollmentID",
                schema: "std",
                table: "Enrollment",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentID",
                schema: "std",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentId");

            migrationBuilder.RenameColumn(
                name: "InstructorID",
                schema: "dpt",
                table: "Department",
                newName: "InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_Department_InstructorID",
                schema: "dpt",
                table: "Department",
                newName: "IX_Department_InstructorId");

            migrationBuilder.RenameColumn(
                name: "InstructorID",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseAssignment_InstructorID",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "IX_CourseAssignment_InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorId",
                schema: "dpt",
                table: "CourseAssignment",
                column: "InstructorId",
                principalSchema: "dpt",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Instructor_InstructorId",
                schema: "dpt",
                table: "Department",
                column: "InstructorId",
                principalSchema: "dpt",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_StudentId",
                schema: "std",
                table: "Enrollment",
                column: "StudentId",
                principalSchema: "std",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorId",
                schema: "dpt",
                table: "CourseAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Instructor_InstructorId",
                schema: "dpt",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollment_Student_StudentId",
                schema: "std",
                table: "Enrollment");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "std",
                table: "Enrollment",
                newName: "StudentID");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "std",
                table: "Enrollment",
                newName: "EnrollmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Enrollment_StudentId",
                schema: "std",
                table: "Enrollment",
                newName: "IX_Enrollment_StudentID");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                schema: "dpt",
                table: "Department",
                newName: "InstructorID");

            migrationBuilder.RenameIndex(
                name: "IX_Department_InstructorId",
                schema: "dpt",
                table: "Department",
                newName: "IX_Department_InstructorID");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "InstructorID");

            migrationBuilder.RenameIndex(
                name: "IX_CourseAssignment_InstructorId",
                schema: "dpt",
                table: "CourseAssignment",
                newName: "IX_CourseAssignment_InstructorID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseAssignment_Instructor_InstructorID",
                schema: "dpt",
                table: "CourseAssignment",
                column: "InstructorID",
                principalSchema: "dpt",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Instructor_InstructorID",
                schema: "dpt",
                table: "Department",
                column: "InstructorID",
                principalSchema: "dpt",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollment_Student_StudentID",
                schema: "std",
                table: "Enrollment",
                column: "StudentID",
                principalSchema: "std",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
