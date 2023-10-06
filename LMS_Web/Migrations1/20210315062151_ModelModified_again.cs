using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class ModelModified_again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentPath",
                table: "LeaveApplications");

            migrationBuilder.AddColumn<string>(
                name: "DocumentPath",
                table: "AttachedFile",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentPath",
                table: "AttachedFile");

            migrationBuilder.AddColumn<string>(
                name: "DocumentPath",
                table: "LeaveApplications",
                type: "text",
                nullable: true);

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
        }
    }
}
