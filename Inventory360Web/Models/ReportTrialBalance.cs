using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class ReportTrialBalance
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string AccountsLevel { get; set; }
        public string LedgerName { get; set; }
        public string ReportTitle { get; set; }
        public string Report { get; set; }
        public string ReportType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CurrencyType { get; set; }
        public string CurrencyCultureInfo { get; set; }
        public List<TempTrialBalance> TrialBalanceLists { get; set; }
    }

    public class TempTrialBalance
    {
        public string GroupName { get; set; }
        public string SubGroupName { get; set; }
        public string ControlName { get; set; }
        public string SubsidiaryName { get; set; }
        public string AccountsName { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public decimal DrTranAmount { get; set; }
        public decimal CrTranAmount { get; set; }
    }
}