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
    
    public partial class Temp_SalesInvoiceDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Temp_SalesInvoiceDetail()
        {
            this.Temp_SalesInvoiceDetail_GovtDuty = new HashSet<Temp_SalesInvoiceDetail_GovtDuty>();
            this.Temp_SalesInvoiceDetailSerial = new HashSet<Temp_SalesInvoiceDetailSerial>();
        }
    
        public System.Guid InvoiceDetailId { get; set; }
        public System.Guid InvoiceId { get; set; }
        public string ChallanNo { get; set; }
        public long ProductEntrySequence { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Price1 { get; set; }
        public decimal Price2 { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal Discount1Value { get; set; }
        public decimal Discount2Value { get; set; }
        public decimal Discount { get; set; }
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal InvoiceDiscount1 { get; set; }
        public decimal InvoiceDiscount2 { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public Nullable<long> WareHouseId { get; set; }
        public bool IsIncludingGovtDuty { get; set; }
        public string ShortSpecification { get; set; }
        public bool IsWarrantyAvailable { get; set; }
        public decimal WarrantyDays { get; set; }
        public bool IsServiceWarranty { get; set; }
        public decimal ServiceWarrantyDays { get; set; }
    
        public virtual Setup_Location Setup_Location { get; set; }
        public virtual Setup_Product Setup_Product { get; set; }
        public virtual Setup_ProductDimension Setup_ProductDimension { get; set; }
        public virtual Setup_UnitType Setup_UnitType { get; set; }
        public virtual Temp_SalesInvoice Temp_SalesInvoice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_SalesInvoiceDetail_GovtDuty> Temp_SalesInvoiceDetail_GovtDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_SalesInvoiceDetailSerial> Temp_SalesInvoiceDetailSerial { get; set; }
    }
}
