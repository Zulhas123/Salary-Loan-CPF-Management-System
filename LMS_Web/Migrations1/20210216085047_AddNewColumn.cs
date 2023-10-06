using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class AddNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Section_SectionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Wing_WingId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "CancellationRemarks",
                table: "LeaveApplications",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 16, 14, 50, 46, 595, DateTimeKind.Local).AddTicks(5274), new DateTime(2021, 2, 16, 14, 50, 46, 594, DateTimeKind.Local).AddTicks(251), new DateTime(2011, 2, 16, 14, 50, 46, 595, DateTimeKind.Local).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 16, 14, 50, 46, 596, DateTimeKind.Local).AddTicks(2889), new DateTime(2021, 2, 16, 14, 50, 46, 596, DateTimeKind.Local).AddTicks(2858), new DateTime(2011, 2, 16, 14, 50, 46, 596, DateTimeKind.Local).AddTicks(2922) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 16, 14, 50, 46, 596, DateTimeKind.Local).AddTicks(3028), new DateTime(2021, 2, 16, 14, 50, 46, 596, DateTimeKind.Local).AddTicks(3026), new DateTime(2011, 2, 16, 14, 50, 46, 596, DateTimeKind.Local).AddTicks(3031) });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Section_SectionId",
                table: "AspNetUsers",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Wing_WingId",
                table: "AspNetUsers",
                column: "WingId",
                principalTable: "Wing",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Section_SectionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Wing_WingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CancellationRemarks",
                table: "LeaveApplications");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 15, 15, 33, 55, 253, DateTimeKind.Local).AddTicks(1590), new DateTime(2021, 2, 15, 15, 33, 55, 250, DateTimeKind.Local).AddTicks(4846), new DateTime(2011, 2, 15, 15, 33, 55, 253, DateTimeKind.Local).AddTicks(5301) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3006), new DateTime(2021, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(2970), new DateTime(2011, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3059) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3219), new DateTime(2021, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3215), new DateTime(2011, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3223) });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Section_SectionId",
                table: "AspNetUsers",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Wing_WingId",
                table: "AspNetUsers",
                column: "WingId",
                principalTable: "Wing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
