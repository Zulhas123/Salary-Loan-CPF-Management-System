using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class TaxTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicStepNo",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentBasic",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTaxes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: false),
                    DeductedAmount = table.Column<decimal>(nullable: false),
                    MonthlyDeduction = table.Column<decimal>(nullable: false),
                    CurrentInstallmentNo = table.Column<int>(nullable: false),
                    TotalInstallment = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTaxes_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaxes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTaxes_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTaxes_AppUserId",
                table: "UserTaxes",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaxes_CreatedById",
                table: "UserTaxes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserTaxes_UpdatedById",
                table: "UserTaxes",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTaxes");

            migrationBuilder.DropColumn(
                name: "CurrentBasic",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BasicStepNo",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
