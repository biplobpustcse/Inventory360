using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskSalesInvoice : CommonHeader
    {
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal InvoiceDiscount { get; set; }
        public decimal GrandTotal { get; set; }
        public string AmountInWord { get; set; }
        public string CurrencyType { get; set; }
        public string TermsAndConditionsDetail { get; set; }
        public List<CommonTaskSalesInvoiceDetail> SalesInvoiceDetailLists { get; set; }
    }

    public class CommonTaskSalesInvoiceDetail
    {
        public string ChallanNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public List<List<CommonTaskProductSerial>> SerialLists { get; set; }
    }
}