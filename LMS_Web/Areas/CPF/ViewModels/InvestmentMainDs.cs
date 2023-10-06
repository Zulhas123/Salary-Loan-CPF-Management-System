using System.Collections.Generic;

namespace LMS_Web.Areas.CPF.ViewModels
{
    public class InvestmentMainDs
    {
        public string Id { get; set; } 
        public string Name { get; set; }     
        public string FMonth { get; set; }     
        public string TMonth { get; set; }     
        public string Comment { get; set; }     
        public string PrlMessage { get; set; }
        public string FinalTotalInvest { get; set; }
        public string FinalTotalInterest{ get; set; }
        public string FinalTotalContribution { get; set; }
    }
}
