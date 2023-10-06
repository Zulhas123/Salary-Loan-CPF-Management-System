using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace LMS_Web.Migrations
{
    public partial class HistoryCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<string>(nullable: true),
                    EmployeeName = table.Column<string>(nullable: true),
                    Scale = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    BasicAllowance = table.Column<decimal>(nullable: false),
                    CurrentBasic = table.Column<decimal>(nullable: false),
                    BankAccountNo = table.Column<string>(nullable: true),
                    MedicalAllowance = table.Column<decimal>(nullable: false),
                    HouseRentAllowance = table.Column<decimal>(nullable: false),
                    DearnessAllowance = table.Column<decimal>(nullable: false),
                    MobileCellphoneAllowance = table.Column<decimal>(nullable: false),
                    TelephoneAllowance = table.Column<decimal>(nullable: false),
                    ChargeAllowance = table.Column<decimal>(nullable: false),
                    EducationAllowance = table.Column<decimal>(nullable: false),
                    HonoraryAllowance = table.Column<decimal>(nullable: false),
                    TravelingAllowance = table.Column<decimal>(nullable: false),
                    AdvanceAllowance = table.Column<decimal>(nullable: false),
                    TransportAllowance = table.Column<decimal>(nullable: false),
                    PrantikSubidha = table.Column<decimal>(nullable: false),
                    BonusRefund = table.Column<decimal>(nullable: false),
                    OthersAllowance = table.Column<decimal>(nullable: false),
                    TiffinAllowance = table.Column<decimal>(nullable: false),
                    WashAllowance = table.Column<decimal>(nullable: false),
                    ArrearsBasic = table.Column<decimal>(nullable: false),
                    FestivalAllowance = table.Column<decimal>(nullable: false),
                    CPFRegular = table.Column<decimal>(nullable: false),
                    CPFAdditional = table.Column<decimal>(nullable: false),
                    CPFArrears = table.Column<decimal>(nullable: false),
                    HouseRentDeduction = table.Column<decimal>(nullable: false),
                    ElectricBill = table.Column<decimal>(nullable: false),
                    GasBill = table.Column<decimal>(nullable: false),
                    WaterBill = table.Column<decimal>(nullable: false),
                    IncomeTaxAmount = table.Column<decimal>(nullable: false),
                    IncomeTaxInstallment = table.Column<string>(nullable: true),
                    CPFFirstCapital = table.Column<decimal>(nullable: false),
                    CPFFirstInstallment = table.Column<string>(nullable: true),
                    CPFSecondCapital = table.Column<decimal>(nullable: false),
                    CPFSecondInstallment = table.Column<string>(nullable: true),
                    MotorCycleFirstCapital = table.Column<decimal>(nullable: false),
                    MotorCycleFirstCapitalInstallment = table.Column<string>(nullable: true),
                    MotorCycleFirstInterest = table.Column<decimal>(nullable: false),
                    MotorCycleFirstInterestInstallment = table.Column<string>(nullable: true),
                    MotorCycleSecondCapital = table.Column<decimal>(nullable: false),
                    MotorCycleSecondCapitalInstallment = table.Column<string>(nullable: true),
                    MotorCycleSecondInterest = table.Column<decimal>(nullable: false),
                    MotorCycleSecondInterestInstallment = table.Column<string>(nullable: true),
                    CarFirstCapital = table.Column<decimal>(nullable: false),
                    CarFirstCapitalInstallment = table.Column<string>(nullable: true),
                    CarFirstInterest = table.Column<decimal>(nullable: false),
                    CarFirstInterestInstallment = table.Column<string>(nullable: true),
                    CarSecondCapital = table.Column<decimal>(nullable: false),
                    CarSecondCapitalInstallment = table.Column<string>(nullable: true),
                    CarSecondInterest = table.Column<decimal>(nullable: false),
                    CarSecondInterestInstallment = table.Column<string>(nullable: true),
                    HouseBuildingFirstCapital = table.Column<decimal>(nullable: false),
                    HouseBuildingFirstCapitalInstallment = table.Column<string>(nullable: true),
                    HouseBuildingFirstInterest = table.Column<decimal>(nullable: false),
                    HouseBuildingFirstInterestInstallment = table.Column<string>(nullable: true),
                    HouseBuildingSecondCapital = table.Column<decimal>(nullable: false),
                    HouseBuildingSecondCapitalInstallment = table.Column<string>(nullable: true),
                    HouseBuildingSecondInterest = table.Column<decimal>(nullable: false),
                    HouseBuildingSecondInterestInstallment = table.Column<string>(nullable: true),
                    HouseRepairingFirstCapital = table.Column<decimal>(nullable: false),
                    HouseRepairingFirstCapitalInstallment = table.Column<string>(nullable: true),
                    HouseRepairingFirstInterest = table.Column<decimal>(nullable: false),
                    HouseRepairingFirstInterestInstallment = table.Column<string>(nullable: true),
                    HouseRepairingSecondCapital = table.Column<decimal>(nullable: false),
                    HouseRepairingSecondCapitalInstallment = table.Column<string>(nullable: true),
                    HouseRepairingSecondInterest = table.Column<decimal>(nullable: false),
                    HouseRepairingSecondInterestInstallment = table.Column<string>(nullable: true),
                    OthersAdvanceCapital = table.Column<decimal>(nullable: false),
                    OthersAdvanceCapitalInstallment = table.Column<string>(nullable: true),
                    OthersAdvanceInterest = table.Column<decimal>(nullable: false),
                    OthersAdvanceInterestInstallment = table.Column<string>(nullable: true),
                    OthersCapital = table.Column<decimal>(nullable: false),
                    OthersCapitalInstallment = table.Column<string>(nullable: true),
                    OthersInterest = table.Column<decimal>(nullable: false),
                    OthersInterestInstallment = table.Column<string>(nullable: true),
                    HouseRepairingThirdCapital = table.Column<decimal>(nullable: false),
                    HouseRepairingThirdCapitalInstallment = table.Column<string>(nullable: true),
                    HouseRepairingThirdInterest = table.Column<decimal>(nullable: false),
                    HouseRepairingThirdInterestInstallment = table.Column<string>(nullable: true),
                    BasicDeduction = table.Column<decimal>(nullable: false),
                    Transport = table.Column<decimal>(nullable: false),
                    Garage = table.Column<decimal>(nullable: false),
                    GroupInsurance = table.Column<decimal>(nullable: false),
                    WelfareFund = table.Column<decimal>(nullable: false),
                    Rehabilitation = table.Column<decimal>(nullable: false),
                    GrossSalary = table.Column<decimal>(nullable: false),
                    TotalDeduction = table.Column<decimal>(nullable: false),
                    NetSalary = table.Column<decimal>(nullable: false),
                    NetInWord = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    NetSalaryBangla = table.Column<string>(nullable: true),
                    EmployeeNameBangla = table.Column<string>(nullable: true),
                    DesignationBangla = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalaryIncrement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryIncrement", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryHistory");

            migrationBuilder.DropTable(
                name: "SalaryIncrement");
        }
    }
}
