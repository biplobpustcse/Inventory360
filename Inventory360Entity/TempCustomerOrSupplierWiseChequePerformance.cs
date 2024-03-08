//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory360Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class TempCustomerOrSupplierWiseChequePerformance
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
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public string LocationName { get; set; }
        public Nullable<int> NoOfPurelyHonoredCheque { get; set; }
        public Nullable<decimal> PurelyHonoredChequeAmount { get; set; }
        public Nullable<decimal> PurelyHonoredChequePercentageAmount { get; set; }
    }
}