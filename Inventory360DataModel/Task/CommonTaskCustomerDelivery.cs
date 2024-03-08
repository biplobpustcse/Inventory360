using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCustomerDelivery
    {
        public Guid DeliveryId { get; set; }
        public string DeliveryNo { get; set; }
        public string DeliveryDate { get; set; }
        public decimal ExchangeRate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public long RequestedBy { get; set; }
        public long CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmount1 { get; set; }
        public decimal TotalAmount2 { get; set; }
        public decimal TotalChargeAmount { get; set; }
        public decimal TotalChargeAmount1 { get; set; }
        public decimal TotalChargeAmount2 { get; set; }
        public decimal Discount { get; set; }
        public decimal Discount1 { get; set; }
        public decimal Discount2 { get; set; }
        public string Remarks { get; set; }
        public string Approved { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string CancelReason { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskCustomerDelivery_Charge> CustomerDeliveryCharge { get; set; }
        public List<CommonTaskCustomerDeliveryDetail> CustomerDeliveryDetail { get; set; }
    }
}