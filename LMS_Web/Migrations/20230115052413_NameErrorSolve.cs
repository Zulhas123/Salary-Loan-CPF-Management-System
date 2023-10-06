using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class NameErrorSolve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameBangala",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullNameBangala",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastNameBangala",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameBangla",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameBangla",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNameBangla",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstNameBangla",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullNameBangla",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastNameBangla",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "FirstNameBangala",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameBangala",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNameBangala",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
