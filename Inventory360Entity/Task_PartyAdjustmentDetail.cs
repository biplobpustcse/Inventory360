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
    
    public partial class Task_PartyAdjustmentDetail
    {
        public System.Guid AdjustmentDetailId { get; set; }
        public System.Guid AdjustmentId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public string Particulars { get; set; }
        public decimal AdjustedAmount { get; set; }
        public decimal AdjustedAmount1 { get; set; }
        public decimal AdjustedAmount2 { get; set; }
    
        public virtual Setup_Customer Setup_Customer { get; set; }
        public virtual Setup_Supplier Setup_Supplier { get; set; }
        public virtual Task_PartyAdjustment Task_PartyAdjustment { get; set; }
    }
}