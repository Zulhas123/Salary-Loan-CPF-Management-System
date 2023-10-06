using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class LeaveHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OperationType",
                table: "ApprovedHistory",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 13, 23, 55, 75, DateTimeKind.Local).AddTicks(8157), new DateTime(2021, 3, 15, 13, 23, 55, 74, DateTimeKind.Local).AddTicks(30), new DateTime(2011, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(621) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(5815), new DateTime(2021, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(5792), new DateTime(2011, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(5851) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(5987), new DateTime(2021, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(5985), new DateTime(2011, 3, 15, 13, 23, 55, 76, DateTimeKind.Local).AddTicks(5991) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "ApprovedHistory");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(466), new DateTime(2021, 3, 15, 12, 21, 50, 989, DateTimeKind.Local).AddTicks(6125), new DateTime(2011, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(2389) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(6066), new DateTime(2021, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(6050), new DateTime(2011, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(6090) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(6156), new DateTime(2021, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(6155), new DateTime(2011, 3, 15, 12, 21, 50, 991, DateTimeKind.Local).AddTicks(6159) });
        }
    }
}
