using LMS_Web.Areas.Salary.Models;

namespace LMS_Web.Areas.Salary.Interface.Manager
{
    public interface ISalaryStepInfoManager
    {
        SalaryIncrement GetByMonthYear(int month, int year);
    }
}
