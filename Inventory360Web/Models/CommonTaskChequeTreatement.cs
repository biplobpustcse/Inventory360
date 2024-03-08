using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskChequeTreatement : CommonHeader
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string ChequeType { get; set; }
        public string ReportName { get; set; }
        public List<statusList> statusList { get; set; }
        public List<object> CustomerOrSupplierNameList { get; set; }
        public List<CommonTaskChequeChequeInfo> ChequeInfoList { get; set; }
    }
    public class statusList
    {
        public string Status { get; set; }
        public string StatusName { get; set; }
    }
    public class ChequeTreatment
    {
        public Guid ChequeInfoId { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public DateTime StatusDate { get; set; }
        public string BankName { get; set; }
    }
    public class CommonTaskChequeChequeInfo
    {
        public Guid ChequeInfoId { get; set; }
        public string PreviousStatus { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public Guid? VoucherId { get; set; }
        public string VoucherNo { get; set; }
        public bool isSelected { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string BankName { get; set; }
        public decimal Amount { get; set; }
        public string SendBankName { get; set; }
        public long? SendBankId { get; set; }
        public string CustomerOrSupplierName { get; set; }
        public string CollectionOrPaymentNo { get; set; }
        public DateTime CollectionOrPaymentDate { get; set; }
        public long CompanyId { get; set; }
        public long LocationId { get; set; }   
        public string BankCompareWith { get; set; }
        public List<ChequeTreatment> ChequeTreatment { get; set; }
    }

    //public class CommonTaskPaymentCollectionDetail
    //{
    //    public string PaymentMode { get; set; }
    //    public decimal Amount { get; set; }
    //    public List<ChequeInfo> ChequeInfo { get; set; }
    //}

    //public class ChequeInfo
    //{
    //    public string BankName { get; set; }
    //    public string ChequeNo { get; set; }
    //    public DateTime ChequeDate { get; set; }
    //    public decimal ChequeAmount { get; set; }
    //}
}