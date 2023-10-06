using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class AddSingInHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSignInHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    LoginDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSignInHistory", x => x.Id);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSignInHistory");

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
        }
    }
}
