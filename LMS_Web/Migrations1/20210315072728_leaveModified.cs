using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class leaveModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "LeaveApplications",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(2181), new DateTime(2021, 3, 15, 13, 27, 27, 59, DateTimeKind.Local).AddTicks(4611), new DateTime(2011, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(4606) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(9444), new DateTime(2021, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(9420), new DateTime(2011, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(9480) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(9589), new DateTime(2021, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(9586), new DateTime(2011, 3, 15, 13, 27, 27, 61, DateTimeKind.Local).AddTicks(9591) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "LeaveApplications");

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
    }
}
