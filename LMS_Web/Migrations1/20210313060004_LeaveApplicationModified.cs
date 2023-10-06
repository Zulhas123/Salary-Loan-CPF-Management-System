using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class LeaveApplicationModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherReason",
                table: "LeaveApplications",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 13, 12, 0, 2, 824, DateTimeKind.Local).AddTicks(398), new DateTime(2021, 3, 13, 12, 0, 2, 821, DateTimeKind.Local).AddTicks(3315), new DateTime(2011, 3, 13, 12, 0, 2, 824, DateTimeKind.Local).AddTicks(4161) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 13, 12, 0, 2, 825, DateTimeKind.Local).AddTicks(2019), new DateTime(2021, 3, 13, 12, 0, 2, 825, DateTimeKind.Local).AddTicks(1978), new DateTime(2011, 3, 13, 12, 0, 2, 825, DateTimeKind.Local).AddTicks(2072) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 13, 12, 0, 2, 825, DateTimeKind.Local).AddTicks(2254), new DateTime(2021, 3, 13, 12, 0, 2, 825, DateTimeKind.Local).AddTicks(2251), new DateTime(2011, 3, 13, 12, 0, 2, 825, DateTimeKind.Local).AddTicks(2258) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherReason",
                table: "LeaveApplications");

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
    }
}
