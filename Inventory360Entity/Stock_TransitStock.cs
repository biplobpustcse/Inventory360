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
    
    public partial class Stock_TransitStock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stock_TransitStock()
        {
            this.Stock_TransitStockSerial = new HashSet<Stock_TransitStockSerial>();
        }
    
        public System.Guid TransitStockId { get; set; }
        public string TransitType { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public string BatchNo { get; set; }
        public Nullable<System.DateTime> ManufactureDate { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public string ReferenceNo { get; set; }
        public System.DateTime ReferenceDate { get; set; }
        public Nullable<System.Guid> GoodsReceiveId { get; set; }
        public Nullable<System.Guid> ImportedStockInId { get; set; }
        public Nullable<long> SupplierId { get; set; }
        public long LocationId { get; set; }
        public Nullable<long> WareHouseId { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Setup_Company Setup_Company { get; set; }
        public virtual Setup_Location Setup_Location { get; set; }
        public virtual Setup_Location Setup_Location1 { get; set; }
        public virtual Setup_Product Setup_Product { get; set; }
        public virtual Setup_ProductDimension Setup_ProductDimension { get; set; }
        public virtual Setup_Supplier Setup_Supplier { get; set; }
        public virtual Setup_UnitType Setup_UnitType { get; set; }
        public virtual Task_GoodsReceive Task_GoodsReceive { get; set; }
        public virtual Task_ImportedStockIn Task_ImportedStockIn { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_TransitStockSerial> Stock_TransitStockSerial { get; set; }
    }
}
