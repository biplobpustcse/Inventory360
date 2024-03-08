using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskDirectSales
    {
        public string SelectedCurrency { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long CustomerId { get; set; }
        public long SalesPersonId { get; set; }
        public long? TermsAndConditionsId { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountTypeValue { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal Collection { get; set; }
        public string CommissionType { get; set; }
        public decimal CommissionValue { get; set; }
        public decimal CalculatedCommission { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskSalesInvoiceDetail> InvoiceDetailLists { get; set; }
        public List<CommonTaskCollectionDetail> CollectionInfoLists { get; set; }
    }
}