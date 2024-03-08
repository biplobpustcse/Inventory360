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
    
    public partial class Task_TransferRequisitionFinalizeDetail
    {
        public System.Guid RequisitionDetailId { get; set; }
        public System.Guid RequisitionId { get; set; }
        public Nullable<System.Guid> ItemRequisitionId { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal OrderedQuantity { get; set; }
    
        public virtual Setup_Product Setup_Product { get; set; }
        public virtual Setup_ProductDimension Setup_ProductDimension { get; set; }
        public virtual Setup_UnitType Setup_UnitType { get; set; }
        public virtual Task_ItemRequisition Task_ItemRequisition { get; set; }
        public virtual Task_TransferRequisitionFinalize Task_TransferRequisitionFinalize { get; set; }
    }
}
