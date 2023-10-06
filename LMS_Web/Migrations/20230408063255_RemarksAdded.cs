using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class RemarksAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "UserSpecificAllowances",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "UserDeductions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "TaxInstallmentInfo",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxInstallmentInfo_AppUserId",
                table: "TaxInstallmentInfo",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxInstallmentInfo_AspNetUsers_AppUserId",
                table: "TaxInstallmentInfo",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxInstallmentInfo_AspNetUsers_AppUserId",
                table: "TaxInstallmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_TaxInstallmentInfo_AppUserId",
                table: "TaxInstallmentInfo");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "UserSpecificAllowances");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "UserDeductions");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "TaxInstallmentInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
