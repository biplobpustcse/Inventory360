using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory360DataModel.Task
{
    public class CommonTaskChequeTreatmentDTO: CommonHeader
    {
        CommonList commonList = new CommonList();
        public Guid ChequeInfoId { get; set; }
        public string PreviousStatus { get; set; }
        public string Status { get; set; }
        public string StatusName { get { return commonList.SelectChequeStatus().Where(x => x.Value == Status).Select(s => s.Item).FirstOrDefault(); }}
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
        public string CustomerOrSupplierCode { get; set; }
        public string CustomerOrSupplierAddress { get; set; }
        public string CustomerOrSupplierPhoneNo { get; set; }
        public string CollectionOrPaymentNo { get; set; }       
        public long LocationId { get; set; }
        public DateTime CollectionOrPaymentDate { get; set; }
        public string BankCompareWith { get; set; }
        public List<ChequeTreatment> ChequeTreatment { get; set; }
    }
    public class ChequeTreatment
    {
        CommonList commonList = new CommonList();
        public Guid ChequeInfoId { get; set; }
        public string Status { get; set; }
        public string StatusName { get { return commonList.SelectChequeStatus().Where(x => x.Value == Status).Select(s => s.Item).FirstOrDefault(); } }
        public DateTime StatusDate { get; set; }
        public string BankName { get; set; }
    }
}