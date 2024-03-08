using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskSalesInvoice
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long CustomerId { get; set; }
        public long? BuyerId { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public string InvoiceType { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string InvoiceDiscountType { get; set; }
        public decimal InvoiceDiscountValue { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public long? TermsAndConditionsId { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public string CommissionType { get; set; }
        public decimal CommissionValue { get; set; }
        public decimal CommissionAmount { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskSalesInvoiceDetail> InvoiceDetailLists { get; set; }
    }
}