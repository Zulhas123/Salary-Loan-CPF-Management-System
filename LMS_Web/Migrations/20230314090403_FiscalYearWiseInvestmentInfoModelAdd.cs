using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class FiscalYearWiseInvestmentInfoModelAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PrlDate",
                table: "PRlApplicantInfo",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FiscalYearWiseInvestmentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FiscalYearId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true),
                    InvestmentAmount = table.Column<decimal>(nullable: false),
                    InterestAmount = table.Column<decimal>(nullable: false),
                    Total = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYearWiseInvestmentInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiscalYearWiseInvestmentInfo_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FiscalYearWiseInvestmentInfo_FiscalYears_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalTable: "FiscalYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYearWiseInvestmentInfo_AppUserId",
                table: "FiscalYearWiseInvestmentInfo",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalYearWiseInvestmentInfo_FiscalYearId",
                table: "FiscalYearWiseInvestmentInfo",
                column: "FiscalYearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FiscalYearWiseInvestmentInfo");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PrlDate",
                table: "PRlApplicantInfo",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
