using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class RemoveBaseClassFromFundTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFundInfo_AspNetUsers_CreatedById",
                table: "UserFundInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFundInfo_AspNetUsers_UpdatedById",
                table: "UserFundInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserFundInfo_CreatedById",
                table: "UserFundInfo");

            migrationBuilder.DropIndex(
                name: "IX_UserFundInfo_UpdatedById",
                table: "UserFundInfo");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "UserFundInfo");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "UserFundInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "UserFundInfo");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "UserFundInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "UserFundInfo",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "UserFundInfo",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "UserFundInfo",
                type: "varchar(767)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "UserFundInfo",
                type: "datetime",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFundInfo_CreatedById",
                table: "UserFundInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserFundInfo_UpdatedById",
                table: "UserFundInfo",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundInfo_AspNetUsers_CreatedById",
                table: "UserFundInfo",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFundInfo_AspNetUsers_UpdatedById",
                table: "UserFundInfo",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
