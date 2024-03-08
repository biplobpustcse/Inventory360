using Inventory360DataModel;
using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskSalesInvoice
    {
        bool UpdateSalesInvoiceByAmountAndDiscount(decimal invoiceAmount, decimal invoice1Amount, decimal invoice2Amount, decimal invoiceDiscount, decimal invoice1Discount, decimal invoice2Discount, decimal commission, decimal commission1, decimal commission2);
        bool UpdateSalesInvoiceByCollectedAmountIncrease(CurrencyConvertedAmount convertedAmount);
        bool UpdateSalesInvoiceByCollectedAmountDecrease(CurrencyConvertedAmount convertedAmount);
        bool UpdateSalesInvoiceForApprove(long approvedBy);
        bool UpdateSalesInvoiceForCancel(string reason, long cancelledBy);
        bool UpdateSalesInvoiceForVoucherId(Guid voucherId);
        bool UpdateSalesInvoiceForIsSettled(bool value);
    }
}