using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class tablenamechaneg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInstallmentInfo_UserWiseLoans_UserWiseLoanId",
                table: "UserInstallmentInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInstallmentInfo",
                table: "UserInstallmentInfo");

            migrationBuilder.RenameTable(
                name: "UserInstallmentInfo",
                newName: "LoanInstallmentInfo");

            migrationBuilder.RenameIndex(
                name: "IX_UserInstallmentInfo_UserWiseLoanId",
                table: "LoanInstallmentInfo",
                newName: "IX_LoanInstallmentInfo_UserWiseLoanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoanInstallmentInfo",
                table: "LoanInstallmentInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanInstallmentInfo_UserWiseLoans_UserWiseLoanId",
                table: "LoanInstallmentInfo",
                column: "UserWiseLoanId",
                principalTable: "UserWiseLoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanInstallmentInfo_UserWiseLoans_UserWiseLoanId",
                table: "LoanInstallmentInfo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoanInstallmentInfo",
                table: "LoanInstallmentInfo");

            migrationBuilder.RenameTable(
                name: "LoanInstallmentInfo",
                newName: "UserInstallmentInfo");

            migrationBuilder.RenameIndex(
                name: "IX_LoanInstallmentInfo_UserWiseLoanId",
                table: "UserInstallmentInfo",
                newName: "IX_UserInstallmentInfo_UserWiseLoanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInstallmentInfo",
                table: "UserInstallmentInfo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInstallmentInfo_UserWiseLoans_UserWiseLoanId",
                table: "UserInstallmentInfo",
                column: "UserWiseLoanId",
                principalTable: "UserWiseLoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
