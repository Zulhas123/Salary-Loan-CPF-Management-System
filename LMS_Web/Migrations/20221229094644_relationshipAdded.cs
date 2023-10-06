using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class relationshipAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserDeductions_DeductionId",
                table: "UserDeductions",
                column: "DeductionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeductions_Deductions_DeductionId",
                table: "UserDeductions",
                column: "DeductionId",
                principalTable: "Deductions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDeductions_Deductions_DeductionId",
                table: "UserDeductions");

            migrationBuilder.DropIndex(
                name: "IX_UserDeductions_DeductionId",
                table: "UserDeductions");
        }
    }
}
