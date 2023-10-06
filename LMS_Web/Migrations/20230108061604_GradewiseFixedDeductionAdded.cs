using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class GradewiseFixedDeductionAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "UserHouseRents",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GradeWiseFixedDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    DeductionId = table.Column<int>(nullable: false),
                    FromGradeId = table.Column<int>(nullable: false),
                    ToGradeId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeWiseFixedDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeWiseFixedDeductions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradeWiseFixedDeductions_Deductions_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "Deductions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeWiseFixedDeductions_Grades_FromGradeId",
                        column: x => x.FromGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeWiseFixedDeductions_Grades_ToGradeId",
                        column: x => x.ToGradeId,
                        principalTable: "Grades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeWiseFixedDeductions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseRents_AppUserId",
                table: "UserHouseRents",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHouseRents_ResidentStatusId",
                table: "UserHouseRents",
                column: "ResidentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeWiseFixedDeductions_CreatedById",
                table: "GradeWiseFixedDeductions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GradeWiseFixedDeductions_DeductionId",
                table: "GradeWiseFixedDeductions",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeWiseFixedDeductions_FromGradeId",
                table: "GradeWiseFixedDeductions",
                column: "FromGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeWiseFixedDeductions_ToGradeId",
                table: "GradeWiseFixedDeductions",
                column: "ToGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeWiseFixedDeductions_UpdatedById",
                table: "GradeWiseFixedDeductions",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseRents_AspNetUsers_AppUserId",
                table: "UserHouseRents",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHouseRents_ResidentStatus_ResidentStatusId",
                table: "UserHouseRents",
                column: "ResidentStatusId",
                principalTable: "ResidentStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseRents_AspNetUsers_AppUserId",
                table: "UserHouseRents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHouseRents_ResidentStatus_ResidentStatusId",
                table: "UserHouseRents");

            migrationBuilder.DropTable(
                name: "GradeWiseFixedDeductions");

            migrationBuilder.DropIndex(
                name: "IX_UserHouseRents_AppUserId",
                table: "UserHouseRents");

            migrationBuilder.DropIndex(
                name: "IX_UserHouseRents_ResidentStatusId",
                table: "UserHouseRents");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "UserHouseRents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
