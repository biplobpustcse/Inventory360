using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonCustomerDelivery : CommonHeader
    {
        public string DeliveryNo { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }        
        public string RequestedBy { get; set; }      
        public decimal TotalChargeAmount { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public decimal Discount { get; set; }        
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonCustomerDeliveryDetail> CustomerDeliveryDetail { get; set; }
        public List<CommonComplainReceive_Charge> CustomerDelivery_Charge { get; set; }
        
    }

    public class CommonCustomerDeliveryDetail
    {
        public string NewProductName { get; set; }
        public decimal TotalSpareAmount { get; set; }
        public decimal TotalServiceAmount { get; set; }
        public decimal TotalSpareDiscount { get; set; }
        public decimal TotalServiceDiscount { get; set; }
        public decimal AdjustedAmount { get; set; }        
        public string NewSerial { get; set; }
        public string Remarks { get; set; }
        public List<CommonCustomerDeliveryDetail_Problem> CustomerDeliveryDetail_Problem { get; set; }
        public List<CommonCustomerDeliveryDetail_SpareProduct> CustomerDeliveryDetail_SpareProduct { get; set; }
    }
    public class CommonCustomerDelivery_Charge
    {
        public System.Guid ReceiveId { get; set; }
        public string ChargeName { get; set; }
        public decimal ChargeAmount { get; set; }
    }
    public class CommonCustomerDeliveryDetail_Problem
    {
        public System.Guid ReceiveDetailId { get; set; }
        public string ProblemName { get; set; }
        public string ProblemNote { get; set; }
    }
    public class CommonCustomerDeliveryDetail_SpareProduct
    {
        public System.Guid ReceiveDetailId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }        
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }        
    }
}