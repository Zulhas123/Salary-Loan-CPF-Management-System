using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class LoanPaymentHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanPaymentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserWiseLoanId = table.Column<int>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaidAmount = table.Column<decimal>(nullable: false),
                    InstallmentNoBeforePaid = table.Column<int>(nullable: false),
                    Document = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanPaymentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanPaymentHistories_UserWiseLoans_UserWiseLoanId",
                        column: x => x.UserWiseLoanId,
                        principalTable: "UserWiseLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanPaymentHistories_UserWiseLoanId",
                table: "LoanPaymentHistories",
                column: "UserWiseLoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanPaymentHistories");
        }
    }
}
