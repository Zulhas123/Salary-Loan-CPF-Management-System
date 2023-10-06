using LMS_Web.Areas.CPF.Controllers;
using LMS_Web.Areas.CPF.Interface;
using LMS_Web.Areas.CPF.Manager;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.CPF.ViewModels;
using LMS_Web.Areas.Loan.Manager;
using LMS_Web.Areas.Loan.Models;
using LMS_Web.Areas.Salary.Manager;
using LMS_Web.Areas.Salary.Models;
using LMS_Web.Areas.Settings.Models;
using LMS_Web.Data;
using LMS_Web.Models;
using LMS_Web.SecurityExtension;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace LMS_Web.Areas.Salary.Controllers
{
    //[ApiController]
    //[Route("Controller/Action")]
    public class UtilityController : ControllerBase
    {
        private UserManager<AppUser> userManager;
        SalaryController salaryController;
        private readonly CpfPercentManager _cpfPercentManager;
        private readonly CpfInfoManager _cpfInfoManager;
        private readonly InvestmentInfoManager _investmentInfoManager;
        private readonly IUserFundManager _userFundManager;
        private readonly UserWiseLoanManager _userWiseLoanManager;
        private static DataTable salary;
        private LoanInstallmentInfoManager _userInstallmentInfoManager;
        private readonly SalaryHistoryManager _salaryHistoryManager;
        public UtilityController(ApplicationDbContext dbContext, UserManager<AppUser> _userManager, IWebHostEnvironment _webHostEnvironment)
        {
            userManager = _userManager;
            _investmentInfoManager = new InvestmentInfoManager(dbContext);
            _cpfInfoManager = new CpfInfoManager(dbContext);
            _cpfPercentManager = new CpfPercentManager(dbContext);
            salaryController = new SalaryController(dbContext, _userManager, _webHostEnvironment);
            _userFundManager = new UserFundManager(dbContext);
            _userWiseLoanManager = new UserWiseLoanManager(dbContext);
            _userInstallmentInfoManager = new LoanInstallmentInfoManager(dbContext);
            _salaryHistoryManager = new SalaryHistoryManager(dbContext);
        }

        private void GetSalaryInfo(int year, int month)
        {
            IIncludableQueryable<AppUser, Designation> users = userManager.Users.Where(c => c.IsActive).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
            var _salary = salaryController.CalculateSalary(year, month, users, true);
            salary = _salary;
        }



        internal void SaveSalaryHistory(int year, int month)
        {
            try
            {
                if (salary == null || salary.Rows.Count <= 0)
                {
                    GetSalaryInfo(year, month);
                }
                List<SalaryHistory> insertList = new List<SalaryHistory>();
                //List<SalaryHistory> updatelist = new List<SalaryHistory>();

                foreach (DataRow dtRow in salary.Rows)
                {
                    SalaryHistory obj = new SalaryHistory();
                    obj.Year = year;
                    obj.Month = month;
                    obj.UserId = dtRow["UserId"].ToString();
                    obj.EmployeeCode = dtRow["EmployeeCode"].ToString();
                    obj.EmployeeName = dtRow["EmployeeName"].ToString();
                    obj.Scale = dtRow["Scale"].ToString();
                    obj.Department = dtRow["Department"].ToString();
                    obj.Designation = dtRow["Designation"].ToString();
                    obj.BasicAllowance = Convert.ToDecimal(dtRow["BasicAllowance"]);
                    obj.CurrentBasic = Convert.ToDecimal(dtRow["CurrentBasic"]);
                    obj.BankAccountNo = dtRow["BankAccountNo"].ToString();
                    obj.MedicalAllowance = Convert.ToDecimal(dtRow["MedicalAllowance"]);
                    obj.HouseRentAllowance = Convert.ToDecimal(dtRow["HouseRentAllowance"]);
                    obj.DearnessAllowance = Convert.ToDecimal(dtRow["DearnessAllowance"]);
                    obj.MobileCellphoneAllowance = Convert.ToDecimal(dtRow["MobileCellphoneAllowance"]);
                    obj.TelephoneAllowance = Convert.ToDecimal(dtRow["TelephoneAllowance"]);
                    obj.ChargeAllowance = Convert.ToDecimal(dtRow["ChargeAllowance"]);
                    obj.EducationAllowance = Convert.ToDecimal(dtRow["EducationAllowance"]);
                    obj.HonoraryAllowance = Convert.ToDecimal(dtRow["HonoraryAllowance"]);
                    obj.TravelingAllowance = Convert.ToDecimal(dtRow["TravelingAllowance"]);
                    obj.AdvanceAllowance = Convert.ToDecimal(dtRow["AdvanceAllowance"]);
                    obj.TransportAllowance = Convert.ToDecimal(dtRow["TransportAllowance"]);
                    obj.PrantikSubidha = Convert.ToDecimal(dtRow["PrantikSubidha"]);
                    obj.BonusRefund = Convert.ToDecimal(dtRow["BonusRefund"]);
                    obj.OthersAllowance = Convert.ToDecimal(dtRow["OthersAllowance"]);
                    obj.TiffinAllowance = Convert.ToDecimal(dtRow["TiffinAllowance"]);
                    obj.WashAllowance = Convert.ToDecimal(dtRow["WashAllowance"]);
                    obj.ArrearsBasic = Convert.ToDecimal(dtRow["ArrearsBasic"]);
                    obj.FestivalAllowance = Convert.ToDecimal(dtRow["FestivalAllowance"]);
                    obj.SpecialBenifit = Convert.ToDecimal(dtRow["SpecialBenefit"]);
                    obj.CPFRegular = Convert.ToDecimal(dtRow["CPFRegular"]);
                    obj.CPFAdditional = Convert.ToDecimal(dtRow["CPFAdditional"]);
                    obj.CPFArrears = Convert.ToDecimal(dtRow["CPFArrears"]);
                    obj.HouseRentDeduction = Convert.ToDecimal(dtRow["HouseRentDeduction"]);
                    obj.ElectricBill = Convert.ToDecimal(dtRow["ElectricBill"]);
                    obj.GasBill = Convert.ToDecimal(dtRow["GasBill"]);
                    obj.WaterBill = Convert.ToDecimal(dtRow["WaterBill"]);
                    obj.IncomeTaxAmount = Convert.ToDecimal(dtRow["IncomeTaxAmount"]);
                    obj.IncomeTaxInstallment = dtRow["IncomeTaxInstallment"].ToString();
                    obj.CPFFirstCapital = Convert.ToDecimal(dtRow["CPFFirstCapital"]);
                    obj.CPFFirstInstallment = dtRow["CPFFirstInstallment"].ToString();
                    obj.CPFSecondCapital = Convert.ToDecimal(dtRow["CPFSecondCapital"]);
                    obj.CPFSecondInstallment = dtRow["CPFSecondInstallment"].ToString();
                    obj.MotorCycleFirstCapital = Convert.ToDecimal(dtRow["MotorCycleFirstCapital"]);
                    obj.MotorCycleFirstCapitalInstallment = dtRow["MotorCycleFirstCapitalInstallment"].ToString();
                    obj.MotorCycleFirstInterest = Convert.ToDecimal(dtRow["MotorCycleFirstInterest"]);
                    obj.MotorCycleFirstInterestInstallment = dtRow["MotorCycleFirstInterestInstallment"].ToString();
                    obj.MotorCycleSecondCapital = Convert.ToDecimal(dtRow["MotorCycleSecondCapital"]);
                    obj.MotorCycleSecondCapitalInstallment = dtRow["MotorCycleSecondCapitalInstallment"].ToString();
                    obj.MotorCycleSecondInterest = Convert.ToDecimal(dtRow["MotorCycleSecondInterest"]);
                    obj.MotorCycleSecondInterestInstallment = dtRow["MotorCycleSecondInterestInstallment"].ToString();
                    obj.CarFirstCapital = Convert.ToDecimal(dtRow["CarFirstCapital"]);
                    obj.CarFirstCapitalInstallment = dtRow["CarFirstCapitalInstallment"].ToString();
                    obj.CarFirstInterest = Convert.ToDecimal(dtRow["CarFirstInterest"]);
                    obj.CarFirstInterestInstallment = dtRow["CarFirstInterestInstallment"].ToString();
                    obj.CarSecondCapital = Convert.ToDecimal(dtRow["CarSecondCapital"]);
                    obj.CarSecondCapitalInstallment = dtRow["CarSecondCapitalInstallment"].ToString();
                    obj.CarSecondInterest = Convert.ToDecimal(dtRow["CarSecondInterest"]);
                    obj.CarSecondInterestInstallment = dtRow["CarSecondInterestInstallment"].ToString();
                    obj.HouseBuildingFirstCapital = Convert.ToDecimal(dtRow["HouseBuildingFirstCapital"]);
                    obj.HouseBuildingFirstCapitalInstallment = dtRow["HouseBuildingFirstCapitalInstallment"].ToString();
                    obj.HouseBuildingFirstInterest = Convert.ToDecimal(dtRow["HouseBuildingFirstInterest"]);
                    obj.HouseBuildingFirstInterestInstallment = dtRow["HouseBuildingFirstInterestInstallment"].ToString();
                    obj.HouseBuildingSecondCapital = Convert.ToDecimal(dtRow["HouseBuildingSecondCapital"]);
                    obj.HouseBuildingSecondCapitalInstallment = dtRow["HouseBuildingSecondCapitalInstallment"].ToString();
                    obj.HouseBuildingSecondInterest = Convert.ToDecimal(dtRow["HouseBuildingSecondInterest"]);
                    obj.HouseBuildingSecondInterestInstallment = dtRow["HouseBuildingSecondInterestInstallment"].ToString();
                    obj.HouseRepairingFirstCapital = Convert.ToDecimal(dtRow["HouseRepairingFirstCapital"]);
                    obj.HouseRepairingFirstCapitalInstallment = dtRow["HouseRepairingFirstCapitalInstallment"].ToString();
                    obj.HouseRepairingFirstInterest = Convert.ToDecimal(dtRow["HouseRepairingFirstInterest"]);
                    obj.HouseRepairingFirstInterestInstallment = dtRow["HouseRepairingFirstInterestInstallment"].ToString();
                    obj.HouseRepairingSecondCapital = Convert.ToDecimal(dtRow["HouseRepairingSecondCapital"]);
                    obj.HouseRepairingSecondCapitalInstallment = dtRow["HouseRepairingSecondCapitalInstallment"].ToString();
                    obj.HouseRepairingSecondInterest = Convert.ToDecimal(dtRow["HouseRepairingSecondInterest"]);
                    obj.HouseRepairingSecondInterestInstallment = dtRow["HouseRepairingSecondInterestInstallment"].ToString();
                    obj.OthersAdvanceCapital = Convert.ToDecimal(dtRow["OthersAdvanceCapital"]);
                    obj.OthersAdvanceCapitalInstallment = dtRow["OthersAdvanceCapitalInstallment"].ToString();
                    obj.OthersAdvanceInterest = Convert.ToDecimal(dtRow["OthersAdvanceInterest"]);
                    obj.OthersAdvanceInterestInstallment = dtRow["OthersAdvanceInterestInstallment"].ToString();
                    obj.OthersCapital = Convert.ToDecimal(dtRow["OthersCapital"]);
                    obj.OthersCapitalInstallment = dtRow["OthersCapitalInstallment"].ToString();
                    obj.OthersInterest = Convert.ToDecimal(dtRow["OthersInterest"]);
                    obj.OthersInterestInstallment = dtRow["OthersInterestInstallment"].ToString();
                    //obj.HouseRepairingThirdCapital = Convert.ToDecimal(dtRow["HouseRepairingThirdCapital"]);
                    //obj.HouseRepairingThirdCapitalInstallment = dtRow["HouseRepairingThirdCapitalInstallment"].ToString();
                    //obj.HouseRepairingThirdInterest = Convert.ToDecimal(dtRow["HouseRepairingThirdInterest"]);
                    //obj.HouseRepairingThirdInterestInstallment = dtRow["HouseRepairingThirdInterestInstallment"].ToString();
                    obj.BasicDeduction = Convert.ToDecimal(dtRow["BasicDeduction"]);
                    obj.Transport = Convert.ToDecimal(dtRow["Transport"]);
                    obj.Garage = Convert.ToDecimal(dtRow["Garage"]);
                    obj.GroupInsurance = Convert.ToDecimal(dtRow["GroupInsurance"]);
                    obj.WelfareFund = Convert.ToDecimal(dtRow["WelfareFund"]);
                    obj.Rehabilitation = Convert.ToDecimal(dtRow["Rehabilitation"]);
                    obj.GrossSalary = Convert.ToDecimal(dtRow["GrossSalary"]);
                    obj.TotalDeduction = Convert.ToDecimal(dtRow["TotalDeduction"]);
                    obj.NetSalary = Convert.ToDecimal(dtRow["NetSalary"]);
                    obj.NetInWord = dtRow["NetInWord"].ToString();
                    obj.Remarks = dtRow["Remarks"].ToString();
                    obj.NetSalaryBangla = dtRow["NetSalaryBangla"].ToString();
                    obj.EmployeeNameBangla = dtRow["EmployeeNameBangla"].ToString();
                    obj.DesignationBangla = dtRow["DesignationBangla"].ToString();
                    insertList.Add(obj);
                }
                _salaryHistoryManager.Add(insertList);
            }
            catch (Exception e)
            {


            }
        }

        [HttpPost]

        public IActionResult CalculateCpf(int year, int month)
        {
            try
            {


                List<CpfInfo> insertlist = new List<CpfInfo>();
                List<CpfInfo> updatelist = new List<CpfInfo>();

                IIncludableQueryable<AppUser, Designation> users = userManager.Users.Where(c => c.IsActive).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
                //var fggfhgh = userManager.Users.Where(c => c.IsActive).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation).ToList(); ;
                var salary = salaryController.CalculateSalary(year, month, users, false);

                var percent = _cpfPercentManager.GetByName("CPF Regular")?.Percent ?? 10;
                var govtContpercent = _cpfPercentManager.GetByName("GovtContribution")?.Percent ?? 12;

                //var lastYear = currentDate.AddMonths(-1).Year;
                //var lastMonth = currentDate.AddMonths(-1).Month;
                // var cpfInfos = _cpfInfoManager.GetListByMonth(year??0, month??0);
                var row = salary.Rows.Count;



                foreach (DataRow dtRow in salary.Rows)
                {
                    var cpf = dtRow["CPFRegular"].ToString();
                    //if (Convert.ToDecimal(cpf) == 0)
                    //{
                    //    continue;
                    //}
                    var userid = dtRow["UserId"].ToString();

                    var existUsser = _cpfInfoManager.GetListByMonthUser(year, month, userid);


                    var cpfAdd = dtRow["CPFAdditional"].ToString();
                    var basic = dtRow["CurrentBasic"].ToString();
                    var cpfArrear = dtRow["CPFArrears"].ToString();
                    var arrearBasic = dtRow["ArrearsBasic"].ToString();


                    CpfInfo cpfInfo = new CpfInfo();
                    if (existUsser == null)
                    {
                        cpfInfo.AppUserId = userid;
                        cpfInfo.BasicSalary = Convert.ToDecimal(basic);
                        cpfInfo.SelfContribution = Convert.ToDecimal(cpf);
                        cpfInfo.ArrearsBasic = Convert.ToDecimal(cpfArrear) + (Convert.ToDecimal(arrearBasic) / govtContpercent);
                        cpfInfo.GovtContribution = (Convert.ToDecimal(basic) / govtContpercent);
                        cpfInfo.TotalContribution = Convert.ToDecimal(cpf) + (Convert.ToDecimal(basic) / govtContpercent) + (Convert.ToDecimal(cpfArrear));

                        //var cpfGrandTotal = 0;
                        cpfInfo.GrandTotal = 0;//(cpfGrandTotal?.GrandTotal ?? 0) + (cpfInfo?.TotalContribution ?? 0);
                        cpfInfo.Month = month;
                        cpfInfo.Year = year;
                        insertlist.Add(cpfInfo);
                    }
                    else
                    {

                        existUsser.BasicSalary = Convert.ToDecimal(basic);
                        existUsser.SelfContribution = Convert.ToDecimal(cpf);
                        existUsser.ArrearsBasic = Convert.ToDecimal(cpfArrear) + (Convert.ToDecimal(arrearBasic) / govtContpercent);
                        existUsser.GovtContribution = (Convert.ToDecimal(basic) / govtContpercent);
                        existUsser.TotalContribution = Convert.ToDecimal(cpf) + (Convert.ToDecimal(basic) / govtContpercent) + (Convert.ToDecimal(cpfArrear));

                        //var cpfGrandTotal = 0;
                        existUsser.GrandTotal = 0;//(cpfGrandTotal?.GrandTotal ?? 0) + (cpfInfo?.TotalContribution ?? 0);
                        updatelist.Add(existUsser);

                    }

                }
                var save = _cpfInfoManager.Add(insertlist);

                _cpfInfoManager.Update(updatelist);
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest();

            }

        }
        [HttpPost]
        public IActionResult Investment(int year, int month)
        {
            try
            {


                List<InvestmentInfo> insertlist = new List<InvestmentInfo>();
                List<InvestmentInfo> updatelist = new List<InvestmentInfo>();
                var monthFirstDate = new DateTime(year, month, 10);
                var cpfMonth = monthFirstDate.AddMonths(-1);


                var lastYear = cpfMonth.Year;
                var lastMonth = cpfMonth.Month;

                var lastMonthCpf = _cpfInfoManager.GetListByMonth(lastYear, lastMonth);

                var investmentInfos = _investmentInfoManager.GetListByMonth(year, month);
                var lastMonthIvestmentInfos = _investmentInfoManager.GetListByMonth(lastYear, lastMonth);

                foreach (var CpfInfo in lastMonthCpf)
                {
                    var total = CpfInfo.TotalContribution;
                    if (CpfInfo.SelfContribution == 0)
                    {
                        total = 0;
                    }

                    var existData = investmentInfos.FirstOrDefault(c => c.AppUserId == CpfInfo.AppUserId);
                    var lastInvest = lastMonthIvestmentInfos.FirstOrDefault(c => c.AppUserId == CpfInfo.AppUserId);
                    if (existData != null)
                    {

                        existData.InvestmentAmount = total;
                        existData.TotalInvestment = (lastInvest?.TotalInvestment ?? 0) + total;
                        updatelist.Add(existData);
                    }
                    else
                    {
                        InvestmentInfo investmentInfo = new InvestmentInfo();
                        investmentInfo.AppUserId = CpfInfo.AppUserId;
                        investmentInfo.InvestmentAmount = total;
                        investmentInfo.TotalInvestment = (lastInvest?.TotalInvestment ?? 0) + total;
                        investmentInfo.Month = month;
                        investmentInfo.Year = year;
                        insertlist.Add(investmentInfo);

                    }

                }
                _investmentInfoManager.Add(insertlist);
                _investmentInfoManager.Update(updatelist);
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        public void FundCalculate(int year, int month)
        {
            try
            {
                List<UserFundInfo> insertlist = new List<UserFundInfo>();
                List<UserFundInfo> updatelist = new List<UserFundInfo>();

                IIncludableQueryable<AppUser, Designation> users = userManager.Users.Where(c => c.IsActive).Include(c => c.Station).Include(v => v.Department).Include(v => v.Designation);
                var salary = salaryController.CalculateSalary(year, month, users, false);
                foreach (DataRow dtRow in salary.Rows)
                {
                    //var userid = dtRow["UserId"].ToString();
                    var groupInsurancepf = dtRow["GroupInsurance"].ToString();
                    var rehabilitation = dtRow["Rehabilitation"].ToString();
                    var userId = dtRow["UserId"].ToString();
                    var WelfareFund = dtRow["WelfareFund"].ToString();
                    UserFundInfo FundInfo = new UserFundInfo();

                    var getMonthData = _userFundManager.GetByUserAndMonth(userId, month, year);
                    if (getMonthData == null)
                    {
                        FundInfo.AppUserId = userId;
                        FundInfo.GroupInsurance = Convert.ToDecimal(groupInsurancepf);
                        FundInfo.Rehabilitation = Convert.ToDecimal(rehabilitation);
                        FundInfo.WelfareFund = Convert.ToDecimal(WelfareFund);
                        FundInfo.Month = month;
                        FundInfo.Year = year;
                        insertlist.Add(FundInfo);
                    }
                    else
                    {
                        getMonthData.GroupInsurance = Convert.ToDecimal(groupInsurancepf);
                        getMonthData.WelfareFund = Convert.ToDecimal(WelfareFund);
                        getMonthData.Rehabilitation = Convert.ToDecimal(rehabilitation);
                        updatelist.Add(getMonthData);
                    }

                }
                _userFundManager.Add(insertlist);
                _userFundManager.Update(updatelist);
            }
            catch (Exception e)
            {


            }

        }
        public void PaidLoanAfterlastInstallment(int year, int month)
        {
            try
            {
                List<UserWiseLoan> updateData = new List<UserWiseLoan>();
                var allLoan = _userWiseLoanManager.GetActiveLoans();
                //var AllLoan = _userWiseLoanManager.GetList();
                foreach (var item in allLoan)
                {

                    var currentInstalmentNo = _userInstallmentInfoManager.GetCurrentInstalmentByLoan(item.Id, year, month);
                    if (currentInstalmentNo != null)
                    {
                        var currentCapInstlmentNo = currentInstalmentNo?.CapitalInstallmentNo;
                        var currentInInstlNO = currentInstalmentNo?.InterestInstallmentNo;
                        if (currentCapInstlmentNo == 0)
                        {
                            if (currentInInstlNO == item?.NoOfInstallmentForInterest)
                            {
                                item.IsPaid = true;
                                item.LoanHeads = null;
                                updateData.Add(item);
                            }
                        }
                        else
                        {
                            if (item.LoanHeads.LoanHeadType == Loan.LoanHeadType.CPF && currentCapInstlmentNo == item.NoOfInstallment)
                            {
                                item.IsPaid = true;
                                item.LoanHeads = null;
                                updateData.Add(item);
                            }

                        }

                    }
                    else
                    {
                        item.IsPaid = true;
                        item.LoanHeads = null;
                        updateData.Add(item);
                    }

                }
                _userWiseLoanManager.Update(updateData);

            }
            catch (Exception e)
            {


            }



        }
    }
}
