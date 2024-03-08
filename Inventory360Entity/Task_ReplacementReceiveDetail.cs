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
    
    public partial class Task_ReplacementReceiveDetail
    {
        public System.Guid ReceiveDetailId { get; set; }
        public System.Guid ReceiveId { get; set; }
        public System.Guid ReplacementClaimId { get; set; }
        public long PreviousProductId { get; set; }
        public Nullable<long> PreviousProductDimensionId { get; set; }
        public long PreviousUnitTypeId { get; set; }
        public string PreviousSerial { get; set; }
        public long NewProductId { get; set; }
        public Nullable<long> NewProductDimensionId { get; set; }
        public long NewUnitTypeId { get; set; }
        public string NewSerial { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public long ReplacementType { get; set; }
        public string AdjustmentType { get; set; }
        public decimal AdjustedAmount { get; set; }
        public decimal AdjustedAmount1 { get; set; }
        public decimal AdjustedAmount2 { get; set; }
    
        public virtual Setup_Product Setup_Product { get; set; }
        public virtual Setup_Product Setup_Product1 { get; set; }
        public virtual Setup_ProductDimension Setup_ProductDimension { get; set; }
        public virtual Setup_ProductDimension Setup_ProductDimension1 { get; set; }
        public virtual Setup_UnitType Setup_UnitType { get; set; }
        public virtual Setup_UnitType Setup_UnitType1 { get; set; }
        public virtual Task_ReplacementClaim Task_ReplacementClaim { get; set; }
        public virtual Task_ReplacementReceive Task_ReplacementReceive { get; set; }
    }
}
