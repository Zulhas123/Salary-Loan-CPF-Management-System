using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class idchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWiseLoans_AspNetUsers_AppUsersId",
                table: "UserWiseLoans");

            migrationBuilder.DropIndex(
                name: "IX_UserWiseLoans_AppUsersId",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "AppUsersId",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserWiseLoans");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWiseLoans_AppUserId",
                table: "UserWiseLoans",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWiseLoans_AspNetUsers_AppUserId",
                table: "UserWiseLoans",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWiseLoans_AspNetUsers_AppUserId",
                table: "UserWiseLoans");

            migrationBuilder.DropIndex(
                name: "IX_UserWiseLoans_AppUserId",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserWiseLoans");

            migrationBuilder.AddColumn<string>(
                name: "AppUsersId",
                table: "UserWiseLoans",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserWiseLoans",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWiseLoans_AppUsersId",
                table: "UserWiseLoans",
                column: "AppUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWiseLoans_AspNetUsers_AppUsersId",
                table: "UserWiseLoans",
                column: "AppUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
