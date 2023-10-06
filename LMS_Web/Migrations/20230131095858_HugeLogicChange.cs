using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class HugeLogicChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentInstallmentNoForCapital",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "CurrentInstallmentNoForInterest",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "IsStop",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "StopUntilMonth",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "StopUntilYear",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "CurrentInstallmentNo",
                table: "UserTaxes");

            migrationBuilder.AddColumn<int>(
                name: "FiscalYearId",
                table: "UserTaxes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserWiseLoanId",
                table: "UserInstallmentInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserTaxId",
                table: "TaxInstallmentInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYears", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTaxes_FiscalYearId",
                table: "UserTaxes",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInstallmentInfo_UserWiseLoanId",
                table: "UserInstallmentInfo",
                column: "UserWiseLoanId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxInstallmentInfo_UserTaxId",
                table: "TaxInstallmentInfo",
                column: "UserTaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxInstallmentInfo_UserTaxes_UserTaxId",
                table: "TaxInstallmentInfo",
                column: "UserTaxId",
                principalTable: "UserTaxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInstallmentInfo_UserWiseLoans_UserWiseLoanId",
                table: "UserInstallmentInfo",
                column: "UserWiseLoanId",
                principalTable: "UserWiseLoans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTaxes_FiscalYears_FiscalYearId",
                table: "UserTaxes",
                column: "FiscalYearId",
                principalTable: "FiscalYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxInstallmentInfo_UserTaxes_UserTaxId",
                table: "TaxInstallmentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInstallmentInfo_UserWiseLoans_UserWiseLoanId",
                table: "UserInstallmentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTaxes_FiscalYears_FiscalYearId",
                table: "UserTaxes");

            migrationBuilder.DropTable(
                name: "FiscalYears");

            migrationBuilder.DropIndex(
                name: "IX_UserTaxes_FiscalYearId",
                table: "UserTaxes");

            migrationBuilder.DropIndex(
                name: "IX_UserInstallmentInfo_UserWiseLoanId",
                table: "UserInstallmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_TaxInstallmentInfo_UserTaxId",
                table: "TaxInstallmentInfo");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "UserTaxes");

            migrationBuilder.DropColumn(
                name: "UserWiseLoanId",
                table: "UserInstallmentInfo");

            migrationBuilder.DropColumn(
                name: "UserTaxId",
                table: "TaxInstallmentInfo");

            migrationBuilder.AddColumn<int>(
                name: "CurrentInstallmentNoForCapital",
                table: "UserWiseLoans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentInstallmentNoForInterest",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsStop",
                table: "UserWiseLoans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StopUntilMonth",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StopUntilYear",
                table: "UserWiseLoans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentInstallmentNo",
                table: "UserTaxes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
