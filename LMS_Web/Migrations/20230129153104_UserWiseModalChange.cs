using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class UserWiseModalChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromMonth",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromYear",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToMonth",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToYear",
                table: "UserWiseLoans",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromMonth",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "FromYear",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "ToMonth",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "ToYear",
                table: "UserWiseLoans");
        }
    }
}
