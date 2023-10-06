using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Salary.Enum;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Salary.ViewModels;
using LMS_Web.Areas.Settings.Manager;
using LMS_Web.Data;
using LMS_Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Common;
using LMS_Web.Manager;
using Microsoft.EntityFrameworkCore.Query;
using StationManager = LMS_Web.Areas.Salary.Manager.StationManager;
using LMS_Web.Areas.CPF.ViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.Reporting.NETCore;
using LocalReport = Microsoft.Reporting.NETCore.LocalReport;
using LMS_Web.Areas.Settings.Interface;
using LMS_Web.Areas.Loan.Interface;
using LMS_Web.Areas.Salary.Interface.Manager;
using LMS_Web.Interface.Manager;
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json;

namespace LMS_Web.Areas.Salary.Controllers
{
    [Area("Salary")]
    public class SalaryController : Controller
    {

        private decimal sumOfNetSalary = 0;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGradeWisePayScaleManager _gradeWisePayScaleManager;
        private readonly IPayScaleManager _payScaleManager;
        private readonly IHouseRentRuleManager _houseRentRuleManager;
        private readonly IChildrenInfoManager _childrenInfoManager;
        private readonly IDeductionManager _deductionManager;
        private readonly IFixedDeductionManager _fixedDeductionManager;
        private readonly IUserDeductionManager _userDeductionManager;
        private readonly IUserHouseRentManager _userHouseRentManager;
        private readonly IUserTaxManager _userTaxManager;
        private readonly ILoanHeadManager _loanHeadManager;
        private readonly IUserWiseLoanManager _userWiseLoanManager;
        private readonly IGradeWiseFixedDeductionManager _gradeWiseFixedDeductionManager;
        private readonly IUserSpecificAllowanceManager _userSpecificAllowanceManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IWingsManager _wingsManager;
        private readonly StationManager _stationManager;
        private readonly IGradeStepBasicManager _gradeStepManager;
        private readonly IGradeManager _gradeManager;
        private readonly ICpfPercentManager _cpfPercentManager;
        private readonly ITransferHistoryManager _transferHistoryManager;
        private readonly ISuspensionHistoryManager _suspensionHistoryManager;
        private readonly IFiscalYearManager _fiscalYearManager;
        private readonly ITaxInstallmentInfoManager _taxInstallmentInfoManager;
        private readonly ILoanInstallmentInfoManager _loanInstallmentInfoManager;
        private readonly IUserStationPermissionManager _userStationPermissionManager;
        private readonly PRlApplicantInfoManager _pRlApplicantInfoManager;
        private readonly IProcessSalaryManager processSalaryManager;
        private readonly ISalaryHistoryManager salaryHistoryManager;



