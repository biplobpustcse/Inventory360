using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class ReportAccountsLedgerOrProvisionalLedger
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string ReportTitle { get; set; }
        public string LedgerName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CurrencyType { get; set; }
        public bool WithDetail { get; set; }
        public string CurrencyCultureInfo { get; set; }
        public List<TempAccountsLedgerOrProvisionalLedger> LedgerLists { get; set; }
    }

    public class TempAccountsLedgerOrProvisionalLedger
    {
        public DateTime Date { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDescription { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
        public List<TempAccountsLedgerDetail> TempDetail { get; set; }
    }

    public class TempAccountsLedgerDetail
    {
        public string AccountsName { get; set; }
        public decimal DrAmount { get; set; }
        public decimal CrAmount { get; set; }
    }
}