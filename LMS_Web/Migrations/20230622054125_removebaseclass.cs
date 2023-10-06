using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class removebaseclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpfInfo_AspNetUsers_CreatedById",
                table: "CpfInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_CpfInfo_AspNetUsers_UpdatedById",
                table: "CpfInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentInfo_AspNetUsers_CreatedById",
                table: "InvestmentInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_InvestmentInfo_AspNetUsers_UpdatedById",
                table: "InvestmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentInfo_CreatedById",
                table: "InvestmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_InvestmentInfo_UpdatedById",
                table: "InvestmentInfo");

            migrationBuilder.DropIndex(
                name: "IX_CpfInfo_CreatedById",
                table: "CpfInfo");

            migrationBuilder.DropIndex(
                name: "IX_CpfInfo_UpdatedById",
                table: "CpfInfo");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "InvestmentInfo");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "InvestmentInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "InvestmentInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "InvestmentInfo");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CpfInfo");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "CpfInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "CpfInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "CpfInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "InvestmentInfo",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "InvestmentInfo",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "InvestmentInfo",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "InvestmentInfo",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CpfInfo",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "CpfInfo",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "CpfInfo",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "CpfInfo",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInfo_CreatedById",
                table: "InvestmentInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentInfo_UpdatedById",
                table: "InvestmentInfo",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CpfInfo_CreatedById",
                table: "CpfInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CpfInfo_UpdatedById",
                table: "CpfInfo",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_CpfInfo_AspNetUsers_CreatedById",
                table: "CpfInfo",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpfInfo_AspNetUsers_UpdatedById",
                table: "CpfInfo",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentInfo_AspNetUsers_CreatedById",
                table: "InvestmentInfo",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InvestmentInfo_AspNetUsers_UpdatedById",
                table: "InvestmentInfo",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
