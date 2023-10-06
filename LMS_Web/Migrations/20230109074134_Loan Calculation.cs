using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class LoanCalculation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWithInterest",
                table: "LoanHead");

            migrationBuilder.AddColumn<int>(
                name: "NoOfInstallmentForInterest",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoanHeadType",
                table: "LoanHead",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfInstallmentForInterest",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "LoanHeadType",
                table: "LoanHead");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithInterest",
                table: "LoanHead",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
