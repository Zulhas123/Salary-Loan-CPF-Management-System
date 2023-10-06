using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class Salary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Designation_DesignationId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2");

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Designation",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Designation",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EarnLeaveTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EarnLeaveTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Holiday",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Holiday",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "LeaveType",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Department",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Designation",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "BasicStepNo",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidentialStatusId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Deductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    DeductionType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GradeSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GradeId = table.Column<int>(nullable: false),
                    StepNo = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradWisePayScales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    GradeId = table.Column<int>(nullable: false),
                    PayScaleId = table.Column<int>(nullable: false),
                    IsFixed = table.Column<bool>(nullable: false),
                    Amount = table.Column<decimal>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradWisePayScales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradWisePayScales_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GradWisePayScales_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LoanHead",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<int>(nullable: false),
                    IsWithInterest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanHead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayScales",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayScales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResidentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidentStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDeductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    DeductionId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDeductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDeductions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDeductions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWiseLoans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    LoanHeadId = table.Column<int>(nullable: false),
                    LoanAmount = table.Column<decimal>(nullable: false),
                    NoOfInstallment = table.Column<int>(nullable: false),
                    CurrentInstallmentNoForCapital = table.Column<int>(nullable: false),
                    CapitalDeductionAmount = table.Column<decimal>(nullable: false),
                    InterestDeductionAmount = table.Column<decimal>(nullable: true),
                    IsPaid = table.Column<bool>(nullable: false),
                    IsStop = table.Column<bool>(nullable: false),
                    StopUntilMonth = table.Column<int>(nullable: true),
                    StopUntilYear = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWiseLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWiseLoans_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWiseLoans_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_CreatedById",
                table: "Grades",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_UpdatedById",
                table: "Grades",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GradWisePayScales_CreatedById",
                table: "GradWisePayScales",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GradWisePayScales_UpdatedById",
                table: "GradWisePayScales",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_CreatedById",
                table: "Stations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_UpdatedById",
                table: "Stations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeductions_CreatedById",
                table: "UserDeductions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserDeductions_UpdatedById",
                table: "UserDeductions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserWiseLoans_CreatedById",
                table: "UserWiseLoans",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserWiseLoans_UpdatedById",
                table: "UserWiseLoans",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Designation_DesignationId",
                table: "AspNetUsers",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Designation_DesignationId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Deductions");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "GradeSteps");

            migrationBuilder.DropTable(
                name: "GradWisePayScales");

            migrationBuilder.DropTable(
                name: "LoanHead");

            migrationBuilder.DropTable(
                name: "PayScales");

            migrationBuilder.DropTable(
                name: "ResidentStatus");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropTable(
                name: "UserDeductions");

            migrationBuilder.DropTable(
                name: "UserWiseLoans");

            migrationBuilder.DropColumn(
                name: "BasicStepNo",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ResidentialStatusId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3", "3", "User", "User" },
                    { "1", "1", "Super Admin", "Super Admin" },
                    { "2", "2", "Admin", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Department",
                columns: new[] { "Id", "CreatedById", "CreatedDateTime", "IsActive", "Name", "UpdatedById", "UpdatedDateTime", "WingId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "IT", null, null, null },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Account & Finance", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Designation",
                columns: new[] { "Id", "CreatedById", "CreatedDateTime", "Name", "UpdatedById", "UpdatedDateTime" },
                values: new object[,]
                {
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Director", null, null },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Analyst", null, null },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Programmer", null, null }
                });

            migrationBuilder.InsertData(
                table: "EarnLeaveTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "গড়" },
                    { 2, "অর্ধ গড়" }
                });

            migrationBuilder.InsertData(
                table: "Holiday",
                columns: new[] { "Id", "FromDate", "Name", "Remarks", "ToDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "একুশে ফেব্রুয়ারি", "", new DateTime(2021, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2021, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "২৬শে মার্চ", "", new DateTime(2021, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "LeaveType",
                columns: new[] { "Id", "CreatedById", "CreatedDateTime", "Name", "UpdatedById", "UpdatedDateTime" },
                values: new object[,]
                {
                    { 14, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "বিনা বেতনে", null, null },
                    { 13, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "বাধ্যতামূলক", null, null },
                    { 12, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "অক্ষমতাজনিত বিশেষ", null, null },
                    { 11, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "শ্রান্তি ও বিনোদন", null, null },
                    { 10, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ঐচ্ছিক", null, null },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "সংগনিরোধ", null, null },
                    { 8, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "প্রাপ্যতাবিহীন", null, null },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "প্রসূতি", null, null },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "অধ্যয়ন", null, null },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "অর্জিত", null, null },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "অসুস্থতাজনিত", null, null },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "নৈমিত্তিক", null, null },
                    { 9, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "অবসর উত্তর", null, null },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "অসাধারণ", null, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedDateTime", "DepartmentId", "DesignationId", "Email", "EmailConfirmed", "EmployeeCode", "FirstName", "FullName", "Gender", "Image", "IsActive", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "NID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PlrAge", "Religion", "SectionId", "SecurityStamp", "TwoFactorEnabled", "Type", "UpdatedBy", "UpdatedDateTime", "UserName", "WingId" },
                values: new object[] { "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2", 0, new DateTime(1991, 3, 22, 20, 13, 47, 344, DateTimeKind.Local).AddTicks(7655), "c00fcf43-bb53-4671-91cd-2fb94c70f1cb", null, new DateTime(2021, 3, 22, 20, 13, 47, 343, DateTimeKind.Local).AddTicks(3966), 1, 1, "hmuzzal@mail.com", true, null, "Hasan", "Hasan Mahmud", "Male", "1.jpg", true, new DateTime(2011, 3, 22, 20, 13, 47, 344, DateTimeKind.Local).AddTicks(9894), "Mahmud", false, null, null, null, null, "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==", "01715637290", false, 0, null, null, "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF", false, null, null, null, "01715637290", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedDateTime", "DepartmentId", "DesignationId", "Email", "EmailConfirmed", "EmployeeCode", "FirstName", "FullName", "Gender", "Image", "IsActive", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "NID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PlrAge", "Religion", "SectionId", "SecurityStamp", "TwoFactorEnabled", "Type", "UpdatedBy", "UpdatedDateTime", "UserName", "WingId" },
                values: new object[] { "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2", 0, new DateTime(1991, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4838), "c00fcf43-bb53-4671-91cd-2fb94c70f1cb", null, new DateTime(2021, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4812), 1, 1, "hmuzzal@mail.com", true, null, "Safkat", "Safkat Mahmud", "Male", "2.jpg", true, new DateTime(2011, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4882), "Mahmud", false, null, null, null, null, "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==", "01715637291", false, 0, null, null, "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF", false, null, null, null, "01715637291", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedDateTime", "DepartmentId", "DesignationId", "Email", "EmailConfirmed", "EmployeeCode", "FirstName", "FullName", "Gender", "Image", "IsActive", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "NID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PlrAge", "Religion", "SectionId", "SecurityStamp", "TwoFactorEnabled", "Type", "UpdatedBy", "UpdatedDateTime", "UserName", "WingId" },
                values: new object[] { "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2", 0, new DateTime(1991, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4979), "c00fcf43-bb53-4671-91cd-2fb94c70f1cb", null, new DateTime(2021, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4976), 1, 1, "hmuzzal@mail.com", true, null, "Asaduzzaman", "Asaduzzaman Khan", "Male", "3.jpg", true, new DateTime(2011, 3, 22, 20, 13, 47, 345, DateTimeKind.Local).AddTicks(4982), "Khan", false, null, null, null, null, "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==", "01715637292", false, 0, null, null, "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF", false, null, null, null, "01715637292", null });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Department_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Designation_DesignationId",
                table: "AspNetUsers",
                column: "DesignationId",
                principalTable: "Designation",
                principalColumn: "Id");
        }
    }
}
