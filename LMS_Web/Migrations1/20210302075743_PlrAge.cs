using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class PlrAge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_NextApprovedPersonId",
                table: "LeaveApplications");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "LeaveApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NextApprovedPersonId",
                table: "LeaveApplications",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(767)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Holiday",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlrAge",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_NextApprovedPersonId",
                table: "LeaveApplications",
                column: "NextApprovedPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_NextApprovedPersonId",
                table: "LeaveApplications");

            migrationBuilder.DropColumn(
                name: "PlrAge",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "LeaveApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "NextApprovedPersonId",
                table: "LeaveApplications",
                type: "varchar(767)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Holiday",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 17, 12, 17, 36, 948, DateTimeKind.Local).AddTicks(8955), new DateTime(2021, 2, 17, 12, 17, 36, 947, DateTimeKind.Local).AddTicks(6023), new DateTime(2011, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(1087) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(5500), new DateTime(2021, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(5474), new DateTime(2011, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(5536) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(5649), new DateTime(2021, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(5647), new DateTime(2011, 2, 17, 12, 17, 36, 949, DateTimeKind.Local).AddTicks(5652) });

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_NextApprovedPersonId",
                table: "LeaveApplications",
                column: "NextApprovedPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
