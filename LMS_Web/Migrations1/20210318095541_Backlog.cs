using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using System;

namespace LMS_Web.Migrations
{
    public partial class Backlog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BacklogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ApplicantId = table.Column<string>(nullable: true),
                    LeaveTypeId = table.Column<int>(nullable: false),
                    Days = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogEntries", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 18, 15, 55, 40, 166, DateTimeKind.Local).AddTicks(8562), new DateTime(2021, 3, 18, 15, 55, 40, 164, DateTimeKind.Local).AddTicks(804), new DateTime(2011, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(2141) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(9638), new DateTime(2021, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(9598), new DateTime(2011, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(9711) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(9883), new DateTime(2021, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(9879), new DateTime(2011, 3, 18, 15, 55, 40, 167, DateTimeKind.Local).AddTicks(9888) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BacklogEntries");

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
    }
}
