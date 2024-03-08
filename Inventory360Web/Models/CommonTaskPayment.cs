using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskPayment : CommonHeader
    {
        public string PaymentNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaidBy { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonTaskPaymentCollectionDetail> PaymentDetailLists { get; set; }
    }
}