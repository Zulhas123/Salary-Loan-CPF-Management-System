using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class AddLeaveHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveHistories", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(1517), new DateTime(2021, 3, 3, 12, 25, 47, 26, DateTimeKind.Local).AddTicks(9476), new DateTime(2011, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(4175) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(9521), new DateTime(2021, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(9493), new DateTime(2011, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(9555) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(9665), new DateTime(2021, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(9663), new DateTime(2011, 3, 3, 12, 25, 47, 29, DateTimeKind.Local).AddTicks(9668) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaveHistories");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 2, 13, 57, 42, 941, DateTimeKind.Local).AddTicks(8004), new DateTime(2021, 3, 2, 13, 57, 42, 940, DateTimeKind.Local).AddTicks(6612), new DateTime(2011, 3, 2, 13, 57, 42, 941, DateTimeKind.Local).AddTicks(9703) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 2, 13, 57, 42, 942, DateTimeKind.Local).AddTicks(3334), new DateTime(2021, 3, 2, 13, 57, 42, 942, DateTimeKind.Local).AddTicks(3316), new DateTime(2011, 3, 2, 13, 57, 42, 942, DateTimeKind.Local).AddTicks(3365) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 2, 13, 57, 42, 942, DateTimeKind.Local).AddTicks(3433), new DateTime(2021, 3, 2, 13, 57, 42, 942, DateTimeKind.Local).AddTicks(3432), new DateTime(2011, 3, 2, 13, 57, 42, 942, DateTimeKind.Local).AddTicks(3435) });
        }
    }
}
