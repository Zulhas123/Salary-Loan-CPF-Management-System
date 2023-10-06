using LMS_Web.Areas.Loan.Models;
using LMS_Web.Interface.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS_Web.Areas.CPF.Models;
using LMS_Web.Areas.Settings.Models;

namespace LMS_Web.Areas.CPF.Interface
{
    interface ICpfPercentManager : IBaseManager<CpfPercent>
    {   CpfPercent GetByName(string name);       
        ICollection<CpfPercent> GetList();
        CpfPercent GetById(int id);
    }
}
