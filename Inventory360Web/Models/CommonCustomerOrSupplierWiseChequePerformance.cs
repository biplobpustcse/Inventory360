using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonCustomerOrSupplierWiseChequePerformance : CommonHeader
    {
        public string ReportName { get; set; }
        public string ChequeType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string LocationName { get; set; }
        public List<CommonCustomerOrSupplierWiseChequePerformanceDetail> CommonCustomerOrSupplierWiseChequePerformanceDetail { get; set; }
    }
    public class CommonCustomerOrSupplierWiseChequePerformanceDetail
    {
        public long CustomerOrSupplierId { get; set; }
        public string CustomerOrSupplierName { get; set; }
        public string CustomerOrSupplierCode { get; set; }
        public string CustomerOrSupplierAddress { get; set; }
        public string CustomerOrSupplierPhoneNo { get; set; }
        public Nullable<int> NoOfCheque { get; set; }
        public Nullable<decimal> ChequeAmount { get; set; }
        public Nullable<int> NoOfDisHonerCheque { get; set; }
        public Nullable<decimal> DisHonerChequeAmount { get; set; }
        public Nullable<decimal> DisHonerChequePercentageAmount { get; set; }
        public Nullable<int> NoOfBalanceAdjustedCheque { get; set; }
        public Nullable<decimal> BalanceAdjustedChequeAmount { get; set; }
        public Nullable<decimal> BalanceAdjustedChequePercentageAmount { get; set; }
        public Nullable<int> NoOfPurelyHonoredCheque { get; set; }
        public Nullable<decimal> PurelyHonoredChequeAmount { get; set; }
        public Nullable<decimal> PurelyHonoredChequePercentageAmount { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}