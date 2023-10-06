using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class LeaveHistoryModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LeaveId",
                table: "LeaveHistories",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 3, 12, 50, 34, 92, DateTimeKind.Local).AddTicks(1626), new DateTime(2021, 3, 3, 12, 50, 34, 89, DateTimeKind.Local).AddTicks(9675), new DateTime(2011, 3, 3, 12, 50, 34, 92, DateTimeKind.Local).AddTicks(4340) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 3, 12, 50, 34, 93, DateTimeKind.Local).AddTicks(74), new DateTime(2021, 3, 3, 12, 50, 34, 93, DateTimeKind.Local).AddTicks(46), new DateTime(2011, 3, 3, 12, 50, 34, 93, DateTimeKind.Local).AddTicks(119) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 3, 12, 50, 34, 93, DateTimeKind.Local).AddTicks(243), new DateTime(2021, 3, 3, 12, 50, 34, 93, DateTimeKind.Local).AddTicks(241), new DateTime(2011, 3, 3, 12, 50, 34, 93, DateTimeKind.Local).AddTicks(246) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveId",
                table: "LeaveHistories");

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
    }
}
