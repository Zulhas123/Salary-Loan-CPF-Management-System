using System.Collections.Generic;
using Microsoft.Reporting.NETCore;


namespace LMS_Web.Areas.CPF.ViewModels
{
    public class InvestmentStatementVM
    {
        public ReportParameter[] ReportParameters { get; set; }
        public List<InvestmentVm> sources { get; set; }
        public List<InvestmentDescriptionVm> description { get; set; }
    }
}
