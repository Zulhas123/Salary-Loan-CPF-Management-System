using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class InvestmentModalChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalInvestment",
                table: "InvestmentInfo",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InvestmentAmount",
                table: "InvestmentInfo",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvestmentAmount",
                table: "InvestmentInfo");

            migrationBuilder.AlterColumn<string>(
                name: "TotalInvestment",
                table: "InvestmentInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(decimal));
        }
    }
}
