using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class TableChangeMultipleColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameBangla",
                table: "Wing",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameBangla",
                table: "Section",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameBangla",
                table: "Designation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameBangla",
                table: "Department",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApproveDate",
                table: "ChildrenInfos",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNo",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNoBangla",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCodeBangla",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstNameBangala",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameBangala",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNameBangala",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameBangla",
                table: "Wing");

            migrationBuilder.DropColumn(
                name: "NameBangla",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "NameBangla",
                table: "Designation");

            migrationBuilder.DropColumn(
                name: "NameBangla",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "BankAccountNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankAccountNoBangla",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmployeeCodeBangla",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstNameBangala",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullNameBangala",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastNameBangala",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApproveDate",
                table: "ChildrenInfos",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
