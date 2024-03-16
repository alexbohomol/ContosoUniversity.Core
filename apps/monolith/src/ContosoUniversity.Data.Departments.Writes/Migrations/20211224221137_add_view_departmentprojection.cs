using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Data.Departments.Writes.Migrations
{
    public partial class add_view_departmentprojection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
CREATE VIEW [{ReadWriteContext.Schema}].[DepartmentProjection] AS
SELECT 
  [d].[Id], 
  [d].[InstructorId] AS [AdministratorId], 
  [d].[Budget], 
  [d].[Name], 
  [d].[StartDate], 
  [i].[LastName] AS [AdministratorLastName], 
  [i].[FirstName] AS [AdministratorFirstName]
FROM [{ReadWriteContext.Schema}].[Department] AS [d]
LEFT OUTER JOIN [{ReadWriteContext.Schema}].[Instructor] AS [i] 
  ON [d].[InstructorId] = [i].[Id]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
DROP VIEW [{ReadWriteContext.Schema}].[DepartmentProjection]");
        }
    }
}
