using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;
using System;

namespace LMS_Web.Migrations
{
    public partial class zxzadasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 127, nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EarnLeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarnLeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EarnLeave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Obtain = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    LastCalculationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarnLeave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EarnLeave_EarnLeaveTypes_Type",
                        column: x => x.Type,
                        principalTable: "EarnLeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 127, nullable: false),
                    RoleId = table.Column<string>(maxLength: 127, nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovedHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    LeaveApplicationId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovedHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 127, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 127, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 127, nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 127, nullable: false),
                    Name = table.Column<string>(maxLength: 127, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    WingId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Designation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveApplications",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    ApplicantId = table.Column<string>(nullable: true),
                    LeaveTypeId = table.Column<int>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    NextApprovedPersonId = table.Column<string>(nullable: true),
                    IsRejected = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    RejectedById = table.Column<string>(nullable: true),
                    RejectedDate = table.Column<DateTime>(nullable: true),
                    EarnLeaveType = table.Column<int>(nullable: false),
                    IsHalfToFull = table.Column<bool>(nullable: false),
                    TotalDays = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaternityLeave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    TakenTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaternityLeave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotDueLeave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    Obtain = table.Column<int>(nullable: false),
                    NonPaidAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotDueLeave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestAndRecreationLeave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    NextAvailableDate = table.Column<DateTime>(nullable: false),
                    TakenDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestAndRecreationLeave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialDisabilityLeave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    Obtain = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialDisabilityLeave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyLeave",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    Obtain = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyLeave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLeaveQuotas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<string>(nullable: true),
                    LeaveTypeId = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    LeaveObtain = table.Column<int>(nullable: false),
                    Year = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLeaveQuotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLeaveQuotas_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wing",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CreatedById = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<string>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    DesignationId = table.Column<int>(nullable: false),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    JoiningDate = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<string>(nullable: true),
                    NID = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    SectionId = table.Column<int>(nullable: true),
                    WingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Designation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Wing_WingId",
                        column: x => x.WingId,
                        principalTable: "Wing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedDateTime", "DepartmentId", "DesignationId", "Email", "EmailConfirmed", "EmployeeCode", "FirstName", "FullName", "Gender", "Image", "IsActive", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "NID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Religion", "SectionId", "SecurityStamp", "TwoFactorEnabled", "Type", "UpdatedBy", "UpdatedDateTime", "UserName", "WingId" },
                values: new object[] { "352ef6af-8b2e-4d0d-bb01-88dfa619f9f2", 0, new DateTime(1991, 2, 15, 15, 33, 55, 253, DateTimeKind.Local).AddTicks(1590), "c00fcf43-bb53-4671-91cd-2fb94c70f1cb", null, new DateTime(2021, 2, 15, 15, 33, 55, 250, DateTimeKind.Local).AddTicks(4846), 1, 1, "hmuzzal@mail.com", true, null, "Hasan", "Hasan Mahmud", "Male", "1.jpg", true, new DateTime(2011, 2, 15, 15, 33, 55, 253, DateTimeKind.Local).AddTicks(5301), "Mahmud", false, null, null, null, null, "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==", "01715637290", false, null, null, "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF", false, null, null, null, "01715637290", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedDateTime", "DepartmentId", "DesignationId", "Email", "EmailConfirmed", "EmployeeCode", "FirstName", "FullName", "Gender", "Image", "IsActive", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "NID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Religion", "SectionId", "SecurityStamp", "TwoFactorEnabled", "Type", "UpdatedBy", "UpdatedDateTime", "UserName", "WingId" },
                values: new object[] { "352ef6af-8b2e-4l0d-bb01-88dfa619f6o2", 0, new DateTime(1991, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3006), "c00fcf43-bb53-4671-91cd-2fb94c70f1cb", null, new DateTime(2021, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(2970), 1, 1, "hmuzzal@mail.com", true, null, "Safkat", "Safkat Mahmud", "Male", "2.jpg", true, new DateTime(2011, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3059), "Mahmud", false, null, null, null, null, "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==", "01715637291", false, null, null, "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF", false, null, null, null, "01715637291", null });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedDateTime", "DepartmentId", "DesignationId", "Email", "EmailConfirmed", "EmployeeCode", "FirstName", "FullName", "Gender", "Image", "IsActive", "JoiningDate", "LastName", "LockoutEnabled", "LockoutEnd", "NID", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Religion", "SectionId", "SecurityStamp", "TwoFactorEnabled", "Type", "UpdatedBy", "UpdatedDateTime", "UserName", "WingId" },
                values: new object[] { "352ef6af-8t3e-4l0d-bb01-88dfa619qwo2", 0, new DateTime(1991, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3219), "c00fcf43-bb53-4671-91cd-2fb94c70f1cb", null, new DateTime(2021, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3215), 1, 1, "hmuzzal@mail.com", true, null, "Asaduzzaman", "Asaduzzaman Khan", "Male", "3.jpg", true, new DateTime(2011, 2, 15, 15, 33, 55, 254, DateTimeKind.Local).AddTicks(3223), "Khan", false, null, null, null, null, "AQAAAAEAACcQAAAAEIW60+N8AuqIonyaDD/ODWNY/GCLpkM2khiNDoTwsWZEtyg+iIjuAgGIej2cqvNiKA==", "01715637292", false, null, null, "IEC2QG3OUXJYUJGNKQBKIWXFGXKHVEXF", false, null, null, null, "01715637292", null });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedHistory_CreatedById",
                table: "ApprovedHistory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedHistory_LeaveApplicationId",
                table: "ApprovedHistory",
                column: "LeaveApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovedHistory_UpdatedById",
                table: "ApprovedHistory",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_AppUserId",
                table: "AspNetUserClaims",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_AppUserId",
                table: "AspNetUserLogins",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_AppUserId",
                table: "AspNetUserRoles",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DesignationId",
                table: "AspNetUsers",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SectionId",
                table: "AspNetUsers",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WingId",
                table: "AspNetUsers",
                column: "WingId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_AppUserId",
                table: "AspNetUserTokens",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CreatedById",
                table: "Department",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Department_UpdatedById",
                table: "Department",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Department_WingId",
                table: "Department",
                column: "WingId");

            migrationBuilder.CreateIndex(
                name: "IX_Designation_CreatedById",
                table: "Designation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Designation_UpdatedById",
                table: "Designation",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_EarnLeave_Type",
                table: "EarnLeave",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_ApplicantId",
                table: "LeaveApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_CreatedById",
                table: "LeaveApplications",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_LeaveTypeId",
                table: "LeaveApplications",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_NextApprovedPersonId",
                table: "LeaveApplications",
                column: "NextApprovedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_RejectedById",
                table: "LeaveApplications",
                column: "RejectedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveApplications_UpdatedById",
                table: "LeaveApplications",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveReason_CreatedById",
                table: "LeaveReason",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveReason_UpdatedById",
                table: "LeaveReason",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_CreatedById",
                table: "LeaveType",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveType_UpdatedById",
                table: "LeaveType",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MaternityLeave_AppUserId",
                table: "MaternityLeave",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotDueLeave_AppUserId",
                table: "NotDueLeave",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RestAndRecreationLeave_AppUserId",
                table: "RestAndRecreationLeave",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_CreatedById",
                table: "Section",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Section_DepartmentId",
                table: "Section",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_UpdatedById",
                table: "Section",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialDisabilityLeave_AppUserId",
                table: "SpecialDisabilityLeave",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyLeave_AppUserId",
                table: "StudyLeave",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLeaveQuotas_EmployeeId",
                table: "UserLeaveQuotas",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLeaveQuotas_LeaveTypeId",
                table: "UserLeaveQuotas",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wing_CreatedById",
                table: "Wing",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Wing_UpdatedById",
                table: "Wing",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_AppUserId",
                table: "AspNetUserRoles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedHistory_AspNetUsers_CreatedById",
                table: "ApprovedHistory",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedHistory_AspNetUsers_UpdatedById",
                table: "ApprovedHistory",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovedHistory_LeaveApplications_LeaveApplicationId",
                table: "ApprovedHistory",
                column: "LeaveApplicationId",
                principalTable: "LeaveApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_AppUserId",
                table: "AspNetUserClaims",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_AppUserId",
                table: "AspNetUserLogins",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_AppUserId",
                table: "AspNetUserTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_AspNetUsers_CreatedById",
                table: "Department",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_AspNetUsers_UpdatedById",
                table: "Department",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Wing_WingId",
                table: "Department",
                column: "WingId",
                principalTable: "Wing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Designation_AspNetUsers_CreatedById",
                table: "Designation",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Designation_AspNetUsers_UpdatedById",
                table: "Designation",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_ApplicantId",
                table: "LeaveApplications",
                column: "ApplicantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_CreatedById",
                table: "LeaveApplications",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_NextApprovedPersonId",
                table: "LeaveApplications",
                column: "NextApprovedPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_RejectedById",
                table: "LeaveApplications",
                column: "RejectedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_AspNetUsers_UpdatedById",
                table: "LeaveApplications",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveApplications_LeaveType_LeaveTypeId",
                table: "LeaveApplications",
                column: "LeaveTypeId",
                principalTable: "LeaveType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveReason_AspNetUsers_CreatedById",
                table: "LeaveReason",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveReason_AspNetUsers_UpdatedById",
                table: "LeaveReason",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveType_AspNetUsers_CreatedById",
                table: "LeaveType",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveType_AspNetUsers_UpdatedById",
                table: "LeaveType",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaternityLeave_AspNetUsers_AppUserId",
                table: "MaternityLeave",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NotDueLeave_AspNetUsers_AppUserId",
                table: "NotDueLeave",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RestAndRecreationLeave_AspNetUsers_AppUserId",
                table: "RestAndRecreationLeave",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_AspNetUsers_CreatedById",
                table: "Section",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Section_AspNetUsers_UpdatedById",
                table: "Section",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecialDisabilityLeave_AspNetUsers_AppUserId",
                table: "SpecialDisabilityLeave",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyLeave_AspNetUsers_AppUserId",
                table: "StudyLeave",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLeaveQuotas_AspNetUsers_EmployeeId",
                table: "UserLeaveQuotas",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wing_AspNetUsers_CreatedById",
                table: "Wing",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wing_AspNetUsers_UpdatedById",
                table: "Wing",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_AspNetUsers_CreatedById",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_AspNetUsers_UpdatedById",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Designation_AspNetUsers_CreatedById",
                table: "Designation");

            migrationBuilder.DropForeignKey(
                name: "FK_Designation_AspNetUsers_UpdatedById",
                table: "Designation");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_AspNetUsers_CreatedById",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_AspNetUsers_UpdatedById",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Wing_AspNetUsers_CreatedById",
                table: "Wing");

            migrationBuilder.DropForeignKey(
                name: "FK_Wing_AspNetUsers_UpdatedById",
                table: "Wing");

            migrationBuilder.DropTable(
                name: "ApprovedHistory");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EarnLeave");

            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "LeaveReason");

            migrationBuilder.DropTable(
                name: "MaternityLeave");

            migrationBuilder.DropTable(
                name: "NotDueLeave");

            migrationBuilder.DropTable(
                name: "RestAndRecreationLeave");

            migrationBuilder.DropTable(
                name: "SpecialDisabilityLeave");

            migrationBuilder.DropTable(
                name: "StudyLeave");

            migrationBuilder.DropTable(
                name: "UserLeaveQuotas");

            migrationBuilder.DropTable(
                name: "LeaveApplications");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "EarnLeaveTypes");

            migrationBuilder.DropTable(
                name: "LeaveType");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Designation");

            migrationBuilder.DropTable(
                name: "Section");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Wing");
        }
    }
}
