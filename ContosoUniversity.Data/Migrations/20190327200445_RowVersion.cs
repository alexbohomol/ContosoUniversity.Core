namespace ContosoUniversity.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                "RowVersion",
                "Department",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "RowVersion",
                "Department");
        }
    }
}