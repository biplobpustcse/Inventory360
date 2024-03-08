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
    
    public partial class Task_TransferReceiveDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task_TransferReceiveDetail()
        {
            this.Task_TransferReceiveDetailSerial = new HashSet<Task_TransferReceiveDetailSerial>();
        }
    
        public System.Guid ReceiveDetailId { get; set; }
        public System.Guid ReceiveId { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public long PrimaryUnitTypeId { get; set; }
        public Nullable<long> SecondaryUnitTypeId { get; set; }
        public Nullable<long> TertiaryUnitTypeId { get; set; }
        public decimal SecondaryConversionRatio { get; set; }
        public decimal TertiaryConversionRatio { get; set; }
        public Nullable<long> ReceivedWareHouseId { get; set; }
        public Nullable<long> TranFromWareHouseId { get; set; }
        public Nullable<System.Guid> GoodsReceiveId { get; set; }
        public Nullable<System.Guid> ImportedStockInId { get; set; }
        public Nullable<long> SupplierId { get; set; }
    
        public virtual Setup_Location Setup_Location { get; set; }
        public virtual Setup_Location Setup_Location1 { get; set; }
        public virtual Setup_Product Setup_Product { get; set; }
        public virtual Setup_ProductDimension Setup_ProductDimension { get; set; }
        public virtual Setup_Supplier Setup_Supplier { get; set; }
        public virtual Setup_UnitType Setup_UnitType { get; set; }
        public virtual Setup_UnitType Setup_UnitType1 { get; set; }
        public virtual Setup_UnitType Setup_UnitType2 { get; set; }
        public virtual Setup_UnitType Setup_UnitType3 { get; set; }
        public virtual Task_GoodsReceive Task_GoodsReceive { get; set; }
        public virtual Task_ImportedStockIn Task_ImportedStockIn { get; set; }
        public virtual Task_TransferReceive Task_TransferReceive { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferReceiveDetailSerial> Task_TransferReceiveDetailSerial { get; set; }
    }
}