        public SalaryController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IWebHostEnvironment _webHostEnvironment)
        {
            webHostEnvironment = _webHostEnvironment;
            _userManager = userManager;
            _gradeWisePayScaleManager = new GradeWisePayScaleManager(dbContext);
            _payScaleManager = new PayScaleManager(dbContext);
            _houseRentRuleManager = new HouseRentRuleManager(dbContext);
            _childrenInfoManager = new ChildrenInfoManager(dbContext);
            _deductionManager = new DeductionManager(dbContext);
            _fixedDeductionManager = new FixedDeductionManager(dbContext);
            _userDeductionManager = new UserDeductionManager(dbContext);
            _userHouseRentManager = new UserHouseRentManager(dbContext);
            _userTaxManager = new UserTaxManager(dbContext);
            _loanHeadManager = new LoanHeadManager(dbContext);
            _userWiseLoanManager = new UserWiseLoanManager(dbContext);
            _gradeWiseFixedDeductionManager = new GradeWiseFixedDeductionManager(dbContext);
            _userSpecificAllowanceManager = new UserSpecificAllowanceManager(dbContext);
            _wingsManager = new WingsManager(dbContext);
            _stationManager = new StationManager(dbContext);
            _gradeStepManager = new GradeStepBasicManager(dbContext);
            _gradeManager = new GradeManager(dbContext);
            _cpfPercentManager = new CpfPercentManager(dbContext);
            _transferHistoryManager = new TransferHistoryManager(dbContext);
            _suspensionHistoryManager = new SuspensionHistoryManager(dbContext);
            _fiscalYearManager = new FiscalYearManager(dbContext);
            _taxInstallmentInfoManager = new TaxInstallmentInfoManager(dbContext);
            _loanInstallmentInfoManager = new LoanInstallmentInfoManager(dbContext);
            _userStationPermissionManager = new UserStationPermissionManager(dbContext);
            _pRlApplicantInfoManager = new PRlApplicantInfoManager(dbContext);
            processSalaryManager = new ProcessSalaryManager(dbContext);
            salaryHistoryManager = new SalaryHistoryManager(dbContext);


        }


        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult SelfSalaryReport()
        {
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelfSalaryReport(int year, int month)
        {
            var getData = processSalaryManager.GetByMonthYear(month, year);
            if (getData == null)
            {
                TempData["Error"] = "Salary not process yet. Keep patient or ask concern department";
                return RedirectToAction("SelfSalaryReport");
            }
            else
            {
                var user = _userManager.GetUserId(User);
                var users = _userManager.Users.Where(c => c.Id == user).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
                var source = CalculateSalary(year, month, users, false);
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;
                string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\SalaryRpt.rdlc";
                var parameters = new[]
                {
                new ReportParameter("salaryBill",GetMonthName.GetFullName(month) + "/" + year),
                new ReportParameter("billDate", DateTime.Today.ToString("dd/MM/yyyy"))
                };

                report.DataSources.Add(new ReportDataSource("Main", source));
                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);
            }

        }





        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UserSpecificSalaryReport()
        {
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            //ViewBag.users = _userManager.Users.Where(c => c.IsActive).ToList();

            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.users = new SelectList(users, "Value", "Text");
            ViewBag.Error = TempData["Error"];

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserSpecificSalaryReport(int year, int month, string AppUserId)
        {
            var getData = processSalaryManager.GetByMonthYear(month, year);
            if (getData == null)
            {
                TempData["Error"] = "Salary not process yet. Keep patient or ask concern department";
                return RedirectToAction("UserSpecificSalaryReport");
            }
            else
            {

                var users = _userManager.Users.Where(c => c.Id == AppUserId).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
                var source = CalculateSalary(year, month, users, false);
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;
                string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\SalaryRpt.rdlc";
                var parameters = new[]
                {
                new ReportParameter("salaryBill", GetMonthName.GetFullName(month) + "/" + year),
                new ReportParameter("billDate", DateTime.Today.ToString("dd/MM/yyyy"))
            };

                report.DataSources.Add(new ReportDataSource("Main", source));
                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);
            }
        }




        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult Calculate()
        {
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(int year, int month, int StationId, int? WingId, int? DepartmentId,
            int? SectionId, int? FromGradeId, int? ToGradeId)
        {
            try
            {
                string renderFormat = "PDF";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;

                //var users = _userManager.Users.Where(c => c.StationId == StationId && c.IsActive && (WingId == null || c.WingId == WingId) && (DepartmentId == null || c.DepartmentId == DepartmentId) && (SectionId == null || c.SectionId == SectionId)).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);


                var users = _userManager.Users.Where(c => c.StationId == StationId && c.IsActive && (WingId == null || c.WingId == WingId) && (DepartmentId == null || c.DepartmentId == DepartmentId) && (SectionId == null || c.SectionId == SectionId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
                  (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);



                var source = CalculateSalary(year, month, users, false);
                string msg = "";
                if (source.Rows.Count == 0)
                {
                    msg = "Data is not found. please enter relaible data for expected salary report";

                }
                string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\SalaryRpt.rdlc";

                var parameters = new[]
                {
                    new ReportParameter("msg",msg),
                    new ReportParameter("salaryBill",GetMonthName.GetFullName(month) + "/" + year),
                    new ReportParameter("billDate", DateTime.Today.ToString("dd/MM/yyyy"))
                };


                report.DataSources.Add(new ReportDataSource("Main", source));
                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);
            }
            catch (Exception ex)
            {
                return BadRequest("Data Is Not Found !!");
            }



        }
        //[MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public DataTable CalculateSalary(int year, int month, IIncludableQueryable<AppUser, Designation> userList, bool isBanglaRequired)
        {
            var orderedUser = userList.OrderBy(c => c.Designation.DisgOrder).ToList();
            DataTable mainTable = new DataTable();

            var salaryHistory = salaryHistoryManager.GetListByMonthYear(year, month, orderedUser, isBanglaRequired);

            if (salaryHistory.Any())
            {
                string json = JsonConvert.SerializeObject(salaryHistory);
                mainTable = JsonConvert.DeserializeObject<DataTable>(json);
                return mainTable;
            }
            mainTable.Columns.Add("UserId", typeof(string));
            mainTable.Columns.Add("EmployeeCode", typeof(string));
            mainTable.Columns.Add("EmployeeName", typeof(string));
            mainTable.Columns.Add("Scale", typeof(string));
            mainTable.Columns.Add("Department", typeof(string));
            mainTable.Columns.Add("Designation", typeof(string));
            mainTable.Columns.Add("BasicAllowance", typeof(decimal));
            mainTable.Columns.Add("CurrentBasic", typeof(decimal));
            mainTable.Columns.Add("BankAccountNo", typeof(string));
            //
            mainTable.Columns.Add("MedicalAllowance", typeof(decimal));
            mainTable.Columns.Add("HouseRentAllowance", typeof(decimal));
            mainTable.Columns.Add("DearnessAllowance", typeof(decimal));
            mainTable.Columns.Add("MobileCellphoneAllowance", typeof(decimal));
            mainTable.Columns.Add("TelephoneAllowance", typeof(decimal));
            mainTable.Columns.Add("ChargeAllowance", typeof(decimal));
            mainTable.Columns.Add("EducationAllowance", typeof(decimal));
            mainTable.Columns.Add("HonoraryAllowance", typeof(decimal));
            mainTable.Columns.Add("TravelingAllowance", typeof(decimal));
            mainTable.Columns.Add("AdvanceAllowance", typeof(decimal));
            mainTable.Columns.Add("TransportAllowance", typeof(decimal));
            mainTable.Columns.Add("PrantikSubidha", typeof(decimal));
            mainTable.Columns.Add("BonusRefund", typeof(decimal));
            mainTable.Columns.Add("OthersAllowance", typeof(decimal));
            mainTable.Columns.Add("TiffinAllowance", typeof(decimal));
            mainTable.Columns.Add("WashAllowance", typeof(decimal));
            mainTable.Columns.Add("ArrearsBasic", typeof(decimal));
            mainTable.Columns.Add("FestivalAllowance", typeof(decimal));
            mainTable.Columns.Add("SpecialBenefit", typeof(decimal));
            //
            mainTable.Columns.Add("CPFRegular", typeof(decimal));
            mainTable.Columns.Add("CPFAdditional", typeof(decimal));
            mainTable.Columns.Add("CPFArrears", typeof(decimal));
            mainTable.Columns.Add("HouseRentDeduction", typeof(decimal));
            mainTable.Columns.Add("ElectricBill", typeof(decimal));
            mainTable.Columns.Add("GasBill", typeof(decimal));
            mainTable.Columns.Add("WaterBill", typeof(decimal));
            mainTable.Columns.Add("IncomeTaxAmount", typeof(decimal));
            mainTable.Columns.Add("IncomeTaxInstallment", typeof(string));
            //
            mainTable.Columns.Add("CPFFirstCapital", typeof(decimal));
            mainTable.Columns.Add("CPFFirstInstallment", typeof(string));
            mainTable.Columns.Add("CPFSecondCapital", typeof(decimal));
            mainTable.Columns.Add("CPFSecondInstallment", typeof(string));

            mainTable.Columns.Add("MotorCycleFirstCapital", typeof(decimal));
            mainTable.Columns.Add("MotorCycleFirstCapitalInstallment", typeof(string));
            mainTable.Columns.Add("MotorCycleFirstInterest", typeof(decimal));
            mainTable.Columns.Add("MotorCycleFirstInterestInstallment", typeof(string));

            mainTable.Columns.Add("MotorCycleSecondCapital", typeof(decimal));
            mainTable.Columns.Add("MotorCycleSecondCapitalInstallment", typeof(string));
            mainTable.Columns.Add("MotorCycleSecondInterest", typeof(decimal));
            mainTable.Columns.Add("MotorCycleSecondInterestInstallment", typeof(string));
            //
            mainTable.Columns.Add("CarFirstCapital", typeof(decimal));
            mainTable.Columns.Add("CarFirstCapitalInstallment", typeof(string));
            mainTable.Columns.Add("CarFirstInterest", typeof(decimal));
            mainTable.Columns.Add("CarFirstInterestInstallment", typeof(string));
            //
            mainTable.Columns.Add("CarSecondCapital", typeof(decimal));
            mainTable.Columns.Add("CarSecondCapitalInstallment", typeof(string));
            mainTable.Columns.Add("CarSecondInterest", typeof(decimal));
            mainTable.Columns.Add("CarSecondInterestInstallment", typeof(string));

            mainTable.Columns.Add("HouseBuildingFirstCapital", typeof(decimal));
            mainTable.Columns.Add("HouseBuildingFirstCapitalInstallment", typeof(string));
            mainTable.Columns.Add("HouseBuildingFirstInterest", typeof(decimal));
            mainTable.Columns.Add("HouseBuildingFirstInterestInstallment", typeof(string));

            //
            mainTable.Columns.Add("HouseBuildingSecondCapital", typeof(decimal));
            mainTable.Columns.Add("HouseBuildingSecondCapitalInstallment", typeof(string));
            mainTable.Columns.Add("HouseBuildingSecondInterest", typeof(decimal));
            mainTable.Columns.Add("HouseBuildingSecondInterestInstallment", typeof(string));

            //
            mainTable.Columns.Add("HouseRepairingFirstCapital", typeof(decimal));
            mainTable.Columns.Add("HouseRepairingFirstCapitalInstallment", typeof(string));
            mainTable.Columns.Add("HouseRepairingFirstInterest", typeof(decimal));
            mainTable.Columns.Add("HouseRepairingFirstInterestInstallment", typeof(string));

            //
            mainTable.Columns.Add("HouseRepairingSecondCapital", typeof(decimal));
            mainTable.Columns.Add("HouseRepairingSecondCapitalInstallment", typeof(string));
            mainTable.Columns.Add("HouseRepairingSecondInterest", typeof(decimal));
            mainTable.Columns.Add("HouseRepairingSecondInterestInstallment", typeof(string));
            //
            mainTable.Columns.Add("OthersAdvanceCapital", typeof(decimal));
            mainTable.Columns.Add("OthersAdvanceCapitalInstallment", typeof(string));
            mainTable.Columns.Add("OthersAdvanceInterest", typeof(decimal));
            mainTable.Columns.Add("OthersAdvanceInterestInstallment", typeof(string));

            //
            mainTable.Columns.Add("OthersCapital", typeof(decimal));
            mainTable.Columns.Add("OthersCapitalInstallment", typeof(string));
            mainTable.Columns.Add("OthersInterest", typeof(decimal));
            mainTable.Columns.Add("OthersInterestInstallment", typeof(string));

            //
            mainTable.Columns.Add("HouseRepairingThirdCapital", typeof(decimal));
            mainTable.Columns.Add("HouseRepairingThirdCapitalInstallment", typeof(string));
            mainTable.Columns.Add("HouseRepairingThirdInterest", typeof(decimal));
            mainTable.Columns.Add("HouseRepairingThirdInterestInstallment", typeof(string));




            mainTable.Columns.Add("BasicDeduction", typeof(decimal));
            mainTable.Columns.Add("Transport", typeof(decimal));
            mainTable.Columns.Add("Garage", typeof(decimal));
            mainTable.Columns.Add("GroupInsurance", typeof(decimal));
            mainTable.Columns.Add("WelfareFund", typeof(decimal));
            mainTable.Columns.Add("Rehabilitation", typeof(decimal));

            //
            mainTable.Columns.Add("GrossSalary", typeof(decimal));
            mainTable.Columns.Add("TotalDeduction", typeof(decimal));
            mainTable.Columns.Add("NetSalary", typeof(decimal));
            mainTable.Columns.Add("NetInWord", typeof(string));
            mainTable.Columns.Add("Remarks", typeof(string));
            if (isBanglaRequired)
            {
                mainTable.Columns.Add("NetSalaryBangla", typeof(string));
                mainTable.Columns.Add("EmployeeNameBangla", typeof(string));
                mainTable.Columns.Add("DesignationBangla", typeof(string));
            }

           










            var fiscalYear = GetFiscalYear(month, year);
            var firstDayOfMonth = new DateTime(year, month, 1);
            int totalDaysInMonth = DateTime.DaysInMonth(year, month);
            DateTime lastDayOfMonth = new DateTime(year, month, totalDaysInMonth);


            //List<SalaryVM> list = new List<SalaryVM>();
            var payScales = _payScaleManager.GetList();
            var gradeWisePayScales = _gradeWisePayScaleManager.GetList();
            var houseRent = _houseRentRuleManager.GetList();
            var childrenInfos = _childrenInfoManager.GetList();
            var deductions = _deductionManager.GetAll();
            var fixedDeductions = _fixedDeductionManager.GetAll();
            var userWiseDeductions = _userDeductionManager.GetAll();
            var userHouseRents = _userHouseRentManager.GetAll();
            var userTaxes = _userTaxManager.GetTaxesThisFiscalYear(fiscalYear?.Id ?? 0);
            var loanHeads = _loanHeadManager.GetAll();
            var userLoans = _userWiseLoanManager.GetActiveLoans();
            var gradeWiseFixedDeduction = _gradeWiseFixedDeductionManager.GetAll();
            var userSpecificAllowances = _userSpecificAllowanceManager.GetAllByMonthYear(month, year);
            var gradeSteps = _gradeStepManager.GetAll();
            var percent = _cpfPercentManager.GetByName("CPF Regular")?.Percent ?? 10;
            var transferHistory = _transferHistoryManager.getNewTransferHistories(year, month);
            var suspensionList = _suspensionHistoryManager.GetCurrentSuspension(firstDayOfMonth, lastDayOfMonth);
            var taxInstallmentInfo = _taxInstallmentInfoManager.GetByMonthYear(month, year);
            var loanInstallments = _loanInstallmentInfoManager.GetCurrentMonthLoan(year, month);


           
            foreach (var user in orderedUser)
            {
                string remarks = "";
                var endOfJob = user.BirthDate.AddYears(60);
                if (endOfJob <= firstDayOfMonth)
                {
                    continue;
                }


                if (user.IsLien)
                {
                    continue;
                }
                if (user.UserName == "01789377312")
                {
                    continue;
                }
                bool isFraction = false;
                bool isNewlyTransfered = false;
                decimal sumOfAllowance = 0;
                decimal sumOfDeduction = 0;
                // decimal netPayment = 0;
                DataRow mainRow = mainTable.NewRow();
                decimal payableBasic = user.CurrentBasic ?? 0;
                decimal houseRentAllowance = 0;
                decimal arrearBasic = 0;
                decimal houseRentAllownceFromArrearBasic = 0;


                var getHouseRentPercent = houseRent.FirstOrDefault(c =>
                                c.BasicFrom <= user.CurrentBasic && c.BasicTo >= user.CurrentBasic &&
                                c.StationType == user.Station.StationType);



                SalaryVM salary = new SalaryVM();
                var prlDate = user.BirthDate.AddYears(user.PlrAge);

                salary.EmployeeName = user.FullName;
                salary.EmployeeCode = user.EmployeeCode;
                int workingDays = totalDaysInMonth;
                if (user.JoiningDate != user.CurrentStationJoiningDate && user.CurrentStationJoiningDate.Value.Year == year &&
                    user.CurrentStationJoiningDate.Value.Month == month)
                {
                    isNewlyTransfered = true;
                }
                if (user.JoiningDate.Year == year && user.JoiningDate.Month == month)
                {

                    if (user.ResignationDate?.Year == year && user.ResignationDate?.Month == month)
                    {
                        workingDays = (user.ResignationDate?.Day ?? 0 - user.JoiningDate.Day) + 1;
                    }
                    else
                    {
                        workingDays = (totalDaysInMonth - user.JoiningDate.Day) + 1;
                    }
                }
                else if (user.ResignationDate?.Year == year && user.ResignationDate?.Month == month)
                {
                    workingDays = user.ResignationDate.Value.Day;
                }
                else if (endOfJob <= lastDayOfMonth)
                {
                    workingDays = endOfJob.Day - 1;
                }

                if (workingDays < totalDaysInMonth)
                {
                    payableBasic = ((user.CurrentBasic / totalDaysInMonth) * workingDays) ?? 0;
                    isFraction = true;
                }

                var isSuspended = suspensionList.Where(c => c.AppUserId == user.Id);
                int totalSuspendedDays = 0;

                foreach (var item in isSuspended)
                {
                    if (item.StartDate < firstDayOfMonth && item.EndDate > lastDayOfMonth)
                    {
                        totalSuspendedDays = totalDaysInMonth;
                        //break;
                    }

                    if (item.StartDate < firstDayOfMonth && item.EndDate <= lastDayOfMonth)
                    {
                        totalSuspendedDays += item.EndDate.Value.Day;
                        //continue;
                    }

                    if (item.StartDate >= firstDayOfMonth && item.EndDate <= lastDayOfMonth)
                    {
                        totalSuspendedDays += (item.EndDate.Value.Day - item.StartDate.Day + 1);
                        //continue;
                    }

                    if (item.StartDate <= lastDayOfMonth && item.StartDate >= firstDayOfMonth && item.EndDate > lastDayOfMonth)
                    {
                        totalSuspendedDays += (lastDayOfMonth.Day - item.StartDate.Day + 1);
                        //continue;
                    }
                }

                if (totalSuspendedDays > totalDaysInMonth)
                {
                    totalSuspendedDays = totalDaysInMonth;
                }



                if (totalSuspendedDays > 0)
                {

                    user.IsSuspended = true;
                    int nonSuspendedDays = workingDays - totalSuspendedDays;

                    payableBasic = (payableBasic * nonSuspendedDays) / workingDays + (payableBasic * totalSuspendedDays) / (workingDays * 2);
                }

                foreach (var payScale in payScales)
                {
                    PayScaleVm p = new PayScaleVm();
                    p.Name = payScale.Name;
                    p.EmployeeId = user.Id;
                    if (payScale.IsAvailable)
                    {
                        if (payScale.Type == PayScaleType.Basic)
                        {
                            p.Amount = payableBasic;
                            //mainRow["BasicAllowance"] = user.CurrentBasic ?? 0;
                        }
                        else if (payScale.Type == PayScaleType.HouseRent)
                        {


                            decimal? houseRentAmount = 0;

                            if (isNewlyTransfered)
                            {
                                var previousStation = transferHistory.Where(c => c.AppUserId == user.Id).OrderByDescending(c => c.TransferDate)
                                    .FirstOrDefault();
                                if (previousStation != null)
                                {
                                    var getPreviousHouseRentPercent = houseRent.FirstOrDefault(c =>
                                        c.BasicFrom <= user.CurrentBasic && c.BasicTo >= user.CurrentBasic &&
                                        c.StationType == previousStation.FromStation.StationType);


                                    int previousStationWorkingDays = previousStation.TransferDate.Day;
                                    int currentStationWorkingDays = totalDaysInMonth - previousStationWorkingDays;

                                    if (user.IsSuspended)
                                    {
                                        houseRentAmount = (((user.CurrentBasic * getHouseRentPercent.Percentage) / 100) * currentStationWorkingDays) / totalDaysInMonth + (((user.CurrentBasic * getPreviousHouseRentPercent.Percentage) / 100) * previousStationWorkingDays) / totalDaysInMonth;
                                    }
                                    else
                                    {

                                        houseRentAmount = (((payableBasic * getHouseRentPercent.Percentage) / 100) * currentStationWorkingDays) / totalDaysInMonth + (((payableBasic * getPreviousHouseRentPercent.Percentage) / 100) * previousStationWorkingDays) / totalDaysInMonth;
                                    }
                                }

                            }
                            else
                            {
                                if (getHouseRentPercent != null)
                                {
                                    if (user.IsSuspended)
                                    {
                                        houseRentAmount = (user.CurrentBasic * getHouseRentPercent.Percentage) / 100;
                                        if (isFraction)
                                        {
                                            houseRentAmount = (houseRentAmount * workingDays) / totalDaysInMonth;
                                        }
                                    }
                                    else
                                    {
                                        houseRentAmount = (payableBasic * getHouseRentPercent.Percentage) / 100;
                                    }

                                    if (houseRentAmount < getHouseRentPercent.MinimumAmount)
                                    {
                                        if (!isFraction)
                                        {
                                            houseRentAmount = getHouseRentPercent.MinimumAmount;
                                        }

                                    }

                                }

                            }







                            p.Amount = houseRentAmount ?? 0;
                            houseRentAllowance = p.Amount;
                            //mainRow["HouseRentAllowance"] = houseRentAllowance;
                        }
                        else if (payScale.Type == PayScaleType.Education)
                        {
                            var lastDob = firstDayOfMonth.AddYears(-23);
                            var getChildren = childrenInfos.Count(c => c.IsApprove && c.DateOfBirth >= lastDob && c.AppUserId == user.Id);
                            if (getChildren > 2)
                            {
                                getChildren = 2;
                            }
                            p.Amount = 500 * getChildren;
                            //mainRow["EducationAllowance"] = 500 * getChildren;

                        }
                        else if (payScale.Type == PayScaleType.Charge)
                        {
                            if (user.IsAllowedForChargeAllowance)
                            {
                                var am = (user.CurrentBasic * 10) / 100;
                                if (am > 1500)
                                {
                                    am = 1500;
                                }

                                p.Amount = am ?? 0;
                            }



                        }
                        else if (payScale.Type == PayScaleType.Transport)
                        {
                            int cStationWoringDay = totalDaysInMonth;
                            if (!(prlDate < lastDayOfMonth))
                            {

                                //Will be remove here and goes to top

                                if (isNewlyTransfered)
                                {
                                    var previousStation = transferHistory.Where(c => c.AppUserId == user.Id).OrderByDescending(c => c.TransferDate)
                                        .FirstOrDefault();
                                    if (previousStation != null)
                                    {
                                        var getPreviousHouseRentPercent = houseRent.FirstOrDefault(c =>
                                            c.BasicFrom <= user.CurrentBasic && c.BasicTo >= user.CurrentBasic &&
                                            c.StationType == previousStation.FromStation.StationType);


                                        int previousStationWorkingDays = previousStation.TransferDate.Day;
                                        cStationWoringDay = totalDaysInMonth - previousStationWorkingDays;


                                    }

                                }

                                //if (user.GradeId > 10 && !user.IsLiveNear3Km && (user.Station.StationType == StationType.DhakaCity || user.Station.StationType == StationType.OthersCity))
                                if (user.GradeId > 10 && (user.Station.StationType == StationType.DhakaCity || user.Station.StationType == StationType.OthersCity))
                                {
                                    decimal tAmount = ((decimal)(300 * cStationWoringDay) / totalDaysInMonth);
                                    p.Amount = tAmount;
                                }






                            }


                        }
                        else if (payScale.Type == PayScaleType.Wash)
                        {
                            var fiveYearsFromJoining = user.JoiningDate.AddYears(5);
                            if ((user.GradeId > 16 || user.Designation.Name == "গাড়ী চালক" || user.Designation.NameBangla == "গাড়ী চালক") &&
                                lastDayOfMonth >= fiveYearsFromJoining)
                            {
                                p.Amount = 100;
                            }
                        }
                        else
                        {
                            if (payScale.IsUserSpecific)
                            {
                                var uSpecific = userSpecificAllowances.FirstOrDefault(c => c.AppUserId == user.Id && c.PayScaleId == payScale.Id);
                                p.Amount = uSpecific?.Amount ?? 0;
                                remarks += uSpecific?.Remarks + " ";
                                if (payScale.Name == "ArrearsBasic")
                                {
                                    houseRentAllownceFromArrearBasic = (p.Amount * getHouseRentPercent.Percentage) / 100;
                                    arrearBasic = p.Amount;
                                    houseRentAllowance += houseRentAllownceFromArrearBasic;
                                    sumOfAllowance += houseRentAllownceFromArrearBasic;
                                }
                            }
                            else
                            {

                                var gradeWisePayScale = gradeWisePayScales.FirstOrDefault(c =>
                                    c.PayScaleId == payScale.Id && c.GradeId == user.GradeId);
                                if (gradeWisePayScale != null)
                                {
                                    if (gradeWisePayScale.IsFixed)
                                    {
                                        if (payScale.Type == PayScaleType.Tiffin && prlDate < lastDayOfMonth)
                                        {
                                            p.Amount = 0;
                                        }
                                        else
                                        {
                                            p.Amount = gradeWisePayScale.FixedAmount ?? 0;
                                        }


                                    }
                                    else
                                    {
                                        decimal? preAmount = 0;
                                        if (user.IsSuspended)
                                        {
                                            preAmount = (user.CurrentBasic * gradeWisePayScale.Percentage) / 100;
                                        }
                                        else
                                        {
                                            preAmount = (payableBasic * gradeWisePayScale.Percentage) / 100;
                                        }

                                        if (preAmount < gradeWisePayScale.MinimumAmount)
                                        {
                                            if (!isFraction)
                                            {
                                                preAmount = gradeWisePayScale.MinimumAmount;
                                            }

                                        }
                                        else if (preAmount > gradeWisePayScale.MaximumAmount)
                                        {
                                            preAmount = gradeWisePayScale.MaximumAmount;
                                        }




                                        p.Amount = preAmount ?? 0;
                                    }
                                }
                                else
                                {
                                    p.Amount = 0;
                                }
                            }


                        }
                    }
                    else
                    {
                        p.Amount = 0;
                    }
                    if (isFraction && !(payScale.Type == PayScaleType.HouseRent || payScale.Type == PayScaleType.Basic))
                    {
                        p.Amount = (p.Amount * workingDays / totalDaysInMonth);
                    }


                    sumOfAllowance += p.Amount;
                    mainRow[p.Name] = p.Amount;

                    //salaryPart.Add(p);
                }
                mainRow["HouseRentAllowance"] = houseRentAllowance;
                //salary.SalaryPart = salaryPart;
                //Deduction Part
                //CPF
                var cpfStartDate = user.CPFStartDate;
                if (cpfStartDate == null || cpfStartDate == DateTime.MinValue)
                {
                    //For overcome any null reference exception
                    cpfStartDate = user.JoiningDate.AddYears(100);
                }

                if (cpfStartDate > lastDayOfMonth || prlDate < lastDayOfMonth)
                {
                    if (prlDate.Year == year && prlDate.Month == month)
                    {
                        int cpfApplicableDays = prlDate.Day - 1;

                        salary.CPFRegular = (decimal)((((user.CurrentBasic / totalDaysInMonth) * cpfApplicableDays) * percent) / 100);
                        salary.ArrearsBasic = 0;
                    }
                    else
                    {
                        salary.CPFRegular = 0;
                        salary.ArrearsBasic = 0;
                    }





                }
                else
                {
                    //var PRLUser = _pRlApplicantInfoManager.GetListByUser(user.Id);


                    var cpfDays = (int)(lastDayOfMonth - cpfStartDate).Value.TotalDays + 1;
                    if (cpfDays >= totalDaysInMonth)
                    {
                        salary.CPFRegular = (payableBasic * percent) / 100;
                        salary.CPFArrears = (arrearBasic * percent) / 100;
                    }
                    else
                    {
                        var payableCpf = ((payableBasic / totalDaysInMonth) * cpfDays * percent) / 100;
                        salary.CPFRegular = payableCpf;
                        salary.CPFArrears = (arrearBasic * percent) / 100;
                    }
                }

                mainRow["CPFRegular"] = salary.CPFRegular;
                mainRow["CPFArrears"] = salary.CPFArrears;
                mainRow["CPFAdditional"] = 0;
                salary.CPFAdditional = 0;
                mainRow["IncomeTaxAmount"] = 0;
                mainRow["IncomeTaxInstallment"] = "0/0";

                sumOfDeduction += salary.CPFAdditional + salary.CPFArrears + salary.CPFRegular;
                //Utility

                var utilityList = deductions.Where(c => c.DeductionType == DeductionType.Utility);
                foreach (var utility in utilityList)
                {
                    UtilityVm objUtility = new UtilityVm();
                    objUtility.Name = utility.Name;
                    //All resident including dormitory
                    if (user.ResidentialStatusId != 2)
                    {
                        if (utility.UtilityType == UtilityType.HouseRent)
                        {
                            var checkHouseRent = userHouseRents.FirstOrDefault(c => c.AppUserId == user.Id);
                            decimal dHouseRent = 0;
                            if (checkHouseRent != null)
                            {
                                if (isFraction)
                                {
                                    if (user.ResignationDate != null)
                                    {
                                        if (user.ResignationDate.Value.Day > 15)
                                        {
                                            dHouseRent = checkHouseRent.Amount;
                                        }
                                        else
                                        {
                                            dHouseRent = checkHouseRent.Amount / 2;
                                        }
                                    }
                                    else if (user.CurrentStationJoiningDate.Value.Day > 15)
                                    {
                                        dHouseRent = checkHouseRent.Amount / 2;
                                    }
                                    else
                                    {
                                        dHouseRent = checkHouseRent.Amount;
                                    }
                                }
                                else
                                {
                                    dHouseRent = checkHouseRent.Amount;
                                }
                            }
                            else
                            {
                                dHouseRent = houseRentAllowance;
                            }

                            objUtility.Amount = dHouseRent;

                        }
                        else if (utility.IsFixed && user.ResidentialStatusId != 3)
                        {
                            decimal initialDAmount = fixedDeductions.FirstOrDefault(c => c.DeductionId == utility.Id)?.Amount ?? 0;
                            if (isFraction)
                            {
                                if (user.ResignationDate != null)
                                {
                                    if (user.ResignationDate.Value.Day > 15)
                                    {
                                        objUtility.Amount = initialDAmount;
                                    }
                                    else
                                    {
                                        objUtility.Amount = initialDAmount / 2;
                                    }
                                }
                                else if (user.CurrentStationJoiningDate.Value.Day > 15)
                                {
                                    objUtility.Amount = initialDAmount / 2;
                                }
                                else
                                {
                                    objUtility.Amount = initialDAmount;
                                }
                            }
                            else
                            {
                                objUtility.Amount = initialDAmount;
                            }
                        }
                        else
                        {
                            var data = userWiseDeductions.FirstOrDefault(c => (c.AppUserId == user.Id && c.DeductionId == utility.Id && (c.IsSameEveryMonth || (c.Year == year && c.Month == month))));
                            objUtility.Amount = data?.Amount ?? 0;
                        }
                    }
                    //Non resident
                    else
                    {
                        objUtility.Amount = 0;
                    }

                    if (utility.UtilityType != UtilityType.Tax)
                    {
                        sumOfDeduction += objUtility.Amount;
                        mainRow[utility.Name] = objUtility.Amount;
                    }

                    //utilityPart.Add(objUtility);
                }

                //salary.UtilityPart = utilityPart;
                //Tax

                var uTax = userTaxes.FirstOrDefault(c => c.AppUserId == user.Id);
                if (uTax != null)
                {
                    var userTaxData = taxInstallmentInfo.FirstOrDefault(c =>
                        c.Month == month && c.Year == year && c.AppUserId == user.Id);
                    //salary.Tax = uTax;
                    if (userTaxData != null)
                    {
                        mainRow["IncomeTaxAmount"] = uTax?.MonthlyDeduction ?? 0;
                        mainRow["IncomeTaxInstallment"] = userTaxData.InstallmentNo + "/" + uTax.TotalInstallment;
                        sumOfDeduction += uTax?.MonthlyDeduction ?? 0;
                    }


                }
                else
                {
                    mainRow["IncomeTaxAmount"] = 0;
                    mainRow["IncomeTaxInstallment"] = "0/0";
                }



                //Loans
                foreach (var l in loanHeads)
                {
                    LoanVm loan = new LoanVm();
                    loan.Name = l.Name;
                    var cUserLoan = userLoans.FirstOrDefault(c => c.AppUserId == user.Id && c.LoanHeadId == l.Id);
                    if (cUserLoan != null)
                    {
                        var getInstallmentByUser = loanInstallments.FirstOrDefault(c =>c.UserWiseLoanId==cUserLoan.Id);

                        if (getInstallmentByUser != null)
                        {
                            if (getInstallmentByUser.IsCapital)
                            {
                                loan.CapitalAmount = cUserLoan.CapitalDeductionAmount;
                                loan.CurrentCapitalInstallment = getInstallmentByUser.CapitalInstallmentNo;
                                loan.TotalCapitalInstallment = cUserLoan.NoOfInstallment;
                            }
                            else
                            {
                                loan.CurrentInterestInstallment = getInstallmentByUser.InterestInstallmentNo;
                                loan.InterestAmount = cUserLoan.InterestDeductionAmount ?? 0;
                                loan.TotalInterestInstallment = cUserLoan.NoOfInstallmentForInterest ?? 0;

                            }



                        }
                    }
                    switch (l.Name)
                    {
                        case "CPFFirstLoan":
                            mainRow["CPFFirstCapital"] = loan.CapitalAmount;
                            mainRow["CPFFirstInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            break;
                        case "CPFSecondLoan":
                            mainRow["CPFSecondCapital"] = loan.CapitalAmount;
                            mainRow["CPFSecondInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment; ;
                            break;
                        case "MotorCycleFirst":
                            mainRow["MotorCycleFirstCapital"] = loan.CapitalAmount;
                            mainRow["MotorCycleFirstCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["MotorCycleFirstInterest"] = loan.InterestAmount;
                            mainRow["MotorCycleFirstInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "MotorCycleSecond":
                            mainRow["MotorCycleSecondCapital"] = loan.CapitalAmount;
                            mainRow["MotorCycleSecondCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["MotorCycleSecondInterest"] = loan.InterestAmount;
                            mainRow["MotorCycleSecondInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "CarFirst":
                            mainRow["CarFirstCapital"] = loan.CapitalAmount;
                            mainRow["CarFirstCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["CarFirstInterest"] = loan.InterestAmount;
                            mainRow["CarFirstInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "CarSecond":
                            mainRow["CarSecondCapital"] = loan.CapitalAmount;
                            mainRow["CarSecondCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["CarSecondInterest"] = loan.InterestAmount;
                            mainRow["CarSecondInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "HouseBuildingFirst":
                            mainRow["HouseBuildingFirstCapital"] = loan.CapitalAmount;
                            mainRow["HouseBuildingFirstCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["HouseBuildingFirstInterest"] = loan.InterestAmount;
                            mainRow["HouseBuildingFirstInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "HouseBuildingSecond":
                            mainRow["HouseBuildingSecondCapital"] = loan.CapitalAmount;
                            mainRow["HouseBuildingSecondCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["HouseBuildingSecondInterest"] = loan.InterestAmount;
                            mainRow["HouseBuildingSecondInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "HouseRepairingFirst":
                            mainRow["HouseRepairingFirstCapital"] = loan.CapitalAmount;
                            mainRow["HouseRepairingFirstCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["HouseRepairingFirstInterest"] = loan.InterestAmount;
                            mainRow["HouseRepairingFirstInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "HouseRepairingSecond":
                            mainRow["HouseRepairingSecondCapital"] = loan.CapitalAmount;
                            mainRow["HouseRepairingSecondCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["HouseRepairingSecondInterest"] = loan.InterestAmount;
                            mainRow["HouseRepairingSecondInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "HouseRepairingThird":
                            mainRow["HouseRepairingThirdCapital"] = loan.CapitalAmount;
                            mainRow["HouseRepairingThirdCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["HouseRepairingThirdInterest"] = loan.InterestAmount;
                            mainRow["HouseRepairingThirdInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "OthersAdvance":
                            mainRow["OthersAdvanceCapital"] = loan.CapitalAmount;
                            mainRow["OthersAdvanceCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["OthersAdvanceInterest"] = loan.InterestAmount;
                            mainRow["OthersAdvanceInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                        case "Others":
                            mainRow["OthersCapital"] = loan.CapitalAmount;
                            mainRow["OthersCapitalInstallment"] = loan.CurrentCapitalInstallment + "/" + loan.TotalCapitalInstallment;
                            mainRow["OthersInterest"] = loan.InterestAmount;
                            mainRow["OthersInterestInstallment"] = loan.CurrentInterestInstallment + "/" + loan.TotalInterestInstallment;
                            break;
                    }

                    sumOfDeduction += (loan.InterestAmount + loan.CapitalAmount);
                    //  mainRow[l.Name] = loan.Amount;
                }
                // salary.LoanPart = loanPart;

                //Others
                var othersList = deductions.Where(c => c.DeductionType == DeductionType.Others);

                foreach (var o in othersList)
                {
                    OthersVm others = new OthersVm();
                    others.Name = o.Name;

                    var data = userWiseDeductions.FirstOrDefault(c => (c.AppUserId == user.Id && c.DeductionId == o.Id && (c.IsSameEveryMonth || (c.Year == year && c.Month == month))));

                    if (data != null)
                    {
                        others.Amount = data?.Amount ?? 0;
                        remarks += data?.Remarks + " ";
                    }
                    else
                    {
                        var gfd = gradeWiseFixedDeduction.FirstOrDefault(c =>
                            c.DeductionId == o.Id && c.FromGradeId <= user.GradeId && c.ToGradeId >= user.GradeId);

                        if (gfd != null)
                        {
                            others.Amount = gfd?.Amount ?? 0;
                        }
                        else
                        {
                            var fixedDeduction = fixedDeductions.FirstOrDefault(c => c.DeductionId == o.Id);
                            others.Amount = fixedDeduction?.Amount ?? 0;
                        }
                    }


                    //othersPart.Add(others);
                    mainRow[o.Name] = others.Amount;
                    sumOfDeduction += others.Amount;
                }

                //  salary.OthersPart = othersPart;


                //Complete Calculation
                //list.Add(salary);

                var gradeScaleFirst = gradeSteps.Where(c => c.GradeId == user.CurrentGradeId).OrderBy(c => c.Amount).FirstOrDefault().Amount;
                var gradeScaleLast = gradeSteps.Where(c => c.GradeId == user.CurrentGradeId).OrderByDescending(c => c.Amount).FirstOrDefault().Amount;


                mainRow["UserId"] = user.Id;
                mainRow["EmployeeCode"] = user.EmployeeCode;
                mainRow["BankAccountNo"] = user.BankAccountNoBangla;
                mainRow["EmployeeName"] = user.FullName;
                mainRow["Scale"] = gradeScaleFirst + "-" + gradeScaleLast;
                mainRow["Department"] = user.Department.Name;
                mainRow["CurrentBasic"] = user.CurrentBasic;
                mainRow["Designation"] = user.Designation.Name;

                mainRow["GrossSalary"] = sumOfAllowance;
                mainRow["TotalDeduction"] = sumOfDeduction;
                var netSalary = Math.Round((sumOfAllowance - sumOfDeduction), 2);
                mainRow["NetSalary"] = netSalary;
                mainRow["NetInWord"] = NumberToWordConverter.ConvertToWords(netSalary.ToString());
                mainRow["Remarks"] = remarks.Trim();
                sumOfNetSalary += Convert.ToDecimal(netSalary);

                if (isBanglaRequired)
                {
                    mainRow["EmployeeNameBangla"] = user.FullNameBangla;
                    mainRow["DesignationBangla"] = user.Designation.NameBangla;
                    mainRow["NetSalaryBangla"] = string.Concat(netSalary.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".");
                }

                mainTable.Rows.Add(mainRow);
            }
            return mainTable;
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult BankReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BankReport(int StationId, int? WingId, int? FromGradeId, int? ToGradeId, int year, int month, string checkNo, DateTime salaryDate)
        {
            var users = _userManager.Users.Where(c => c.IsActive && c.StationId == StationId && (WingId == null || c.WingId == WingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
              (ToGradeId == null || c.GradeId <= ToGradeId)).Include(c => c.Station).Include(v => v.Wing).Include(v => v.Department).Include(v => v.Designation);
            var source = CalculateSalary(year, month, users, true);
            var getwing = users.FirstOrDefault(c => c.WingId == WingId);
            string msg = "";
            if (source.Rows.Count == 0)
            {
                msg = "Data is  not found. please Enter relaible data for Expected Bank report";

            }

            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\BankReport.rdlc";

            var day = string.Concat(salaryDate.Day.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var m = string.Concat(salaryDate.Month.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var y = string.Concat(salaryDate.Year.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var parameters = new[]
            {
             new ReportParameter("msg",msg),
             new ReportParameter("wing",getwing?.Wing?.Name.ToString()),
            new ReportParameter("checkNo", string.Concat(checkNo.Select(c => (char)('\u09E6' + c - '0')))),
            new ReportParameter("monthYear", GetMonthName.MonthInBangla(month) + "/" + string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
            new ReportParameter("salaryDate", day + "/" + m + "/" + y),
            new ReportParameter("totalSum", string.Concat(sumOfNetSalary.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
            new ReportParameter("totalSumInWord", NumberToWordConverter.ConvertToWordsBangla(sumOfNetSalary.ToString()))

            };
            report.DataSources.Add(new ReportDataSource("Bank", source));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);
        }

        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult CpfDeductionReport()
        {
            var users = _userManager.Users.Include(c => c.Designation).Where(c => c.IsActive).ToList().Select(s => new
            {
                Text = s.EmployeeCode + "-" + s.FullName + "_" + s.Designation.Name,
                Value = s.Id

            }).ToList();
            ViewBag.Users = new SelectList(users, "Value", "Text");
            ViewBag.Station = _stationManager.GetAll();
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        // *********************CPF Deduction Report ***********************************************
        public IActionResult CpfDeductionReport(int year, int month, string AppuserId)
        {
            try
            {
                var users = _userManager.Users.Where(c => c.Id == AppuserId).Include(v => v.Department).Include(c => c.Station).Include(v => v.Designation);
                var source = CalculateSalary(year, month, users, true);
                var getUser = users.FirstOrDefault();

                var motorCycleFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleFirstCapital)", string.Empty)), 2);
                var motorCycleSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleSecondCapital)", string.Empty)), 2);
                var motorCarFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarFirstCapital)", string.Empty)), 2);
                var motorCarSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarSecondCapital)", string.Empty)), 2);
                var houseBuildingFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingFirstCapital)", string.Empty)), 2);
                var houseBuildingSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingSecondCapital)", string.Empty)), 2);
                var houseRepairingFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingFirstCapital)", string.Empty)), 2);
                var houseRepairingSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingSecondCapital)", string.Empty)), 2);
                var othersAdvanceCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersAdvanceCapital)", string.Empty)), 2);
                var othersCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersCapital)", string.Empty)), 2);
                var incomeTax = Math.Round(Convert.ToDecimal(source.Compute("SUM(IncomeTaxAmount)", string.Empty)), 2);
                var rehabilitation = Math.Round(Convert.ToDecimal(source.Compute("SUM(Rehabilitation)", string.Empty)), 2);
                var houseRent = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRentDeduction)", string.Empty)), 2);
                var electricBill = Math.Round(Convert.ToDecimal(source.Compute("SUM(ElectricBill)", string.Empty)), 2);
                var gasBill = Math.Round(Convert.ToDecimal(source.Compute("SUM(GasBill)", string.Empty)), 2);
                var waterBill = Math.Round(Convert.ToDecimal(source.Compute("SUM(WaterBill)", string.Empty)), 2);

                var transport = Math.Round(Convert.ToDecimal(source.Compute("SUM(Transport)", string.Empty)), 2);
                var garage = Math.Round(Convert.ToDecimal(source.Compute("SUM(Garage)", string.Empty)), 2);


                var motorCycleFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleFirstInterest)", string.Empty)), 2);
                var motorCycleSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleSecondInterest)", string.Empty)), 2);
                var motorCarFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarFirstInterest)", string.Empty)), 2);
                var motorCarSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarSecondInterest)", string.Empty)), 2);
                var houseBuildingFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingFirstInterest)", string.Empty)), 2);
                var houseBuildingSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingSecondInterest)", string.Empty)), 2);
                var houseRepairingFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingFirstInterest)", string.Empty)), 2);
                var houseRepairingSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingSecondInterest)", string.Empty)), 2);
                var othersAdvanceInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersAdvanceInterest)", string.Empty)), 2);
                var othersInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersInterest)", string.Empty)), 2);
                var totDecInterest = motorCycleFirstInterest + motorCycleSecondInterest + motorCarFirstInterest + motorCarSecondInterest + houseBuildingFirstInterest + houseBuildingSecondInterest + houseRepairingFirstInterest + houseRepairingSecondInterest + othersAdvanceInterest + othersInterest;

                var totaldeduction = motorCycleFirstCapital + motorCycleSecondCapital + motorCarFirstCapital + motorCarSecondCapital + houseBuildingFirstCapital + houseBuildingSecondCapital + houseRepairingFirstCapital + houseRepairingSecondCapital + othersAdvanceCapital + othersCapital + incomeTax + rehabilitation + houseRent + electricBill + gasBill + waterBill + transport + garage;
                totaldeduction += totDecInterest;
                var totaldeducInBang = NumberToWordConverter.ConvertToWords(totaldeduction.ToString());


                var cpfRegular = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFRegular)", string.Empty)), 2);
                var cpfAdditional = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFAdditional)", string.Empty)), 2);
                var cPFFirstLoan = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFFirstCapital)", string.Empty)), 2);
                var cPFSecondLoan = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFSecondCapital)", string.Empty)), 2);
                var cPFArrear = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFArrears)", string.Empty)), 2);
                var cpfTotal = cpfRegular + cpfAdditional + cPFFirstLoan + cPFSecondLoan + cPFArrear;
                var cpfTotalBng = NumberToWordConverter.ConvertToWords(cpfTotal.ToString());


                var groupInsurance = Math.Round(Convert.ToDecimal(source.Compute("SUM(GroupInsurance)", string.Empty)), 2);
                var welfareFund = Math.Round(Convert.ToDecimal(source.Compute("SUM(WelfareFund)", string.Empty)), 2);
                var totWelfInsurance = groupInsurance + welfareFund;
                var totWelfInsurBng = NumberToWordConverter.ConvertToWords(totWelfInsurance.ToString());

                string renderFormat = "PDF";
                string extension = "pdf";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;
                string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\CpfDeduction.rdlc";

                var parameters = new[]
                {
                 new ReportParameter("CpfTotal", cpfTotal.ToString()),
                new ReportParameter("CpfTotalBng", cpfTotalBng),

                new ReportParameter("totDeductCapital", totaldeduction.ToString()),
                new ReportParameter("totDeducCapitalBn", totaldeducInBang),

                new ReportParameter("totWelfarInsurance", totWelfInsurance.ToString()),
                new ReportParameter("totWelfareInsuBng", totWelfInsurBng),

                new ReportParameter("empName", getUser.FullName),
                new ReportParameter("empId", getUser.EmployeeCode),
                new ReportParameter("designation", getUser.Designation.Name),
                new ReportParameter("department", getUser.Department.Name),
                new ReportParameter("monthYear", MonthInEnglish(month) + " / " + year),
            };

                report.DataSources.Add(new ReportDataSource("SalaryRpt", source));

                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);

            }
            catch (Exception ex)
            {
                return BadRequest("Data Is Not Found OR May Be Generate Surver Error!!");
            }
        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        //**********************Short Bill Report  |*****************************************
        public IActionResult ShortBillReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShortBillReport(int? FromGradeId, int? ToGradeId, int year, int month, int stationId, int? wingId)
        {
            try
            {
                List<ShortBillVM> shortBills = new List<ShortBillVM>();
                var users = _userManager.Users.Where(c => c.StationId == stationId &&
                                c.IsActive && (wingId == null || c.WingId == wingId) && (FromGradeId == null || c.GradeId >= FromGradeId) &&
                                (ToGradeId == null || c.GradeId <= ToGradeId)).Include(v => v.Department).Include(c => c.Station).Include(c => c.Wing).Include(v => v.Designation);
                var source = CalculateSalary(year, month, users, true);


                // Allowance
                var basicAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(BasicAllowance)", string.Empty)), 2);
                var medicalAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(MedicalAllowance)", string.Empty)), 2);
                var houseRentAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRentAllowance)", string.Empty)), 2);
                var dearnessAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(DearnessAllowance)", string.Empty)), 2);
                var cellphoneAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(MobileCellphoneAllowance)", string.Empty)), 2);
                var telephoneAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(TelephoneAllowance)", string.Empty)), 2);
                var chargeAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(ChargeAllowance)", string.Empty)), 2);
                var educationAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(EducationAllowance)", string.Empty)), 2);
                var honoraryAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(HonoraryAllowance)", string.Empty)), 2);
                var travellingAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(TravelingAllowance)", string.Empty)), 2);
                var advanceAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(AdvanceAllowance)", string.Empty)), 2);
                var trasportAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(TransportAllowance)", string.Empty)), 2);
                var prantikSubidha = Math.Round(Convert.ToDecimal(source.Compute("SUM(PrantikSubidha)", string.Empty)), 2);
                var bonusRefund = Math.Round(Convert.ToDecimal(source.Compute("SUM(BonusRefund)", string.Empty)), 2);
                var othersAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersAllowance)", string.Empty)), 2);
                var tiffinAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(TiffinAllowance)", string.Empty)), 2);
                var washAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(WashAllowance)", string.Empty)), 2);                
                var arrearsBasic = Math.Round(Convert.ToDecimal(source.Compute("SUM(ArrearsBasic)", string.Empty)), 2);
                var festivalAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(FestivalAllowance)", string.Empty)), 2);
                var specialBenefit = Math.Round(Convert.ToDecimal(source.Compute("SUM(SpecialBenefit)", string.Empty)), 2);

                var totalAllowance = basicAllowance + medicalAllowance + houseRentAllowance + dearnessAllowance + cellphoneAllowance + telephoneAllowance + chargeAllowance + educationAllowance + honoraryAllowance + travellingAllowance + advanceAllowance + trasportAllowance + prantikSubidha + bonusRefund + othersAllowance + tiffinAllowance + washAllowance + arrearsBasic + festivalAllowance + specialBenefit;
                //Deduction
                var cpfRegular = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFRegular)", string.Empty)), 2);
                var cpfAdditional = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFAdditional)", string.Empty)), 2);
                var arrearsCPF = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFArrears)", string.Empty)), 2);
                var houseRent = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRentDeduction)", string.Empty)), 2);
                var electricBill = Math.Round(Convert.ToDecimal(source.Compute("SUM(ElectricBill)", string.Empty)), 2);
                var gasBill = Math.Round(Convert.ToDecimal(source.Compute("SUM(GasBill)", string.Empty)), 2);
                var waterBill = Math.Round(Convert.ToDecimal(source.Compute("SUM(WaterBill)", string.Empty)), 2);
                var incomeTax = Math.Round(Convert.ToDecimal(source.Compute("SUM(IncomeTaxAmount)", string.Empty)), 2);
                var cPFFirstLoan = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFFirstCapital)", string.Empty)), 2);
                var cPFSecondLoan = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFSecondCapital)", string.Empty)), 2);

