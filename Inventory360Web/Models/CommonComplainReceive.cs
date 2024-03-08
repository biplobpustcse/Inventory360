using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonComplainReceive : CommonHeader
    {
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }        
        public string RequestedBy { get; set; }      
        public decimal TotalChargeAmount { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonComplainReceiveDetail> ComplainReceiveDetail { get; set; }
        public List<CommonComplainReceive_Charge> ComplainReceive_Charge { get; set; }
        
    }

    public class CommonComplainReceiveDetail
    {
        public string ProductName { get; set; }
        public decimal TotalSpareAmount { get; set; }
        public string Serial { get; set; }
        public string Remarks { get; set; }
        public List<CommonComplainComplainReceiveDetail_Problem> ComplainReceiveDetail_Problem { get; set; }
        public List<CommonComplainReceiveDetail_SpareProduct> ComplainReceiveDetail_SpareProduct { get; set; }
    }

    public class CommonComplainReceive_Charge
    {
        public System.Guid ReceiveId { get; set; }
        public string ChargeName { get; set; }
        public decimal ChargeAmount { get; set; }
    }

    public class CommonComplainComplainReceiveDetail_Problem
    {
        public System.Guid ReceiveDetailId { get; set; }
        public string ProblemName { get; set; }
        public string ProblemNote { get; set; }
    }

    public class CommonComplainReceiveDetail_SpareProduct
    {
        public System.Guid ReceiveDetailId { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
    }
}