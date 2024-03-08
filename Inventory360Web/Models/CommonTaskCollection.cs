using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskCollection : CommonHeader
    {
        public string CollectionNo { get; set; }
        public DateTime CollectionDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public decimal CollectedAmount { get; set; }
        public string CollectedBy { get; set; }
        public string MRNo { get; set; }
        public string Remarks { get; set; }
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public List<CommonTaskPaymentCollectionDetail> CollectionDetailLists { get; set; }
    }

    public class CommonTaskPaymentCollectionDetail
    {
        public string PaymentMode { get; set; }
        public decimal Amount { get; set; }
        public List<ChequeInfo> ChequeInfo { get; set; }
    }

    public class ChequeInfo
    {
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal ChequeAmount { get; set; }
    }
}