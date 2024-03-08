using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskPayment
    {
        public Guid PaymentId { get; set; }
        public string PaymentNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal PaidAmount { get; set; }
        public long SupplierId { get; set; }
        public long PaidBy { get; set; }
        public string ReferenceNo { get; set; }
        public string Remarks { get; set; }
        public long OperationTypeId { get; set; }
        public long OperationalEventId { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskPaymentDetail> PaymentDetailLists { get; set; }
        public List<CommonTaskPaymentMapping> PaymentMappingLists { get; set; }
    }

    public class CommonTaskPaymentDetail
    {
        public Guid PaymentDetailId { get; set; }
        public Guid PaymentId { get; set; }
        public long PaymentModeId { get; set; }
        public decimal Amount { get; set; }
        public long BankId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
    }

    public class CommonTaskPaymentMapping
    {
        public Guid PaymentId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? FinalizeId { get; set; }
        public decimal Amount { get; set; }
    }
}