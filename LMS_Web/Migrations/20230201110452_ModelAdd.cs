using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class ModelAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CpfInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    BasicSalary = table.Column<decimal>(nullable: false),
                    SelfContribution = table.Column<decimal>(nullable: false),
                    GovtContribution = table.Column<decimal>(nullable: false),
                    ArrearsBasic = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpfInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpfInfo_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CpfInfo_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CpfInfo_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    TotalInvestment = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentInfo_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentInfo_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestmentInfo_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFundInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    WelfareFund = table.Column<string>(nullable: true),
                    GroupInsurance = table.Column<string>(nullable: true),
                    Rehabilitation = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFundInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFundInfo_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFundInfo_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFundInfo_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpfInfo_AppUserId",
                table: "CpfInfo",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CpfInfo_CreatedById",
                table: "CpfInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CpfInfo_UpdatedById",
                table: "CpfInfo",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInfo_AppUserId",
                table: "InvestmentInfo",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInfo_CreatedById",
                table: "InvestmentInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInfo_UpdatedById",
                table: "InvestmentInfo",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundInfo_AppUserId",
                table: "UserFundInfo",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundInfo_CreatedById",
                table: "UserFundInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundInfo_UpdatedById",
                table: "UserFundInfo",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CpfInfo");

            migrationBuilder.DropTable(
                name: "InvestmentInfo");

            migrationBuilder.DropTable(
                name: "UserFundInfo");
        }
    }
}
