using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class UserSpecificAllowanceTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserSpecific",
                table: "PayScales",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserSpecificAllowances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    PayScaleId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSpecificAllowances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSpecificAllowances_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSpecificAllowances_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserSpecificAllowances_PayScales_PayScaleId",
                        column: x => x.PayScaleId,
                        principalTable: "PayScales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSpecificAllowances_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificAllowances_AppUserId",
                table: "UserSpecificAllowances",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificAllowances_CreatedById",
                table: "UserSpecificAllowances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificAllowances_PayScaleId",
                table: "UserSpecificAllowances",
                column: "PayScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSpecificAllowances_UpdatedById",
                table: "UserSpecificAllowances",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSpecificAllowances");

            migrationBuilder.DropColumn(
                name: "IsUserSpecific",
                table: "PayScales");
        }
    }
}
