using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskSalesOrder : CommonHeader
    {
        public string SalesOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerName { get; set; }
        public string SPCode { get; set; }
        public string SPMobile { get; set; }
        public string SPName { get; set; }
        public string SalesType { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string OperationType { get; set; }
        public string TermsAndConditions { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public string Remarks { get; set; }
        public string ShipmentType { get; set; }
        public DateTime? ApxShipmentDate { get; set; }
        public string ShipmentMode { get; set; }
        public string DeliveryFrom { get; set; }
        public string WareHouse { get; set; }
        public string PaymentMode { get; set; }
        public DateTime? PromisedDate { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermsDetail { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal GrandTotal { get; set; }
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public CommonTaskSalesOrderDeliveryInfo DeliveryInfo { get; set; }
        public List<CommonTaskSalesOrderDetail> SalesOrderDetailLists { get; set; }
    }

    public class CommonTaskSalesOrderDeliveryInfo
    {
        public string DeliveryPlace { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public string TransportName { get; set; }
        public string TransportType { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
    }

    public class CommonTaskSalesOrderDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
    }
}