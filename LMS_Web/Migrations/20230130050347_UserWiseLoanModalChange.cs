using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class UserWiseLoanModalChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromMonth",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromYear",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToMonth",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToYear",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);
        }
    }
}
