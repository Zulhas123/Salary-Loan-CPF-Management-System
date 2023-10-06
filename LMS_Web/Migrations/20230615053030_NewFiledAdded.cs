using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class NewFiledAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MemorandumNo",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MemorandumNo",
                table: "ChildrenInfos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CPFStartDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemorandumNo",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "MemorandumNo",
                table: "ChildrenInfos");

            migrationBuilder.DropColumn(
                name: "CPFStartDate",
                table: "AspNetUsers");
        }
    }
}
