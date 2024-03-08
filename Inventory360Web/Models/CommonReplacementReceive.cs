using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonReplacementReceive : CommonHeader
    {
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string SupplierCode { get; set; }       
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierPhone { get; set; }
        public string RequestedBy { get; set; }      
        public decimal TotalChargeAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }  
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonReplacementReceiveDetail> ReplacementReceiveDetail { get; set; }
        public List<CommonReplacementReceive_Charge> ReplacementReceive_Charge { get; set; }
        
    }

    public class CommonReplacementReceiveDetail
    {
        public string PreviousProductName { get; set; }
        public string NewProductName { get; set; }
        public string PreviousSerial { get; set; }
        public string NewSerial { get; set; }
        public string AdjustmentType { get; set; }
        public decimal AdjustedAmount { get; set; }
        public string ClaimNo { get; set; }      
        public string Remarks { get; set; }
    }
    public class CommonReplacementReceive_Charge
    {
        public System.Guid ReceiveId { get; set; }
        public string ChargeName { get; set; }
        public decimal ChargeAmount { get; set; }
    }
}