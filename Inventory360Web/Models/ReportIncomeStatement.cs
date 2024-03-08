using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class ReportIncomeStatement
    {
        public FeatureList Features { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string ReportType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CurrencyType { get; set; }
        public string CurrencyCultureInfo { get; set; }
        public List<ReportIncomeStatementDetail> IncomeStatementLists { get; set; }
    }

    public class ReportIncomeStatementDetail
    {
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Categorization { get; set; }
        public byte CategorizationId { get; set; }
        public string HeadName { get; set; }
        public decimal Amount { get; set; }
    }

    public class FeatureList
    {
        public bool IsPerpetualInventory { get; set; }
    }
}