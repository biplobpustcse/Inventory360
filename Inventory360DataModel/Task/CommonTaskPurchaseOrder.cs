using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskPurchaseOrder
    {
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string PurchaseType { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public long SupplierId { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public long PaymentModeId { get; set; }
        public string Remarks { get; set; }
        public long? TermsAndConditionsId { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public long? PaymentTermsId { get; set; }
        public string PaymentTermsDetail { get; set; }
        public string ShipmentType { get; set; }
        public string DeliveryTo { get; set; }
        public string DeliveryContactNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskPurchaseOrderDetail> PurchaseOrderDetailLists { get; set; }
    }
}