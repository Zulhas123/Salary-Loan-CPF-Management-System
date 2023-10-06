using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class AddOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "SuspensionHistories",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "MainMenus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuspensionHistories_AppUserId",
                table: "SuspensionHistories",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuspensionHistories_AspNetUsers_AppUserId",
                table: "SuspensionHistories",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuspensionHistories_AspNetUsers_AppUserId",
                table: "SuspensionHistories");

            migrationBuilder.DropIndex(
                name: "IX_SuspensionHistories_AppUserId",
                table: "SuspensionHistories");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "MainMenus");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "SuspensionHistories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
