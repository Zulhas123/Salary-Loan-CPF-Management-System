using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class HistoryTableModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "SalaryHistory");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SalaryHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SalaryHistory");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "SalaryHistory",
                type: "text",
                nullable: true);
        }
    }
}
