using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class LeaveApplicationModifiedAgain_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachedFile",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LeaveId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachedFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachedFile_LeaveApplications_LeaveId",
                        column: x => x.LeaveId,
                        principalTable: "LeaveApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 14, 17, 32, 13, 699, DateTimeKind.Local).AddTicks(6176), new DateTime(2021, 3, 14, 17, 32, 13, 697, DateTimeKind.Local).AddTicks(5651), new DateTime(2011, 3, 14, 17, 32, 13, 699, DateTimeKind.Local).AddTicks(8574) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 14, 17, 32, 13, 700, DateTimeKind.Local).AddTicks(3517), new DateTime(2021, 3, 14, 17, 32, 13, 700, DateTimeKind.Local).AddTicks(3495), new DateTime(2011, 3, 14, 17, 32, 13, 700, DateTimeKind.Local).AddTicks(3549) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 14, 17, 32, 13, 700, DateTimeKind.Local).AddTicks(3633), new DateTime(2021, 3, 14, 17, 32, 13, 700, DateTimeKind.Local).AddTicks(3631), new DateTime(2011, 3, 14, 17, 32, 13, 700, DateTimeKind.Local).AddTicks(3635) });

            migrationBuilder.CreateIndex(
                name: "IX_AttachedFile_LeaveId",
                table: "AttachedFile",
                column: "LeaveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachedFile");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 13, 15, 51, 24, 729, DateTimeKind.Local).AddTicks(9479), new DateTime(2021, 3, 13, 15, 51, 24, 727, DateTimeKind.Local).AddTicks(9801), new DateTime(2011, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(2142) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(7604), new DateTime(2021, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(7578), new DateTime(2011, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(7638) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(7810), new DateTime(2021, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(7808), new DateTime(2011, 3, 13, 15, 51, 24, 730, DateTimeKind.Local).AddTicks(7813) });
        }
    }
}
