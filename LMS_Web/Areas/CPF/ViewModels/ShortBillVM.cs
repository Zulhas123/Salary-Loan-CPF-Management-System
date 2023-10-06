namespace LMS_Web.Areas.CPF.ViewModels
{
    public class ShortBillVM
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Scale { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public decimal CurrentBasic { get; set; }
        public string BankAccountNo { get; set; }

        public decimal BasicAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal HouseRentAllowance { get; set; }
        public decimal DearnessAllowance { get; set; }
        public decimal MobileCellphoneAllowance { get; set; }
        public decimal TelephoneAllowance { get; set; }
        public decimal ChargeAllowance { get; set; }
        public decimal EducationAllowance { get; set; }
        public decimal HonoraryAllowance { get; set; }
        public decimal TravelingAllowance { get; set; }
        public decimal AdvanceAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal PrantikSubidha { get; set; }
        public decimal BonusRefund { get; set; }
        public decimal OthersAllowance { get; set; }
        public decimal TiffinAllowance { get; set; }
        public decimal WashAllowance { get; set; }
        public decimal SpecialBenefit { get; set; }
        public decimal ArrearsBasic { get; set; }
        public decimal FestivalAllowance { get; set; }

        //Deduction
        public decimal CPFRegular { get; set; }
        public decimal CPFAdditional { get; set; }
        public decimal CPFArrears { get; set; }
        public decimal HouseRentDeduction { get; set; }
        public decimal ElectricBill { get; set; }
        public decimal GasBill { get; set; }
        public decimal WaterBill { get; set; }
        public decimal IncomeTaxAmount { get; set; }
        public string IncomeTaxInstallment { get; set; }


        //Loan

        public decimal CPFFirstCapital { get; set; }
        public string CPFFirstInstallment { get; set; }
        public decimal CPFSecondCapital { get; set; }
        public string CPFSecondInstallment { get; set; }
        //
        public decimal MotorCycleFirstCapital { get; set; }
        public string MotorCycleFirstCapitalInstallment { get; set; }
        public decimal MotorCycleFirstInterest { get; set; }
        public string MotorCycleFirstInterestInstallment { get; set; }
        //
        public decimal MotorCycleSecondCapital { get; set; }
        public string MotorCycleSecondCapitalInstallment { get; set; }
        public decimal MotorCycleSecondInterest { get; set; }
        public string MotorCycleSecondInterestInstallment { get; set; }
        //
        public decimal CarFirstCapital { get; set; }
        public string CarFirstCapitalInstallment { get; set; }
        public decimal CarFirstInterest { get; set; }
        public string CarFirstInterestInstallment { get; set; }
        //
        public decimal CarSecondCapital { get; set; }
        public string CarSecondCapitalInstallment { get; set; }
        public decimal CarSecondInterest { get; set; }
        public string CarsSecondInterestInstallment { get; set; }
        //


        public decimal HouseBuildingFirstCapital { get; set; }
        public string HouseBuildingFirstCapitalInstallment { get; set; }
        public decimal HouseBuildingFirstInterest { get; set; }
        public string HouseBuildingFirstInterestInstallment { get; set; }

        //
        public decimal HouseBuildingSecondCapital { get; set; }
        public string HouseBuildingSecondCapitalInstallment { get; set; }
        public decimal HouseBuildingSecondInterest { get; set; }
        public string HouseBuildingSecondInterestInstallment { get; set; }
        //
        public decimal HouseRepairingFirstCapital { get; set; }
        public string HouseRepairingFirstCapitalInstallment { get; set; }
        public decimal HouseRepairingFirstInterest { get; set; }
        public string HouseRepairingFirstInterestInstallment { get; set; }
        //
        public decimal HouseRepairingSecondCapital { get; set; }
        public string HouseRepairingSecondCapitalInstallment { get; set; }
        public decimal HouseRepairingSecondInterest { get; set; }
        public string HouseRepairingSecondInterestInstallment { get; set; }

        //
        public decimal HouseRepairingThirdCapital { get; set; }
        public string HouseRepairingThirdCapitalInstallment { get; set; }
        public decimal HouseRepairingThirdInterest { get; set; }
        public string HouseRepairingThirdInterestInstallment { get; set; }
        //
        public decimal OthersAdvanceCapital { get; set; }
        public string OthersAdvanceCapitalInstallment { get; set; }
        public decimal OthersAdvanceInterest { get; set; }
        public string OthersAdvanceInterestInstallment { get; set; }
        //
        public decimal OthersCapital { get; set; }
        public string OthersCapitalInstallment { get; set; }
        public decimal OthersInterest { get; set; }
        public string OthersInterestInstallment { get; set; }

        //
        public decimal BasicDeduction { get; set; }
        public decimal TransportGarage { get; set; }
        public decimal Transport { get; set; }
        public decimal GroupInsurance { get; set; }
        public decimal WelfareFund { get; set; }
        public decimal Rehabilitation { get; set; }



    }
}
