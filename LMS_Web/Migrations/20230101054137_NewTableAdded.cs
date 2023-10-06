using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class NewTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChildrenInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    IsApprove = table.Column<bool>(nullable: false),
                    ApproveDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChildrenInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChildrenInfos_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChildrenInfos_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChildrenInfos_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserStationPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    StationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStationPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStationPermissions_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserStationPermissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserStationPermissions_Stations_StationId",
                        column: x => x.StationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStationPermissions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeductions_DeductionId",
                table: "FixedDeductions",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenInfos_AppUserId",
                table: "ChildrenInfos",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenInfos_CreatedById",
                table: "ChildrenInfos",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChildrenInfos_UpdatedById",
                table: "ChildrenInfos",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserStationPermissions_AppUserId",
                table: "UserStationPermissions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStationPermissions_CreatedById",
                table: "UserStationPermissions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserStationPermissions_StationId",
                table: "UserStationPermissions",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStationPermissions_UpdatedById",
                table: "UserStationPermissions",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedDeductions_Deductions_DeductionId",
                table: "FixedDeductions",
                column: "DeductionId",
                principalTable: "Deductions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedDeductions_Deductions_DeductionId",
                table: "FixedDeductions");

            migrationBuilder.DropTable(
                name: "ChildrenInfos");

            migrationBuilder.DropTable(
                name: "UserStationPermissions");

            migrationBuilder.DropIndex(
                name: "IX_FixedDeductions_DeductionId",
                table: "FixedDeductions");
        }
    }
}
