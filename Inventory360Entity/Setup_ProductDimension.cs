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
    
    public partial class Setup_ProductDimension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_ProductDimension()
        {
            this.Setup_ConvertionRatioDetail = new HashSet<Setup_ConvertionRatioDetail>();
            this.Setup_Price = new HashSet<Setup_Price>();
            this.Stock_BadStock = new HashSet<Stock_BadStock>();
            this.Stock_CurrentStock = new HashSet<Stock_CurrentStock>();
            this.Stock_LIMStock = new HashSet<Stock_LIMStock>();
            this.Stock_RMAStock = new HashSet<Stock_RMAStock>();
            this.Stock_TransitStock = new HashSet<Stock_TransitStock>();
            this.Task_ComplainReceiveDetail = new HashSet<Task_ComplainReceiveDetail>();
            this.Task_ComplainReceiveDetail_SpareProduct = new HashSet<Task_ComplainReceiveDetail_SpareProduct>();
            this.Task_ConvertionDetail = new HashSet<Task_ConvertionDetail>();
            this.Task_CustomerDeliveryDetail = new HashSet<Task_CustomerDeliveryDetail>();
            this.Task_CustomerDeliveryDetail1 = new HashSet<Task_CustomerDeliveryDetail>();
            this.Task_CustomerDeliveryDetail_SpareProduct = new HashSet<Task_CustomerDeliveryDetail_SpareProduct>();
            this.Task_DeliveryChallanDetail = new HashSet<Task_DeliveryChallanDetail>();
            this.Task_GoodsReceiveDetail = new HashSet<Task_GoodsReceiveDetail>();
            this.Task_ImportCostingProduct = new HashSet<Task_ImportCostingProduct>();
            this.Task_ImportedStockInDetail = new HashSet<Task_ImportedStockInDetail>();
            this.Task_ItemRequisitionDetail = new HashSet<Task_ItemRequisitionDetail>();
            this.Task_LIMStockInDetail = new HashSet<Task_LIMStockInDetail>();
            this.Task_ProformaInvoiceDetail = new HashSet<Task_ProformaInvoiceDetail>();
            this.Task_PurchaseOrderDetail = new HashSet<Task_PurchaseOrderDetail>();
            this.Task_PurchaseReturnDetail = new HashSet<Task_PurchaseReturnDetail>();
            this.Task_ReceiveFinalizeDetail = new HashSet<Task_ReceiveFinalizeDetail>();
            this.Task_ReplacementClaimDetail = new HashSet<Task_ReplacementClaimDetail>();
            this.Task_ReplacementReceiveDetail = new HashSet<Task_ReplacementReceiveDetail>();
            this.Task_ReplacementReceiveDetail1 = new HashSet<Task_ReplacementReceiveDetail>();
            this.Task_RequisitionFinalizeDetail = new HashSet<Task_RequisitionFinalizeDetail>();
            this.Task_SalesInvoiceDetail = new HashSet<Task_SalesInvoiceDetail>();
            this.Task_SalesOrderDetail = new HashSet<Task_SalesOrderDetail>();
            this.Task_SalesQuotationDetail = new HashSet<Task_SalesQuotationDetail>();
            this.Task_SalesReturnDetail = new HashSet<Task_SalesReturnDetail>();
            this.Task_StockAdjustmentDetail = new HashSet<Task_StockAdjustmentDetail>();
            this.Task_TransferChallanDetail = new HashSet<Task_TransferChallanDetail>();
            this.Task_TransferOrderDetail = new HashSet<Task_TransferOrderDetail>();
            this.Task_TransferReceiveDetail = new HashSet<Task_TransferReceiveDetail>();
            this.Task_TransferRequisitionFinalizeDetail = new HashSet<Task_TransferRequisitionFinalizeDetail>();
            this.Temp_SalesInvoiceDetail = new HashSet<Temp_SalesInvoiceDetail>();
        }
    
        public long ProductDimensionId { get; set; }
        public string Code { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> MeasurementId { get; set; }
        public Nullable<long> SizeId { get; set; }
        public Nullable<long> StyleId { get; set; }
        public Nullable<long> ColorId { get; set; }
        public string SKUCode { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Color Setup_Color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_ConvertionRatioDetail> Setup_ConvertionRatioDetail { get; set; }
        public virtual Setup_Measurement Setup_Measurement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_Price> Setup_Price { get; set; }
        public virtual Setup_Product Setup_Product { get; set; }
        public virtual Setup_Size Setup_Size { get; set; }
        public virtual Setup_Style Setup_Style { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_BadStock> Stock_BadStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_CurrentStock> Stock_CurrentStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_LIMStock> Stock_LIMStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_RMAStock> Stock_RMAStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stock_TransitStock> Stock_TransitStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ComplainReceiveDetail> Task_ComplainReceiveDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ComplainReceiveDetail_SpareProduct> Task_ComplainReceiveDetail_SpareProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ConvertionDetail> Task_ConvertionDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_CustomerDeliveryDetail> Task_CustomerDeliveryDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_CustomerDeliveryDetail> Task_CustomerDeliveryDetail1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_CustomerDeliveryDetail_SpareProduct> Task_CustomerDeliveryDetail_SpareProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_DeliveryChallanDetail> Task_DeliveryChallanDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_GoodsReceiveDetail> Task_GoodsReceiveDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ImportCostingProduct> Task_ImportCostingProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ImportedStockInDetail> Task_ImportedStockInDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ItemRequisitionDetail> Task_ItemRequisitionDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_LIMStockInDetail> Task_LIMStockInDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ProformaInvoiceDetail> Task_ProformaInvoiceDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PurchaseOrderDetail> Task_PurchaseOrderDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PurchaseReturnDetail> Task_PurchaseReturnDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReceiveFinalizeDetail> Task_ReceiveFinalizeDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReplacementClaimDetail> Task_ReplacementClaimDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReplacementReceiveDetail> Task_ReplacementReceiveDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReplacementReceiveDetail> Task_ReplacementReceiveDetail1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_RequisitionFinalizeDetail> Task_RequisitionFinalizeDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesInvoiceDetail> Task_SalesInvoiceDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesOrderDetail> Task_SalesOrderDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesQuotationDetail> Task_SalesQuotationDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesReturnDetail> Task_SalesReturnDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_StockAdjustmentDetail> Task_StockAdjustmentDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferChallanDetail> Task_TransferChallanDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferOrderDetail> Task_TransferOrderDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferReceiveDetail> Task_TransferReceiveDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_TransferRequisitionFinalizeDetail> Task_TransferRequisitionFinalizeDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_SalesInvoiceDetail> Temp_SalesInvoiceDetail { get; set; }
    }
}