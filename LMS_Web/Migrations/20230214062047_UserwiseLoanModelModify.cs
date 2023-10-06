using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class UserwiseLoanModelModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "UserWiseLoans",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ApproveDate",
                table: "UserWiseLoans",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprove",
                table: "UserWiseLoans",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRefundable",
                table: "UserWiseLoans",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "ApproveDate",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "IsApprove",
                table: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "IsRefundable",
                table: "UserWiseLoans");
        }
    }
}
