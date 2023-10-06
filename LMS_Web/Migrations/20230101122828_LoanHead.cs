using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class LoanHead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUsersId",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LoanHead",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserWiseLoans_AppUsersId",
                table: "UserWiseLoans",
                column: "AppUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWiseLoans_LoanHeadId",
                table: "UserWiseLoans",
                column: "LoanHeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWiseLoans_AspNetUsers_AppUsersId",
                table: "UserWiseLoans",
                column: "AppUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWiseLoans_LoanHead_LoanHeadId",
                table: "UserWiseLoans",
                column: "LoanHeadId",
                principalTable: "LoanHead",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWiseLoans_AspNetUsers_AppUsersId",
                table: "UserWiseLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_UserWiseLoans_LoanHead_LoanHeadId",
                table: "UserWiseLoans");

            migrationBuilder.DropIndex(
                name: "IX_UserWiseLoans_AppUsersId",
                table: "UserWiseLoans");

            migrationBuilder.DropIndex(
                name: "IX_UserWiseLoans_LoanHeadId",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "AppUsersId",
                table: "UserWiseLoans");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "LoanHead",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
