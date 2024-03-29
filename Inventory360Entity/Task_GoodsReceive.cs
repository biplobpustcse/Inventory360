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
    
    public partial class Task_GoodsReceive
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task_GoodsReceive()
        {
            this.Stock_BadStock = new HashSet<Stock_BadStock>();
            this.Stock_CurrentStock = new HashSet<Stock_CurrentStock>();
            this.Stock_RMAStock = new HashSet<Stock_RMAStock>();
            this.Stock_TransitStock = new HashSet<Stock_TransitStock>();
            this.Task_ConvertionDetail = new HashSet<Task_ConvertionDetail>();
            this.Task_DeliveryChallanDetail = new HashSet<Task_DeliveryChallanDetail>();
            this.Task_GoodsReceiveDetail = new HashSet<Task_GoodsReceiveDetail>();
            this.Task_PaymentMapping = new HashSet<Task_PaymentMapping>();
            this.Task_PurchaseReturnDetail = new HashSet<Task_PurchaseReturnDetail>();
            this.Task_ReceiveFinalizeDetail = new HashSet<Task_ReceiveFinalizeDetail>();
            this.Task_SalesInvoiceDetail = new HashSet<Task_SalesInvoiceDetail>();
            this.Task_SalesReturnDetail = new HashSet<Task_SalesReturnDetail>();
            this.Task_TransferChallanDetail = new HashSet<Task_TransferChallanDetail>();
            this.Task_TransferReceiveDetail = new HashSet<Task_TransferReceiveDetail>();
        }
    
        public System.Guid ReceiveId { get; set; }
        public string ReceiveNo { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<System.DateTime> ReferenceDate { get; set; }
        public long SupplierId { get; set; }
        public System.Guid OrderId { get; set; }
        public string Remarks { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public bool IsSettledByRecFinalize { get; set; }
        public string Approved { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string CancelReason { get; set; }
        public string PurchaseOperationType { get; set; }
        public decimal ReceiveAmount { get; set; }
        public decimal Receive1Amount { get; set; }
        public decimal Receive2Amount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal Discount1Value { get; set; }
        public decimal Discount2Value { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Discount1Amount { get; set; }
        public decimal Discount2Amount { get; set; }
        public bool IsSettledByReturn { get; set; }
        public Nullable<System.Guid> VoucherId { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<long> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Security_User Security_User1 { get; set; }
        public virtual Security_User Security_User2 { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        public virtual Setup_Location Setup_Location { get; set; }
        public virtual Setup_Supplier Setup_Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_BadStock> Stock_BadStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_CurrentStock> Stock_CurrentStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_RMAStock> Stock_RMAStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_TransitStock> Stock_TransitStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ConvertionDetail> Task_ConvertionDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_DeliveryChallanDetail> Task_DeliveryChallanDetail { get; set; }
        public virtual Task_PurchaseOrder Task_PurchaseOrder { get; set; }
        public virtual Task_Voucher Task_Voucher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_GoodsReceiveDetail> Task_GoodsReceiveDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PaymentMapping> Task_PaymentMapping { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PurchaseReturnDetail> Task_PurchaseReturnDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReceiveFinalizeDetail> Task_ReceiveFinalizeDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesInvoiceDetail> Task_SalesInvoiceDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesReturnDetail> Task_SalesReturnDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferChallanDetail> Task_TransferChallanDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferReceiveDetail> Task_TransferReceiveDetail { get; set; }
    }
}
