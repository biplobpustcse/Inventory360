using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskSalesOrder
    {
        public Guid SalesOrderId { get; set; }
        public string SalesOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public long CustomerId { get; set; }
        public long? BuyerId { get; set; }
        public long SalesPersonId { get; set; }
        public string SalesType { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public long OperationTypeId { get; set; }
        public long? TermsAndConditionsId { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public string Remarks { get; set; }
        public string ShipmentType { get; set; }
        public DateTime? ApxShipmentDate { get; set; }
        public string ShipmentMode { get; set; }
        public long DeliveryFromId { get; set; }
        public long? WareHouseId { get; set; }
        public long PaymentModeId { get; set; }
        public DateTime? PromisedDate { get; set; }
        public long? PaymentTermsId { get; set; }
        public string PaymentTermsDetail { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal OrderDiscount { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public string DeliveryPlace { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public long TransportId { get; set; }
        public long TransportTypeId { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
        public List<CommonTaskSalesOrderDetail> SalesOrderDetailLists { get; set; }
    }
}