                var motorCycleFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleFirstCapital)", string.Empty)), 2);
                var motorCycleFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleFirstInterest)", string.Empty)), 2);

                var motorCycleSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleSecondCapital)", string.Empty)), 2);
                var motorCycleSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleSecondInterest)", string.Empty)), 2);

                var motorCarFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarFirstCapital)", string.Empty)), 2);
                var motorCarFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarFirstInterest)", string.Empty)), 2);

                var motorCarSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarSecondCapital)", string.Empty)), 2);
                var motorCarSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarSecondInterest)", string.Empty)), 2);

                var houseBuildingFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingFirstCapital)", string.Empty)), 2);
                var houseBuildingFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingFirstInterest)", string.Empty)), 2);

                var houseBuildingSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingSecondCapital)", string.Empty)), 2);
                var houseBuildingSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingSecondInterest)", string.Empty)), 2);

                var houseRepairingFirstCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingFirstCapital)", string.Empty)), 2);
                var houseRepairingFirstInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingFirstInterest)", string.Empty)), 2);

                var houseRepairingSecondCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingSecondCapital)", string.Empty)), 2);
                var houseRepairingSecondInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingSecondInterest)", string.Empty)), 2);

                var othersAdvanceCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersAdvanceCapital)", string.Empty)), 2);
                var othersAdvanceInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersAdvanceInterest)", string.Empty)), 2);

                var othersCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersCapital)", string.Empty)), 2);
                var othersInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersInterest)", string.Empty)), 2);

                var basicDeduction = Math.Round(Convert.ToDecimal(source.Compute("SUM(BasicDeduction)", string.Empty)), 2);
                var transport = Math.Round(Convert.ToDecimal(source.Compute("SUM(Transport)", string.Empty)), 2);
                var garage = Math.Round(Convert.ToDecimal(source.Compute("SUM(Garage)", string.Empty)), 2);
                var groupInsurance = Math.Round(Convert.ToDecimal(source.Compute("SUM(GroupInsurance)", string.Empty)), 2);
                var welfareFund = Math.Round(Convert.ToDecimal(source.Compute("SUM(WelfareFund)", string.Empty)), 2);
                var rehabilitation = Math.Round(Convert.ToDecimal(source.Compute("SUM(Rehabilitation)", string.Empty)), 2);

                var totalDeduction = cpfRegular + cpfAdditional + arrearsCPF + houseRent + electricBill + gasBill + waterBill + incomeTax + cPFFirstLoan + cPFSecondLoan + motorCycleFirstCapital + motorCycleFirstInterest + motorCycleSecondCapital + motorCarSecondInterest + motorCarFirstCapital + motorCarFirstInterest + motorCarSecondCapital + motorCarSecondInterest + houseBuildingFirstCapital + houseBuildingFirstInterest + houseBuildingSecondCapital + houseBuildingSecondInterest + houseRepairingFirstCapital + houseRepairingFirstInterest + houseRepairingSecondCapital + houseRepairingSecondInterest + othersAdvanceCapital + othersAdvanceInterest + othersCapital + othersInterest + basicDeduction + transport + garage + groupInsurance + welfareFund + rehabilitation;

                var totalPayable = totalAllowance - totalDeduction;
                var payableInword = NumberToWordConverter.ConvertToWordsBangla(totalPayable.ToString());

                ShortBillVM vm = new ShortBillVM();
                vm.BasicAllowance = basicAllowance;
                vm.MedicalAllowance = medicalAllowance;
                vm.HouseRentAllowance = houseRentAllowance;
                vm.DearnessAllowance = dearnessAllowance;
                vm.MobileCellphoneAllowance = cellphoneAllowance;
                vm.TelephoneAllowance = telephoneAllowance;
                vm.ChargeAllowance = chargeAllowance;
                vm.EducationAllowance = educationAllowance;
                vm.HonoraryAllowance = honoraryAllowance;
                vm.TransportAllowance = trasportAllowance;
                vm.TravelingAllowance = travellingAllowance;
                vm.AdvanceAllowance = advanceAllowance;
                vm.PrantikSubidha = prantikSubidha;
                vm.BonusRefund = bonusRefund;
                vm.OthersAllowance = othersAllowance;
                vm.TiffinAllowance = tiffinAllowance;
                vm.WashAllowance = washAllowance;                
                vm.ArrearsBasic = arrearsBasic;
                vm.FestivalAllowance = festivalAllowance;
                vm.SpecialBenefit = specialBenefit;
                vm.CPFRegular = cpfRegular;
                vm.CPFAdditional = cpfAdditional;
                vm.CPFArrears = arrearsCPF;
                vm.HouseRentDeduction = houseRent;
                vm.ElectricBill = electricBill;
                vm.GasBill = gasBill;
                vm.WaterBill = waterBill;
                vm.IncomeTaxAmount = incomeTax;
                vm.CPFFirstCapital = cPFFirstLoan;
                vm.CPFSecondCapital = cPFSecondLoan;
                vm.MotorCycleFirstCapital = motorCycleFirstCapital;
                vm.MotorCycleFirstInterest = motorCycleFirstInterest;
                vm.MotorCycleSecondCapital = motorCycleSecondCapital;
                vm.MotorCycleSecondInterest = motorCycleSecondInterest;
                vm.CarFirstCapital = motorCarFirstCapital;
                vm.CarFirstInterest = motorCarFirstInterest;
                vm.CarSecondCapital = motorCarSecondCapital;
                vm.CarSecondInterest = motorCarSecondInterest;
                vm.HouseBuildingFirstCapital = houseBuildingFirstCapital;
                vm.HouseBuildingFirstInterest = houseBuildingFirstInterest;
                vm.HouseBuildingSecondCapital = houseBuildingSecondCapital;
                vm.HouseBuildingSecondInterest = houseBuildingSecondInterest;
                vm.HouseRepairingFirstCapital = houseRepairingFirstCapital;
                vm.HouseRepairingFirstInterest = houseRepairingFirstInterest;
                vm.HouseRepairingSecondCapital = houseRepairingSecondCapital;
                vm.HouseRepairingSecondInterest = houseRepairingSecondInterest;
                vm.OthersAdvanceCapital = othersAdvanceCapital;
                vm.OthersAdvanceInterest = othersAdvanceInterest;
                vm.OthersCapital = othersCapital;
                vm.OthersInterest = othersInterest;
                vm.BasicDeduction = basicDeduction;
                vm.TransportGarage = garage;
                vm.Transport = transport;
                vm.GroupInsurance = groupInsurance;
                vm.WelfareFund = welfareFund;
                vm.Rehabilitation = rehabilitation;
                shortBills.Add(vm);

                string renderFormat = "PDF";
               // string extension = "pdf";
                string mimtype = "application/pdf";
                using var report = new LocalReport();
                report.EnableExternalImages = true;
                string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\ShortBill.rdlc";
                string gradeName = "";
                var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
                if (FromGradeId != null && ToGradeId != null)
                {
                    gradeName = fgrade + "থেকে" + tgrade;
                }
                else
                {
                    gradeName = "সকল";
                }

                var parameters = new[]
                {
                new ReportParameter("monthYear", GetMonthName.MonthInBangla(month) + "/" + string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                new ReportParameter("gradeName", gradeName),
                new ReportParameter("billDate", DateTime.Today.ToString("dd/MM/yyyy")),
                new ReportParameter("totalAllowance", totalAllowance.ToString()),
                new ReportParameter("totalDeduction", totalDeduction.ToString()),
                new ReportParameter("totalPayable", totalPayable.ToString()),
                new ReportParameter("payableBng", payableInword.ToString()),

                };

                report.DataSources.Add(new ReportDataSource("DsShortBill", shortBills));

                report.ReportPath = rptPath;
                report.SetParameters(parameters);
                var pdf = report.Render(renderFormat);
                return File(pdf, mimtype);
            }
            catch (Exception ex)
            {
                return BadRequest("Data is Not Found Or Server Error please Try Again!!");
            }
        }

        private FiscalYear GetFiscalYear(int month, int year)
        {
            string value = "";
            if (month <= 6)
            {
                value = year - 1 + "-" + year;
            }
            else
            {
                value = year + "-" + year + 1;
            }

            return _fiscalYearManager.GetByValue(value);

        }

        private string MonthInEnglish(int month)
        {
            switch (month)
            {
                case 1:
                    return "January";
                    break;
                case 2:
                    return "February";
                    break;
                case 3:
                    return "March";
                    break;
                case 4:
                    return "April";
                    break;
                case 5:
                    return "May";
                    break;
                case 6:
                    return "June";
                    break;
                case 7:
                    return "July";
                    break;
                case 8:
                    return "August";
                    break;
                case 9:
                    return "September";
                    break;
                case 10:
                    return "October";
                    break;
                case 11:
                    return "November";
                    break;
                case 12:
                    return "December";
                    break;
                default:
                    return "";
                    break;

            }

        }
        [HttpGet]
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult UitilityBillReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult UitilityBillReport(int? FromGradeId, int? WingId, int? ToGradeId, int year, int month, int stationId)
        {
            var users = _userManager.Users.Where(c => c.StationId == stationId && (WingId == null || c.WingId == WingId) &&
                            c.IsActive && (FromGradeId == null || c.GradeId >= FromGradeId) &&
                            (ToGradeId == null || c.GradeId <= ToGradeId)).Include(v => v.Department).Include(c => c.Wing).Include(c => c.Station).Include(v => v.Designation);
            var source = CalculateSalary(year, month, users, true);
            var getwing = _wingsManager.GetWingById(WingId);

            List<UtilityBillVm> list = new List<UtilityBillVm>();
            int slNo = 1;
            foreach (DataRow dtRow in source.Rows)
            {
                var empCode = dtRow["EmployeeCode"].ToString();
                var userInfo = users.FirstOrDefault(x => x.EmployeeCode == empCode);
                if (userInfo != null && userInfo.ResidentialStatusId == 2)
                {
                    continue;
                }

                var empNameBn = dtRow["EmployeeNameBangla"].ToString();
                var empDes = dtRow["Designation"].ToString();
                var houseRent = Math.Round(Convert.ToDecimal(dtRow["HouseRentDeduction"]), 2);
                var electricBill = Math.Round(Convert.ToDecimal(dtRow["ElectricBill"]), 2);
                var gasBill = Math.Round(Convert.ToDecimal(dtRow["GasBill"]), 2);
                var waterBill = Math.Round(Convert.ToDecimal(dtRow["WaterBill"]), 2);
                UtilityBillVm obj = new UtilityBillVm()
                {
                    Name = string.Concat(empCode.ToString().Select(c => (char)('\u09E6' + c - '0'))) + "-" + empNameBn + "," + empDes,
                    Elctricity = string.Concat(electricBill.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    Gas = string.Concat(gasBill.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    HouseRent = string.Concat(houseRent.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                    SlNo = string.Concat(slNo.ToString().Select(c => (char)('\u09E6' + c - '0'))),
                    Water = string.Concat(waterBill.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", "."),
                };
                list.Add(obj);
                slNo++;

            }
            string gradeName = "";
            var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            if (FromGradeId != null && ToGradeId != null)
            {
                gradeName = fgrade + "থেকে" + tgrade;
            }
            else
            {
                gradeName = "সকল";
            }


            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\UtilityRpt.rdlc";

            var houseRentSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRentDeduction)", string.Empty)), 2);
            var electricBillSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(ElectricBill)", string.Empty)), 2);
            var gasBillSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(GasBill)", string.Empty)), 2);
            var waterBillSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(WaterBill)", string.Empty)), 2);

            var parameters = new[]
            {
                new ReportParameter("gradeName",gradeName.ToString()),
                new ReportParameter("wing", getwing != null ? getwing.Name.ToString() : " সকল উইং "),
                new ReportParameter("monthYear", GetMonthName.MonthInBangla(month)+"/"+ string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                new ReportParameter("houseRentSum",string.Concat(houseRentSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                new ReportParameter("gasBillSum", string.Concat(electricBillSum.ToString().Select(c => (char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                new ReportParameter("electricBillSum", string.Concat(gasBillSum.ToString().Select(c =>(char)('\u09E6' + c - '0'))).Replace("৤", ".")),
                new ReportParameter("waterBillSum", string.Concat(waterBillSum.ToString().Select(c =>(char)('\u09E6' + c - '0'))).Replace("৤", "."))
            };

            report.DataSources.Add(new ReportDataSource("Utility", list));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);

        }
        [MiddlewareFilter(typeof(MyCustomAuthenticationMiddlewarePipeline))]
        public IActionResult SummaryReport()
        {
            ViewBag.Stations = _userStationPermissionManager.UserWiseLoadStation(_userManager.GetUserId(User));
            ViewBag.Wings = _wingsManager.GetWings();
            ViewBag.Grade = _gradeManager.GetList();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SummaryReport(int? FromGradeId, int? WingId, int? ToGradeId, int year, int month, int stationId)
        {
            var users = _userManager.Users.Where(c => c.StationId == stationId && (WingId == null || c.WingId == WingId) &&
                            c.IsActive && (FromGradeId == null || c.GradeId >= FromGradeId) &&
                            (ToGradeId == null || c.GradeId <= ToGradeId)).Include(v => v.Department).Include(c => c.Wing).Include(c => c.Station).Include(v => v.Designation);
            var source = CalculateSalary(year, month, users, true);
            var getwing = users.FirstOrDefault(c => c.WingId == WingId);




            //allowance
            var basicAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(BasicAllowance)", string.Empty)), 2);
            var medicalAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(MedicalAllowance)", string.Empty)), 2);
            var houseRentAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRentAllowance)", string.Empty)), 2);
            var transportAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(TransportAllowance)", string.Empty)), 2);
            var tiffinAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(TiffinAllowance)", string.Empty)), 2);
            var washAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(WashAllowance)", string.Empty)), 2);
            var educationAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(EducationAllowance)", string.Empty)), 2);
            var chargeAllowance = Math.Round(Convert.ToDecimal(source.Compute("SUM(ChargeAllowance)", string.Empty)), 2);
            var specialBenefit = Math.Round(Convert.ToDecimal(source.Compute("SUM(SpecialBenefit)", string.Empty)), 2);
            var grossSalary = Math.Round(Convert.ToDecimal(source.Compute("SUM(GrossSalary)", string.Empty)), 2);
            //CPF

            var regularCpf = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFRegular)", string.Empty)), 2);

            var cpfFirst = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFFirstCapital)", string.Empty)), 2);

            var cpfSecond = Math.Round(Convert.ToDecimal(source.Compute("SUM(CPFSecondCapital)", string.Empty)), 2);

            var cpfLoanSum = cpfFirst + cpfSecond;

            var houseRentSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRentDeduction)", string.Empty)), 2);
            var electricBillSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(ElectricBill)", string.Empty)), 2);
            var gasBillSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(GasBill)", string.Empty)), 2);
            var waterBillSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(WaterBill)", string.Empty)), 2);


            //Loan

            //Motor Cycle
            var motorCycle1stCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleFirstCapital)", string.Empty)), 2);
            var motorCycle1stInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleFirstInterest)", string.Empty)), 2);
            var motorCycle2ndCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleSecondCapital)", string.Empty)), 2);
            var motorCycle2ndInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(MotorCycleSecondInterest)", string.Empty)), 2);

            var motorCycleSum = motorCycle1stCapital + motorCycle1stInterest + motorCycle2ndCapital + motorCycle2ndInterest;

            //Motor Car

            var carFirstCapitalSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarFirstCapital)", string.Empty)), 2);
            var carFirstInterestSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarFirstInterest)", string.Empty)), 2);
            var carSecondCapitalSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarSecondCapital)", string.Empty)), 2);
            var carSecondInterestSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(CarSecondInterest)", string.Empty)), 2);

            var carSum = carFirstCapitalSum + carFirstInterestSum + carSecondCapitalSum + carSecondInterestSum;

            //House Building
            var houseBuildingFirstCapitalSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingFirstCapital)", string.Empty)), 2);
            var houseBuildingFirstInterestSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingFirstInterest)", string.Empty)), 2);
            var houseBuildingSecondCapitalSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingSecondCapital)", string.Empty)), 2);
            var houseBuildingSecondInterestSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseBuildingSecondInterest)", string.Empty)), 2);

            var houseBuildingSum = houseBuildingFirstCapitalSum + houseBuildingFirstInterestSum + houseBuildingSecondCapitalSum + houseBuildingSecondInterestSum;

            //House repairing
            //House Building
            var houseRepairingFirstCapitalSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingFirstCapital)", string.Empty)), 2);
            var houseRepairingFirstInterestSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingFirstInterest)", string.Empty)), 2);
            var houseRepairingSecondCapitalSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingSecondCapital)", string.Empty)), 2);
            var houseRepairingSecondInterestSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingSecondInterest)", string.Empty)), 2);
            var houseRepairingThirdCapitalSum = 0;// Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingThirdCapital)", string.Empty)), 2);
            var houseRepairingThirdInterestSum = 0;//Math.Round(Convert.ToDecimal(source.Compute("SUM(HouseRepairingThirdInterest)", string.Empty)), 2);

            var houseRepairingSum = houseBuildingFirstCapitalSum + houseBuildingFirstInterestSum + houseBuildingSecondCapitalSum + houseRepairingSecondInterestSum + houseRepairingSecondInterestSum + houseRepairingThirdCapitalSum;

            //Others Loan
            var othersCapital = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersCapital)", string.Empty)), 2);
            var othersInterest = Math.Round(Convert.ToDecimal(source.Compute("SUM(OthersInterest)", string.Empty)), 2);

            var othersSum = othersCapital + othersInterest;


            var basicDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(BasicDeduction)", string.Empty)), 2);

            var transportDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(Transport)", string.Empty)), 2);
            var garageDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(Garage)", string.Empty)), 2);
            var groupInsuranceDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(GroupInsurance)", string.Empty)), 2);

            var welfareFundDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(WelfareFund)", string.Empty)), 2);
            var rehabilitationDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(Rehabilitation)", string.Empty)), 2);
            var incomeTaxAmountDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(IncomeTaxAmount)", string.Empty)), 2);

            var totalDeductionSum = Math.Round(Convert.ToDecimal(source.Compute("SUM(TotalDeduction)", string.Empty)), 2);
            var netSalarySum = Math.Round(Convert.ToDecimal(source.Compute("SUM(NetSalary)", string.Empty)), 2);




            List<SummaryVm> list = new List<SummaryVm>();
            int slNo = 1;
            foreach (DataRow dtRow in source.Rows)
            {
                string remarks = "";
                var empCode = dtRow["EmployeeCode"].ToString();
                var user = users.FirstOrDefault(c => c.EmployeeCode == empCode);
                var empNameBn = dtRow["EmployeeNameBangla"].ToString();
                var empDes = dtRow["Designation"].ToString();

                var cpf1stInstallment = dtRow["CPFFirstInstallment"].ToString();
                if (cpf1stInstallment != "0/0")
                {
                    remarks += "সিপি১-" + Converter.EnglishToBanglaNumberConvert(cpf1stInstallment) + " ,";
                }

                var cpf2ndInstallment = dtRow["CPFSecondInstallment"].ToString();
                if (cpf2ndInstallment != "0/0")
                {
                    remarks += "সিপি২-" + Converter.EnglishToBanglaNumberConvert(cpf2ndInstallment) + " ,";
                }


                var mCycleFirstCaptial = dtRow["MotorCycleFirstCapital"].ToString();
                var mCycleFirstCapitalInstallment = dtRow["MotorCycleFirstCapitalInstallment"].ToString();
                var mCycleFirstInterest = dtRow["MotorCycleFirstInterest"].ToString();
                var mCycleFirstInterestInstallment = dtRow["MotorCycleFirstInterestInstallment"].ToString();

                var fKistiForMotor1 = mCycleFirstCapitalInstallment;
                var fAmountForMotor1 = mCycleFirstCaptial;

                if (Convert.ToDecimal(mCycleFirstCaptial) == 0)
                {
                    fAmountForMotor1 = mCycleFirstInterest;
                    fKistiForMotor1 = mCycleFirstInterestInstallment;

                    if (Convert.ToDecimal(fAmountForMotor1) != 0)
                    {
                        remarks += "মসা১ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForMotor1) + " ,";
                    }

                }
                else
                {
                    remarks += "মসা১-" + Converter.EnglishToBanglaNumberConvert(fKistiForMotor1) + " ,";
                }

                var motorCycleSecondCapital = dtRow["MotorCycleSecondCapital"].ToString();
                var motorCycleSecondCapitalInstallment = dtRow["MotorCycleSecondCapitalInstallment"].ToString();
                var motorCycleSecondInterest = dtRow["MotorCycleSecondInterest"].ToString();
                var motorCycleSecondInterestInstallment = dtRow["MotorCycleSecondInterestInstallment"].ToString();

                var fKistiForMotor2 = motorCycleSecondCapitalInstallment;
                var fAmountForMotor2 = motorCycleSecondCapital;
                if (Convert.ToDecimal(motorCycleSecondCapital) == 0)
                {
                    fAmountForMotor2 = motorCycleSecondInterest;
                    fKistiForMotor2 = motorCycleSecondInterestInstallment;

                    if (Convert.ToDecimal(fAmountForMotor2) != 0)
                    {
                        remarks += "মসা২ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForMotor2) + " ,";
                    }
                }
                else
                {
                    remarks += "মসা২-" + Converter.EnglishToBanglaNumberConvert(fKistiForMotor2) + " ,";
                }


                var carFirstCapital = dtRow["CarFirstCapital"].ToString();
                var carFirstInstallment = dtRow["CarFirstCapitalInstallment"].ToString();
                var carFirstInterest = dtRow["CarFirstInterest"].ToString();
                var carFirstInterestInstallment = dtRow["CarFirstInterestInstallment"].ToString();

                var fKistiForCar1 = carFirstInstallment;
                var fAmountForCar1 = carFirstCapital;

                if (Convert.ToDecimal(carFirstCapital) == 0)
                {
                    fAmountForCar1 = carFirstInterest;
                    fKistiForCar1 = carFirstInterestInstallment;

                    if (Convert.ToDecimal(fAmountForCar1) != 0)
                    {
                        remarks += "মকা১ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForCar1) + " ,";
                    }
                }
                else
                {
                    remarks += "মকা১-" + Converter.EnglishToBanglaNumberConvert(fKistiForCar1) + " ,";
                }


                var carSecondCapital = dtRow["CarSecondCapital"].ToString();
                var carSecondCapitalInstallment = dtRow["CarSecondCapitalInstallment"].ToString();
                var carSecondInterest = dtRow["CarSecondInterest"].ToString();
                var carSecondInterestInstallment = dtRow["CarSecondInterestInstallment"].ToString();

                var fKistiForCar2 = carSecondCapitalInstallment;
                var fAmountForCar2 = carSecondCapital;

                if (Convert.ToDecimal(carSecondCapital) == 0)
                {
                    fAmountForCar2 = carSecondInterest;
                    fKistiForCar2 = carSecondInterestInstallment;

                    if (Convert.ToDecimal(fAmountForCar2) != 0)
                    {
                        remarks += "মকা২ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForCar2) + " ,";
                    }
                }
                else
                {
                    remarks += "মকা২-" + Converter.EnglishToBanglaNumberConvert(fKistiForCar2) + " ,";
                }


                var houseBuildingFirstCapital = dtRow["HouseBuildingFirstCapital"].ToString();
                var houseBuildingFirstCapitalInstallment = dtRow["HouseBuildingFirstCapitalInstallment"].ToString();
                var houseBuildingFirstInterest = dtRow["HouseBuildingFirstInterest"].ToString();
                var houseBuildingFirstInterestInstallment = dtRow["HouseBuildingFirstInterestInstallment"].ToString();

                var fKistiForhb1 = houseBuildingFirstCapitalInstallment;
                var fAmountForhb1 = houseBuildingFirstCapital;

                if (Convert.ToDecimal(houseBuildingFirstCapital) == 0)
                {
                    fAmountForhb1 = houseBuildingFirstInterest;
                    fKistiForhb1 = houseBuildingFirstInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhb1) != 0)
                    {
                        remarks += "গৃনি১ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhb1) + " ,";
                    }
                }
                else
                {
                    remarks += "গৃনি১-" + Converter.EnglishToBanglaNumberConvert(fKistiForhb1) + " ,";
                }

                var houseBuildingSecondCapital = dtRow["HouseBuildingSecondCapital"].ToString();
                var houseBuildingSecondCapitalInstallment = dtRow["HouseBuildingSecondCapitalInstallment"].ToString();
                var houseBuildingSecondInterest = dtRow["HouseBuildingSecondInterest"].ToString();
                var houseBuildingSecondInterestInstallment = dtRow["HouseBuildingSecondInterestInstallment"].ToString();

                var fKistiForhb2 = houseBuildingSecondCapitalInstallment;
                var fAmountForhb2 = houseBuildingSecondCapital;

                if (Convert.ToDecimal(houseBuildingSecondCapital) == 0)
                {
                    fAmountForhb2 = houseBuildingSecondInterest;
                    fKistiForhb2 = houseBuildingSecondInterestInstallment;

                    if (Convert.ToDecimal(fAmountForhb2) != 0)
                    {
                        remarks += "গৃনি১ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhb1) + " ,";
                    }
                }
                else
                {
                    remarks += "গৃনি১-" + Converter.EnglishToBanglaNumberConvert(fKistiForhb1) + " ,";
                }


                var houseRepairingFirstCapital = dtRow["HouseRepairingFirstCapital"].ToString();
                var houseRepairingFirstCapitalInstallment = dtRow["HouseRepairingFirstCapitalInstallment"].ToString();
                var houseRepairingFirstInterest = dtRow["HouseRepairingFirstInterest"].ToString();
                var houseRepairingFirstInterestInstallment = dtRow["HouseRepairingFirstInterestInstallment"].ToString();

                var fKistiForhr1 = houseRepairingFirstCapitalInstallment;
                var fAmountForhr1 = houseRepairingFirstCapital;

                if (Convert.ToDecimal(houseRepairingFirstCapital) == 0)
                {
                    fAmountForhr1 = houseRepairingFirstInterest;
                    fKistiForhr1 = houseRepairingFirstInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhr1) != 0)
                    {
                        remarks += "গৃমে১ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhr1) + " ,";
                    }

                }
                else
                {
                    remarks += "গৃমে১-" + Converter.EnglishToBanglaNumberConvert(fKistiForhr1) + " ,";
                }

                var houseRepairingSecondCapital = dtRow["HouseRepairingSecondCapital"].ToString();
                var houseRepairingSecondCapitalInstallment = dtRow["HouseRepairingSecondCapitalInstallment"].ToString();
                var houseRepairingSecondInterest = dtRow["HouseRepairingSecondInterest"].ToString();
                var houseRepairingSecondInterestInstallment = dtRow["HouseRepairingSecondInterestInstallment"].ToString();

                var fKistiForhr2 = houseRepairingSecondCapitalInstallment;
                var fAmountForhr2 = houseRepairingSecondCapital;

                if (Convert.ToDecimal(houseRepairingSecondCapital) == 0)
                {
                    fAmountForhr2 = houseRepairingSecondInterest;
                    fKistiForhr2 = houseRepairingSecondInterestInstallment;

                    if (Convert.ToDecimal(fAmountForhr2) != 0)
                    {
                        remarks += "গৃমে২ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhr2) + " ,";
                    }
                }
                else
                {
                    remarks += "গৃমে২ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhr2) + " ,";

                }


                var houseRepairingThirdCapital = 0; //dtRow["HouseRepairingThirdCapital"].ToString();
                var houseRepairingThirdCapitalInstallment = 0; //dtRow["HouseRepairingThirdCapitalInstallment"].ToString();
                var houseRepairingThirdInterest = 0;//dtRow["HouseRepairingThirdInterest"].ToString();
                var houseRepairingThirdInterestInstallment = 0;// dtRow["HouseRepairingThirdInterestInstallment"].ToString();

                var fKistiForhr3 = houseRepairingThirdCapitalInstallment;
                var fAmountForhr3 = houseRepairingThirdCapital;

                if (Convert.ToDecimal(houseRepairingThirdCapital) == 0)
                {
                    fAmountForhr3 = houseRepairingThirdInterest;
                    fKistiForhr3 = houseRepairingThirdInterestInstallment;
                    if (Convert.ToDecimal(fAmountForhr3) != 0)
                    {
                        remarks += "গৃমে২ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhr3) + " ,";
                    }
                }
                else
                {
                    remarks += "গৃমে২ফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForhr3) + " ,";
                }


                var othersAdvanceCapital = dtRow["OthersAdvanceCapital"].ToString();
                var othersAdvanceCapitalInstallment = dtRow["OthersAdvanceCapitalInstallment"].ToString();
                var othersAdvanceInterest = dtRow["OthersAdvanceInterest"].ToString();
                var othersAdvanceInterestInstallment = dtRow["OthersAdvanceInterestInstallment"].ToString();


                var fKistiForOthers = othersAdvanceCapitalInstallment;
                var fAmountForOthers = othersAdvanceCapital;

                if (Convert.ToDecimal(othersAdvanceCapital) == 0)
                {
                    fAmountForOthers = othersAdvanceInterest;
                    fKistiForOthers = othersAdvanceInterestInstallment;

                    if (Convert.ToDecimal(fAmountForOthers) != 0)
                    {
                        remarks += "অঅঋফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForOthers) + " ,";
                    }

                }
                else
                {
                    remarks += "অঅঋফি-" + Converter.EnglishToBanglaNumberConvert(fKistiForOthers) + " ,";

                }
                var prlDate = user.BirthDate.AddYears(user.PlrAge);
                var lastDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                if (prlDate < lastDate)
                {
                    remarks += "PRL";
                }
                else
                {
                    remarks += "Regular";
                }

                SummaryVm obj = new SummaryVm();

                obj.SlNo = Converter.EnglishToBanglaNumberConvert(slNo);
                obj.Name = Converter.EnglishToBanglaNumberConvert(empCode) + "-" + empNameBn + "," + empDes;
                obj.BasicSalary = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["BasicAllowance"]), 2));
                obj.MedicalAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["MedicalAllowance"]), 2));
                obj.HouseRentAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["HouseRentAllowance"]), 2));
                obj.TransportAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["TransportAllowance"]), 2));
                obj.TiffinAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["TiffinAllowance"]), 2));
                obj.WashAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["WashAllowance"]), 2));
                obj.EducationAllwance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["EducationAllowance"]), 2));
                obj.ChargeAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["ChargeAllowance"]), 2));
                obj.SpecialBenefit = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["SpecialBenefit"]), 2));
                obj.SumOfAllowance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["GrossSalary"]), 2));
                obj.CpfRegular = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["CPFRegular"]), 2));
                obj.CpfFirstAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["CPFFirstCapital"]), 2));
                obj.CpfSecondAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["CPFSecondCapital"]), 2));
                obj.HouseRentDeduction = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["HouseRentDeduction"]), 2));
                obj.ElectricBill = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["ElectricBill"]), 2));
                obj.GasBill = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["GasBill"]), 2));
                obj.WaterBill = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["WaterBill"]), 2));
                obj.MotorCycleFirstAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForMotor1), 2));
                obj.MotorCycleSecondAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForMotor2), 2));
                obj.MotorCarFirstAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForCar1), 2));
                obj.MotorCarSecondAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForCar2), 2));
                obj.HouseBuldingFirstAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForhb1), 2));
                obj.HouseBuildingSecondAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForhb2), 2));
                obj.HouseRepairingFirstAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForhr1), 2));
                obj.HouseRepairingSecondAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForhr2), 2));
                obj.HouseRepairingThirdAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForhr3), 2));
                obj.OthersAdvncedAmount = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(fAmountForOthers), 2));
                obj.BasicDeduction = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["BasicDeduction"]), 2));
                obj.TransportDeduction = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["Transport"]), 2));
                obj.Garage = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["Garage"]), 2));
                obj.WelfareFund = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["WelfareFund"]), 2));
                obj.Rehabilitation = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["Rehabilitation"]), 2));
                obj.GroupInsurance = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["GroupInsurance"]), 2));
                obj.IncomeTax = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["IncomeTaxAmount"]), 2));
                obj.SunOfDeduction = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["TotalDeduction"]), 2));
                obj.NetPay = Converter.EnglishToBanglaNumberConvert(Math.Round(Convert.ToDecimal(dtRow["NetSalary"]), 2));
                obj.Remarks = remarks;



                list.Add(obj);
                slNo++;

            }


            string renderFormat = "PDF";
            string mimtype = "application/pdf";
            using var report = new LocalReport();
            report.EnableExternalImages = true;
            string rptPath = $"{this.webHostEnvironment.WebRootPath}\\Reports\\SummarySalary.rdlc";

            string gradeName = "";
            var fgrade = string.Concat(FromGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            var tgrade = string.Concat(ToGradeId.ToString().Select(c => (char)('\u09E6' + c - '0')));
            if (FromGradeId != null && ToGradeId != null)
            {
                gradeName = fgrade + "থেকে" + tgrade;
            }
            else
            {
                gradeName = "সকল";
            }


            var parameters = new[]
            {
                new ReportParameter("monthYear", GetMonthName.MonthInBangla(month)+"/"+ string.Concat(year.ToString().Select(c => (char)('\u09E6' + c - '0')))),
                new ReportParameter("wing",getwing?.Wing?.Name.ToString()),
                new ReportParameter("basicAllowance",Converter.EnglishToBanglaNumberConvert(basicAllowance)),
                new ReportParameter("medicalAllowance",Converter.EnglishToBanglaNumberConvert(medicalAllowance)),
                new ReportParameter("houseRentAllowance",Converter.EnglishToBanglaNumberConvert(houseRentAllowance)),
                new ReportParameter("transportAllowance",Converter.EnglishToBanglaNumberConvert(transportAllowance)),
                new ReportParameter("tiffinAllowance",Converter.EnglishToBanglaNumberConvert(tiffinAllowance)),
                new ReportParameter("washAllowance",Converter.EnglishToBanglaNumberConvert(washAllowance)),
                new ReportParameter("educationAllowance",Converter.EnglishToBanglaNumberConvert(educationAllowance)),
                new ReportParameter("chargeAllowance",Converter.EnglishToBanglaNumberConvert(chargeAllowance)),
                new ReportParameter("specialBenefitSum",Converter.EnglishToBanglaNumberConvert(specialBenefit)),
                new ReportParameter("grossSalary",Converter.EnglishToBanglaNumberConvert(grossSalary)),
                new ReportParameter("regularCpf",Converter.EnglishToBanglaNumberConvert(regularCpf)),
                new ReportParameter("cpfLoanSum",Converter.EnglishToBanglaNumberConvert(cpfLoanSum)),
                new ReportParameter("houseRentSum",Converter.EnglishToBanglaNumberConvert(houseRentSum)),
                new ReportParameter("electricBillSum",Converter.EnglishToBanglaNumberConvert(electricBillSum)),
                new ReportParameter("gasBillSum",Converter.EnglishToBanglaNumberConvert(gasBillSum)),
                new ReportParameter("waterBillSum",Converter.EnglishToBanglaNumberConvert(waterBillSum)),
                new ReportParameter("motorCycleSum",Converter.EnglishToBanglaNumberConvert(motorCycleSum)),
                new ReportParameter("carSum",Converter.EnglishToBanglaNumberConvert(carSum)),
                new ReportParameter("houseBuildingSum",Converter.EnglishToBanglaNumberConvert(houseBuildingSum)),
                new ReportParameter("houseRepairingSum",Converter.EnglishToBanglaNumberConvert(houseRepairingSum)),
                new ReportParameter("othersSum",Converter.EnglishToBanglaNumberConvert(othersSum)),
                new ReportParameter("basicDeductionSum",Converter.EnglishToBanglaNumberConvert(basicDeductionSum)),
                new ReportParameter("transportDeductionSum",Converter.EnglishToBanglaNumberConvert(transportDeductionSum)),
                new ReportParameter("garageDeductionSum",Converter.EnglishToBanglaNumberConvert(garageDeductionSum)),
                new ReportParameter("groupInsuranceDeductionSum",Converter.EnglishToBanglaNumberConvert(groupInsuranceDeductionSum)),
                new ReportParameter("welfareFundDeductionSum",Converter.EnglishToBanglaNumberConvert(welfareFundDeductionSum)),
                new ReportParameter("rehabilitationDeductionSum",Converter.EnglishToBanglaNumberConvert(rehabilitationDeductionSum)),
                new ReportParameter("incomeTaxAmountDeductionSum",Converter.EnglishToBanglaNumberConvert(incomeTaxAmountDeductionSum)),
                new ReportParameter("totalDeductionSum",Converter.EnglishToBanglaNumberConvert(totalDeductionSum)),
                new ReportParameter("netSalarySum",Converter.EnglishToBanglaNumberConvert(netSalarySum)),

            };

            report.DataSources.Add(new ReportDataSource("SummaryRpt", list));

            report.ReportPath = rptPath;
            report.SetParameters(parameters);
            var pdf = report.Render(renderFormat);
            return File(pdf, mimtype);

        }

    }
}
