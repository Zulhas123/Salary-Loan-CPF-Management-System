using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class TableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "GradWisePayScales");

            migrationBuilder.AddColumn<int>(
                name: "CurrentInstallmentNoForInterest",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "UserDeductions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "UserDeductions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "UserDeductions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoleSubMenus",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "FixedAmount",
                table: "GradWisePayScales",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaximumAmount",
                table: "GradWisePayScales",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumAmount",
                table: "GradWisePayScales",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLien",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuspended",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DueDuringSuspensions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppuserId = table.Column<string>(nullable: true),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DueDuringSuspensions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FixedDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    DeductionId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedDeductions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedDeductions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LienUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LienUsers_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LienUsers_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuspensionHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspensionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspensionHistories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuspensionHistories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeductions_CreatedById",
                table: "FixedDeductions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeductions_UpdatedById",
                table: "FixedDeductions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LienUsers_CreatedById",
                table: "LienUsers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LienUsers_UpdatedById",
                table: "LienUsers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SuspensionHistories_CreatedById",
                table: "SuspensionHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SuspensionHistories_UpdatedById",
                table: "SuspensionHistories",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DueDuringSuspensions");

            migrationBuilder.DropTable(
                name: "FixedDeductions");

            migrationBuilder.DropTable(
                name: "LienUsers");

            migrationBuilder.DropTable(
                name: "SuspensionHistories");

            migrationBuilder.DropColumn(
                name: "CurrentInstallmentNoForInterest",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "UserDeductions");

            migrationBuilder.DropColumn(
                name: "FixedAmount",
                table: "GradWisePayScales");

            migrationBuilder.DropColumn(
                name: "MaximumAmount",
                table: "GradWisePayScales");

            migrationBuilder.DropColumn(
                name: "MinimumAmount",
                table: "GradWisePayScales");

            migrationBuilder.DropColumn(
                name: "IsLien",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsSuspended",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserDeductions",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "RoleSubMenus",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "GradWisePayScales",
                type: "decimal(18, 2)",
                nullable: true);
        }
    }
}
