using Inventory360DataModel;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskPurchaseOrder
    {
        bool UpdatePurchaseOrderForApprove(long approvedBy);
        bool UpdatePurchaseOrderForCancel(string reason, long cancelledBy);
        bool UpdatePurchaseOrderForIsSettled(bool value);
        bool UpdatePurchaseOrderByPaymentInPaidAmountIncrease(CurrencyConvertedAmount convertedAmount);
        bool UpdatePurchaseOrderByPaymentInPaidAmountDecrease(CurrencyConvertedAmount convertedAmount);
    }
}