using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class AddApprovaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApprovalDesignation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalDesignation", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 22, 20, 13, 47, 344, DateTimeKind.Local).AddTicks(7655), new DateTime(2021, 3, 22, 20, 13, 47, 343, DateTimeKind.Local).AddTicks(3966), new DateTime(2011, 3, 22, 20, 13, 47, 344, DateTimeKind.Local).AddTicks(9894) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4838), new DateTime(2021, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4812), new DateTime(2011, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4882) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2",
                columns: new[] { "BirthDate", "CreatedDateTime", "JoiningDate" },
                values: new object[] { new DateTime(1991, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4979), new DateTime(2021, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4976), new DateTime(2011, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4982) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalDesignation");

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
    }
}
