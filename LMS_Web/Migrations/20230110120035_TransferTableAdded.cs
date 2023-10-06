using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class TransferTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentStationJoiningDate",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiedDate",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDied",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResignationDate",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TransferHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    FromStationId = table.Column<int>(nullable: false),
                    ToStationId = table.Column<int>(nullable: false),
                    TransferDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferHistories_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferHistories_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferHistories_Stations_FromStationId",
                        column: x => x.FromStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferHistories_Stations_ToStationId",
                        column: x => x.ToStationId,
                        principalTable: "Stations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferHistories_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_AppUserId",
                table: "TransferHistories",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_CreatedById",
                table: "TransferHistories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_FromStationId",
                table: "TransferHistories",
                column: "FromStationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_ToStationId",
                table: "TransferHistories",
                column: "ToStationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_UpdatedById",
                table: "TransferHistories",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferHistories");

            migrationBuilder.DropColumn(
                name: "CurrentStationJoiningDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DiedDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDied",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResignationDate",
                table: "AspNetUsers");
        }
    }
}
