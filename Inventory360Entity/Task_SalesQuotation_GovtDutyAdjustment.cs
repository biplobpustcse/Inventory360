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
    
    public partial class Task_SalesQuotation_GovtDutyAdjustment
    {
        public System.Guid QuotationDutyAdjustmentId { get; set; }
        public System.Guid QuotationId { get; set; }
        public long GovtDutyAdjustmentId { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
    
        public virtual Setup_GovtDutyAdjustment Setup_GovtDutyAdjustment { get; set; }
        public virtual Task_SalesQuotation Task_SalesQuotation { get; set; }
    }
}
