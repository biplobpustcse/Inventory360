using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskPurchaseOrder : CommonHeader
    {
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string SuppliedBy { get; set; }
        public string Remarks { get; set; }
        public string PurchaseType { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string PaymentMode { get; set; }
        public string TermsAndConditions { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermsDetail { get; set; }
        public string ShipmentType { get; set; }
        public string DeliveryTo { get; set; }
        public string DeliveryContactNo { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonTaskPurchaseOrderDetail> OrderDetailLists { get; set; }
    }

    public class CommonTaskPurchaseOrderDetail
    {
        public string Identity { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public string RequisitionNo { get; set; }
    }
}