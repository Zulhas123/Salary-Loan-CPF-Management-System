using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class UserwiseLoanModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FromInterest",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FromMain",
                table: "UserWiseLoans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromInterest",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "FromMain",
                table: "UserWiseLoans");
        }
    }
}
