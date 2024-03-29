﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Inventory360Entities : DbContext
    {
        public Inventory360Entities()
            : base("name=Inventory360Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Configuration_Code> Configuration_Code { get; set; }
        public virtual DbSet<Configuration_CurrencyRate> Configuration_CurrencyRate { get; set; }
        public virtual DbSet<Configuration_EventWiseCharge> Configuration_EventWiseCharge { get; set; }
        public virtual DbSet<Configuration_Features> Configuration_Features { get; set; }
        public virtual DbSet<Configuration_FormattingTag> Configuration_FormattingTag { get; set; }
        public virtual DbSet<Configuration_GovtDutyRate_HSCode> Configuration_GovtDutyRate_HSCode { get; set; }
        public virtual DbSet<Configuration_GovtDutyRate_Location> Configuration_GovtDutyRate_Location { get; set; }
        public virtual DbSet<Configuration_OperationalEvent> Configuration_OperationalEvent { get; set; }
        public virtual DbSet<Configuration_OperationalEventDetail> Configuration_OperationalEventDetail { get; set; }
        public virtual DbSet<Configuration_OperationType> Configuration_OperationType { get; set; }
        public virtual DbSet<Configuration_PaymentMode> Configuration_PaymentMode { get; set; }
        public virtual DbSet<Configuration_Voucher> Configuration_Voucher { get; set; }
        public virtual DbSet<Configuration_VoucherDetail> Configuration_VoucherDetail { get; set; }
        public virtual DbSet<Others_Documents> Others_Documents { get; set; }
        public virtual DbSet<Others_Menu> Others_Menu { get; set; }
        public virtual DbSet<Others_Report> Others_Report { get; set; }
        public virtual DbSet<Others_ReportDesignConfig> Others_ReportDesignConfig { get; set; }
        public virtual DbSet<Security_Level> Security_Level { get; set; }
        public virtual DbSet<Security_Role> Security_Role { get; set; }
        public virtual DbSet<Security_RoleWiseMenuPermission> Security_RoleWiseMenuPermission { get; set; }
        public virtual DbSet<Security_User> Security_User { get; set; }
        public virtual DbSet<Security_UserLocation> Security_UserLocation { get; set; }
        public virtual DbSet<Security_UserWiseMenuPermission> Security_UserWiseMenuPermission { get; set; }
        public virtual DbSet<Setup_Accounts> Setup_Accounts { get; set; }
        public virtual DbSet<Setup_AccountsCashBankIdentification> Setup_AccountsCashBankIdentification { get; set; }
        public virtual DbSet<Setup_AccountsControl> Setup_AccountsControl { get; set; }
        public virtual DbSet<Setup_AccountsDetail_Location> Setup_AccountsDetail_Location { get; set; }
        public virtual DbSet<Setup_AccountsGroup> Setup_AccountsGroup { get; set; }
        public virtual DbSet<Setup_AccountsSubGroup> Setup_AccountsSubGroup { get; set; }
        public virtual DbSet<Setup_AccountsSubsidiary> Setup_AccountsSubsidiary { get; set; }
        public virtual DbSet<Setup_Bank> Setup_Bank { get; set; }
        public virtual DbSet<Setup_Brand> Setup_Brand { get; set; }
        public virtual DbSet<Setup_Capacity> Setup_Capacity { get; set; }
        public virtual DbSet<Setup_Charge> Setup_Charge { get; set; }
        public virtual DbSet<Setup_Color> Setup_Color { get; set; }
        public virtual DbSet<Setup_Company> Setup_Company { get; set; }
        public virtual DbSet<Setup_ConvertionRatio> Setup_ConvertionRatio { get; set; }
        public virtual DbSet<Setup_ConvertionRatioDetail> Setup_ConvertionRatioDetail { get; set; }
        public virtual DbSet<Setup_CostingControl> Setup_CostingControl { get; set; }
        public virtual DbSet<Setup_CostingGroup> Setup_CostingGroup { get; set; }
        public virtual DbSet<Setup_CostingHead> Setup_CostingHead { get; set; }
        public virtual DbSet<Setup_Country> Setup_Country { get; set; }
        public virtual DbSet<Setup_Customer> Setup_Customer { get; set; }
        public virtual DbSet<Setup_CustomerGroup> Setup_CustomerGroup { get; set; }
        public virtual DbSet<Setup_Designation> Setup_Designation { get; set; }
        public virtual DbSet<Setup_DocumentsGroup> Setup_DocumentsGroup { get; set; }
        public virtual DbSet<Setup_DocumentsTitle> Setup_DocumentsTitle { get; set; }
        public virtual DbSet<Setup_Employee> Setup_Employee { get; set; }
        public virtual DbSet<Setup_Features> Setup_Features { get; set; }
        public virtual DbSet<Setup_GeoDistrict> Setup_GeoDistrict { get; set; }
        public virtual DbSet<Setup_GeoDivision> Setup_GeoDivision { get; set; }
        public virtual DbSet<Setup_GeoPoliceStation> Setup_GeoPoliceStation { get; set; }
        public virtual DbSet<Setup_GeoRegion> Setup_GeoRegion { get; set; }
        public virtual DbSet<Setup_GovtDuty> Setup_GovtDuty { get; set; }
        public virtual DbSet<Setup_GovtDutyAdjustment> Setup_GovtDutyAdjustment { get; set; }
        public virtual DbSet<Setup_HSCode> Setup_HSCode { get; set; }
        public virtual DbSet<Setup_Location> Setup_Location { get; set; }
        public virtual DbSet<Setup_Measurement> Setup_Measurement { get; set; }
        public virtual DbSet<Setup_Port> Setup_Port { get; set; }
        public virtual DbSet<Setup_Price> Setup_Price { get; set; }
        public virtual DbSet<Setup_PriceDetail> Setup_PriceDetail { get; set; }
        public virtual DbSet<Setup_PriceType> Setup_PriceType { get; set; }
        public virtual DbSet<Setup_Problem> Setup_Problem { get; set; }
        public virtual DbSet<Setup_Product> Setup_Product { get; set; }
        public virtual DbSet<Setup_ProductCategory> Setup_ProductCategory { get; set; }
        public virtual DbSet<Setup_ProductDimension> Setup_ProductDimension { get; set; }
        public virtual DbSet<Setup_ProductGroup> Setup_ProductGroup { get; set; }
        public virtual DbSet<Setup_ProductSubGroup> Setup_ProductSubGroup { get; set; }
        public virtual DbSet<Setup_Profession> Setup_Profession { get; set; }
        public virtual DbSet<Setup_Project> Setup_Project { get; set; }
        public virtual DbSet<Setup_RelatedProduct> Setup_RelatedProduct { get; set; }
        public virtual DbSet<Setup_Size> Setup_Size { get; set; }
        public virtual DbSet<Setup_Style> Setup_Style { get; set; }
        public virtual DbSet<Setup_SubFeatures> Setup_SubFeatures { get; set; }
        public virtual DbSet<Setup_Supplier> Setup_Supplier { get; set; }
        public virtual DbSet<Setup_SupplierGroup> Setup_SupplierGroup { get; set; }
        public virtual DbSet<Setup_TemplateHeader> Setup_TemplateHeader { get; set; }
        public virtual DbSet<Setup_TermsAndConditions> Setup_TermsAndConditions { get; set; }
        public virtual DbSet<Setup_Transport> Setup_Transport { get; set; }
        public virtual DbSet<Setup_TransportType> Setup_TransportType { get; set; }
        public virtual DbSet<Setup_UnitType> Setup_UnitType { get; set; }
        public virtual DbSet<Stock_BadStock> Stock_BadStock { get; set; }
        public virtual DbSet<Stock_BadStockSerial> Stock_BadStockSerial { get; set; }
        public virtual DbSet<Stock_CurrentStock> Stock_CurrentStock { get; set; }
        public virtual DbSet<Stock_CurrentStockSerial> Stock_CurrentStockSerial { get; set; }
        public virtual DbSet<Stock_LIMStock> Stock_LIMStock { get; set; }
        public virtual DbSet<Stock_RMAStock> Stock_RMAStock { get; set; }
        public virtual DbSet<Stock_RMAStockSerial> Stock_RMAStockSerial { get; set; }
        public virtual DbSet<Stock_TransitStock> Stock_TransitStock { get; set; }
        public virtual DbSet<Stock_TransitStockSerial> Stock_TransitStockSerial { get; set; }
        public virtual DbSet<Task_ChequeInfo> Task_ChequeInfo { get; set; }
        public virtual DbSet<Task_ChequeTreatment> Task_ChequeTreatment { get; set; }
        public virtual DbSet<Task_Collection> Task_Collection { get; set; }
        public virtual DbSet<Task_Collection_GovtDutyAdjustment> Task_Collection_GovtDutyAdjustment { get; set; }
        public virtual DbSet<Task_CollectionDetail> Task_CollectionDetail { get; set; }
        public virtual DbSet<Task_CollectionMapping> Task_CollectionMapping { get; set; }
        public virtual DbSet<Task_CollectionNos> Task_CollectionNos { get; set; }
        public virtual DbSet<Task_ComplainReceive> Task_ComplainReceive { get; set; }
        public virtual DbSet<Task_ComplainReceive_Charge> Task_ComplainReceive_Charge { get; set; }
        public virtual DbSet<Task_ComplainReceiveDetail> Task_ComplainReceiveDetail { get; set; }
        public virtual DbSet<Task_ComplainReceiveDetail_Problem> Task_ComplainReceiveDetail_Problem { get; set; }
        public virtual DbSet<Task_ComplainReceiveDetail_SpareProduct> Task_ComplainReceiveDetail_SpareProduct { get; set; }
        public virtual DbSet<Task_ComplainReceiveNos> Task_ComplainReceiveNos { get; set; }
        public virtual DbSet<Task_Convertion> Task_Convertion { get; set; }
        public virtual DbSet<Task_ConvertionDetail> Task_ConvertionDetail { get; set; }
        public virtual DbSet<Task_ConvertionDetailSerial> Task_ConvertionDetailSerial { get; set; }
        public virtual DbSet<Task_ConvertionNos> Task_ConvertionNos { get; set; }
        public virtual DbSet<Task_CustomerDelivery> Task_CustomerDelivery { get; set; }
        public virtual DbSet<Task_CustomerDelivery_Charge> Task_CustomerDelivery_Charge { get; set; }
        public virtual DbSet<Task_CustomerDeliveryDetail> Task_CustomerDeliveryDetail { get; set; }
        public virtual DbSet<Task_CustomerDeliveryDetail_Problem> Task_CustomerDeliveryDetail_Problem { get; set; }
        public virtual DbSet<Task_CustomerDeliveryDetail_SpareProduct> Task_CustomerDeliveryDetail_SpareProduct { get; set; }
        public virtual DbSet<Task_CustomerDeliveryDetail_SpareSerial> Task_CustomerDeliveryDetail_SpareSerial { get; set; }
        public virtual DbSet<Task_CustomerDeliveryNos> Task_CustomerDeliveryNos { get; set; }
        public virtual DbSet<Task_DeliveryChallan> Task_DeliveryChallan { get; set; }
        public virtual DbSet<Task_DeliveryChallan_Charge> Task_DeliveryChallan_Charge { get; set; }
        public virtual DbSet<Task_DeliveryChallanDetail> Task_DeliveryChallanDetail { get; set; }
        public virtual DbSet<Task_DeliveryChallanDetail_GovtDuty> Task_DeliveryChallanDetail_GovtDuty { get; set; }
        public virtual DbSet<Task_DeliveryChallanDetailSerial> Task_DeliveryChallanDetailSerial { get; set; }
        public virtual DbSet<Task_DeliveryChallanNos> Task_DeliveryChallanNos { get; set; }
        public virtual DbSet<Task_GoodsReceive> Task_GoodsReceive { get; set; }
        public virtual DbSet<Task_GoodsReceiveDetail> Task_GoodsReceiveDetail { get; set; }
        public virtual DbSet<Task_GoodsReceiveDetailSerial> Task_GoodsReceiveDetailSerial { get; set; }
        public virtual DbSet<Task_GoodsReceiveNos> Task_GoodsReceiveNos { get; set; }
        public virtual DbSet<Task_ImportCosting> Task_ImportCosting { get; set; }
        public virtual DbSet<Task_ImportCostingDetail> Task_ImportCostingDetail { get; set; }
        public virtual DbSet<Task_ImportCostingNos> Task_ImportCostingNos { get; set; }
        public virtual DbSet<Task_ImportCostingProduct> Task_ImportCostingProduct { get; set; }
        public virtual DbSet<Task_ImportDocuments> Task_ImportDocuments { get; set; }
        public virtual DbSet<Task_ImportedStockIn> Task_ImportedStockIn { get; set; }
        public virtual DbSet<Task_ImportedStockInDetail> Task_ImportedStockInDetail { get; set; }
        public virtual DbSet<Task_ImportedStockInDetailSerial> Task_ImportedStockInDetailSerial { get; set; }
        public virtual DbSet<Task_ImportedStockInNos> Task_ImportedStockInNos { get; set; }
        public virtual DbSet<Task_ItemRequisition> Task_ItemRequisition { get; set; }
        public virtual DbSet<Task_ItemRequisitionDetail> Task_ItemRequisitionDetail { get; set; }
        public virtual DbSet<Task_ItemRequisitionNos> Task_ItemRequisitionNos { get; set; }
        public virtual DbSet<Task_LCOpening> Task_LCOpening { get; set; }
        public virtual DbSet<Task_LCOpeningNos> Task_LCOpeningNos { get; set; }
        public virtual DbSet<Task_LIMStockIn> Task_LIMStockIn { get; set; }
        public virtual DbSet<Task_LIMStockInDetail> Task_LIMStockInDetail { get; set; }
        public virtual DbSet<Task_LIMStockInNos> Task_LIMStockInNos { get; set; }
        public virtual DbSet<Task_PartyAdjustment> Task_PartyAdjustment { get; set; }
        public virtual DbSet<Task_PartyAdjustmentDetail> Task_PartyAdjustmentDetail { get; set; }
        public virtual DbSet<Task_PartyAdjustmentNos> Task_PartyAdjustmentNos { get; set; }
        public virtual DbSet<Task_Payment> Task_Payment { get; set; }
        public virtual DbSet<Task_Payment_GovtDutyAdjustment> Task_Payment_GovtDutyAdjustment { get; set; }
        public virtual DbSet<Task_PaymentDetail> Task_PaymentDetail { get; set; }
        public virtual DbSet<Task_PaymentMapping> Task_PaymentMapping { get; set; }
        public virtual DbSet<Task_PaymentNos> Task_PaymentNos { get; set; }
        public virtual DbSet<Task_PostedVoucher> Task_PostedVoucher { get; set; }
        public virtual DbSet<Task_ProformaInvoice> Task_ProformaInvoice { get; set; }
        public virtual DbSet<Task_ProformaInvoiceDetail> Task_ProformaInvoiceDetail { get; set; }
        public virtual DbSet<Task_ProformaInvoiceNos> Task_ProformaInvoiceNos { get; set; }
        public virtual DbSet<Task_PurchaseOrder> Task_PurchaseOrder { get; set; }
        public virtual DbSet<Task_PurchaseOrderDetail> Task_PurchaseOrderDetail { get; set; }
        public virtual DbSet<Task_PurchaseOrderNos> Task_PurchaseOrderNos { get; set; }
        public virtual DbSet<Task_PurchaseReturn> Task_PurchaseReturn { get; set; }
        public virtual DbSet<Task_PurchaseReturnDetail> Task_PurchaseReturnDetail { get; set; }
        public virtual DbSet<Task_PurchaseReturnDetailSerial> Task_PurchaseReturnDetailSerial { get; set; }
        public virtual DbSet<Task_PurchaseReturnNos> Task_PurchaseReturnNos { get; set; }
        public virtual DbSet<Task_ReceiveFinalize> Task_ReceiveFinalize { get; set; }
        public virtual DbSet<Task_ReceiveFinalizeDetail> Task_ReceiveFinalizeDetail { get; set; }
        public virtual DbSet<Task_ReceiveFinalizeDetailSerial> Task_ReceiveFinalizeDetailSerial { get; set; }
        public virtual DbSet<Task_ReceiveFinalizeNos> Task_ReceiveFinalizeNos { get; set; }
        public virtual DbSet<Task_ReplacementClaim> Task_ReplacementClaim { get; set; }
        public virtual DbSet<Task_ReplacementClaimDetail> Task_ReplacementClaimDetail { get; set; }
        public virtual DbSet<Task_ReplacementClaimDetail_Problem> Task_ReplacementClaimDetail_Problem { get; set; }
        public virtual DbSet<Task_ReplacementClaimNos> Task_ReplacementClaimNos { get; set; }
        public virtual DbSet<Task_ReplacementReceive> Task_ReplacementReceive { get; set; }
        public virtual DbSet<Task_ReplacementReceive_Charge> Task_ReplacementReceive_Charge { get; set; }
        public virtual DbSet<Task_ReplacementReceiveDetail> Task_ReplacementReceiveDetail { get; set; }
        public virtual DbSet<Task_ReplacementReceiveNos> Task_ReplacementReceiveNos { get; set; }
        public virtual DbSet<Task_RequisitionFinalize> Task_RequisitionFinalize { get; set; }
        public virtual DbSet<Task_RequisitionFinalizeDetail> Task_RequisitionFinalizeDetail { get; set; }
        public virtual DbSet<Task_RequisitionFinalizeNos> Task_RequisitionFinalizeNos { get; set; }
        public virtual DbSet<Task_SalesInvoice> Task_SalesInvoice { get; set; }
        public virtual DbSet<Task_SalesInvoice_Charge> Task_SalesInvoice_Charge { get; set; }
        public virtual DbSet<Task_SalesInvoiceDetail> Task_SalesInvoiceDetail { get; set; }
        public virtual DbSet<Task_SalesInvoiceDetail_GovtDuty> Task_SalesInvoiceDetail_GovtDuty { get; set; }
        public virtual DbSet<Task_SalesInvoiceDetailSerial> Task_SalesInvoiceDetailSerial { get; set; }
        public virtual DbSet<Task_SalesInvoiceNos> Task_SalesInvoiceNos { get; set; }
        public virtual DbSet<Task_SalesOrder> Task_SalesOrder { get; set; }
        public virtual DbSet<Task_SalesOrder_Charge> Task_SalesOrder_Charge { get; set; }
        public virtual DbSet<Task_SalesOrderDeliveryInfo> Task_SalesOrderDeliveryInfo { get; set; }
        public virtual DbSet<Task_SalesOrderDetail> Task_SalesOrderDetail { get; set; }
        public virtual DbSet<Task_SalesOrderDetail_GovtDuty> Task_SalesOrderDetail_GovtDuty { get; set; }
        public virtual DbSet<Task_SalesOrderNos> Task_SalesOrderNos { get; set; }
        public virtual DbSet<Task_SalesQuotation> Task_SalesQuotation { get; set; }
        public virtual DbSet<Task_SalesQuotation_Charge> Task_SalesQuotation_Charge { get; set; }
        public virtual DbSet<Task_SalesQuotation_DeliveryInfo> Task_SalesQuotation_DeliveryInfo { get; set; }
        public virtual DbSet<Task_SalesQuotation_GovtDutyAdjustment> Task_SalesQuotation_GovtDutyAdjustment { get; set; }
        public virtual DbSet<Task_SalesQuotation_Header> Task_SalesQuotation_Header { get; set; }
        public virtual DbSet<Task_SalesQuotation_SecurityAndBanking> Task_SalesQuotation_SecurityAndBanking { get; set; }
        public virtual DbSet<Task_SalesQuotationDetail> Task_SalesQuotationDetail { get; set; }
        public virtual DbSet<Task_SalesQuotationDetail_GovtDuty> Task_SalesQuotationDetail_GovtDuty { get; set; }
        public virtual DbSet<Task_SalesQuotationNos> Task_SalesQuotationNos { get; set; }
        public virtual DbSet<Task_SalesReturn> Task_SalesReturn { get; set; }
        public virtual DbSet<Task_SalesReturnDetail> Task_SalesReturnDetail { get; set; }
        public virtual DbSet<Task_SalesReturnDetailSerial> Task_SalesReturnDetailSerial { get; set; }
        public virtual DbSet<Task_SalesReturnNos> Task_SalesReturnNos { get; set; }
        public virtual DbSet<Task_StockAdjustment> Task_StockAdjustment { get; set; }
        public virtual DbSet<Task_StockAdjustmentDetail> Task_StockAdjustmentDetail { get; set; }
        public virtual DbSet<Task_StockAdjustmentDetailSerial> Task_StockAdjustmentDetailSerial { get; set; }
        public virtual DbSet<Task_StockAdjustmentNos> Task_StockAdjustmentNos { get; set; }
        public virtual DbSet<Task_TransferChallan> Task_TransferChallan { get; set; }
        public virtual DbSet<Task_TransferChallanDetail> Task_TransferChallanDetail { get; set; }
        public virtual DbSet<Task_TransferChallanDetailSerial> Task_TransferChallanDetailSerial { get; set; }
        public virtual DbSet<Task_TransferChallanNos> Task_TransferChallanNos { get; set; }
        public virtual DbSet<Task_TransferOrder> Task_TransferOrder { get; set; }
        public virtual DbSet<Task_TransferOrderDetail> Task_TransferOrderDetail { get; set; }
        public virtual DbSet<Task_TransferOrderNos> Task_TransferOrderNos { get; set; }
        public virtual DbSet<Task_TransferReceive> Task_TransferReceive { get; set; }
        public virtual DbSet<Task_TransferReceiveDetail> Task_TransferReceiveDetail { get; set; }
        public virtual DbSet<Task_TransferReceiveDetailSerial> Task_TransferReceiveDetailSerial { get; set; }
        public virtual DbSet<Task_TransferReceiveNos> Task_TransferReceiveNos { get; set; }
        public virtual DbSet<Task_TransferRequisitionFinalize> Task_TransferRequisitionFinalize { get; set; }
        public virtual DbSet<Task_TransferRequisitionFinalizeDetail> Task_TransferRequisitionFinalizeDetail { get; set; }
        public virtual DbSet<Task_TransferRequisitionFinalizeNos> Task_TransferRequisitionFinalizeNos { get; set; }
        public virtual DbSet<Task_Voucher> Task_Voucher { get; set; }
        public virtual DbSet<Task_VoucherDetail> Task_VoucherDetail { get; set; }
        public virtual DbSet<Task_VoucherNos> Task_VoucherNos { get; set; }
        public virtual DbSet<Temp_AccountsLedgerDetail> Temp_AccountsLedgerDetail { get; set; }
        public virtual DbSet<Temp_AccountsLedgerOrProvisionalLedger> Temp_AccountsLedgerOrProvisionalLedger { get; set; }
        public virtual DbSet<Temp_ChequeInfo> Temp_ChequeInfo { get; set; }
        public virtual DbSet<Temp_Collection> Temp_Collection { get; set; }
        public virtual DbSet<Temp_CollectionDetail> Temp_CollectionDetail { get; set; }
        public virtual DbSet<Temp_CollectionMapping> Temp_CollectionMapping { get; set; }
        public virtual DbSet<Temp_CustomerSupplierOutstanding> Temp_CustomerSupplierOutstanding { get; set; }
        public virtual DbSet<Temp_PartyLedger> Temp_PartyLedger { get; set; }
        public virtual DbSet<Temp_SalesInvoice> Temp_SalesInvoice { get; set; }
        public virtual DbSet<Temp_SalesInvoice_Charge> Temp_SalesInvoice_Charge { get; set; }
        public virtual DbSet<Temp_SalesInvoiceDetail> Temp_SalesInvoiceDetail { get; set; }
        public virtual DbSet<Temp_SalesInvoiceDetail_GovtDuty> Temp_SalesInvoiceDetail_GovtDuty { get; set; }
        public virtual DbSet<Temp_SalesInvoiceDetailSerial> Temp_SalesInvoiceDetailSerial { get; set; }
        public virtual DbSet<Temp_TrialBalance> Temp_TrialBalance { get; set; }
        public virtual DbSet<TempCustomerOrSupplierWiseChequePerformance> TempCustomerOrSupplierWiseChequePerformances { get; set; }
    }
}
