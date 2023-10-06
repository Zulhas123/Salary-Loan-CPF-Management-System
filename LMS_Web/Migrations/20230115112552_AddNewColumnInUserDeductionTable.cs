using Microsoft.EntityFrameworkCore.Migrations;

namespace LMS_Web.Migrations
{
    public partial class AddNewColumnInUserDeductionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSameEveryMonth",
                table: "UserDeductions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSameEveryMonth",
                table: "UserDeductions");
        }
    }
}
