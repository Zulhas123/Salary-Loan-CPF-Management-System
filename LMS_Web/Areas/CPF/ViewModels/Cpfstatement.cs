using Microsoft.Reporting.NETCore;
using System.Collections.Generic;

namespace LMS_Web.Areas.CPF.ViewModels
{
    public class Cpfstatement
    {
       public ReportParameter[] ReportParameters { get; set; }
        public List<CpfVM> sources { get; set; }
    }
}
