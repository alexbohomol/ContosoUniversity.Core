namespace ContosoUniversity.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ColumnFirstName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "FirstMidName",
                "Student",
                "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                "FirstName",
                "Student",
                "FirstMidName");
        }
    }
}