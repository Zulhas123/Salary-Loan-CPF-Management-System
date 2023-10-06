using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class AddNameBanglaToStation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameBangla",
                table: "Stations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameBangla",
                table: "Stations");
        }
    }
}
