using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class referenceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Grades",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(767)")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_GradWisePayScales_GradeId",
                table: "GradWisePayScales",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradWisePayScales_PayScaleId",
                table: "GradWisePayScales",
                column: "PayScaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_GradWisePayScales_Grades_GradeId",
                table: "GradWisePayScales",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GradWisePayScales_PayScales_PayScaleId",
                table: "GradWisePayScales",
                column: "PayScaleId",
                principalTable: "PayScales",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GradWisePayScales_Grades_GradeId",
                table: "GradWisePayScales");

            migrationBuilder.DropForeignKey(
                name: "FK_GradWisePayScales_PayScales_PayScaleId",
                table: "GradWisePayScales");

            migrationBuilder.DropIndex(
                name: "IX_GradWisePayScales_GradeId",
                table: "GradWisePayScales");

            migrationBuilder.DropIndex(
                name: "IX_GradWisePayScales_PayScaleId",
                table: "GradWisePayScales");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Grades",
                type: "varchar(767)",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);
        }
    }
}
