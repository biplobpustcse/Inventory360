using Inventory360DataModel;
using Inventory360DataModel.Task;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskSalesOrder
    {
        bool UpdateSalesOrder(CommonTaskSalesOrder entity, CurrencyConvertedAmount orderAmount, CurrencyConvertedAmount discountAmount);
        bool UpdateSalesOrderForApprove(long approvedBy);
        bool UpdateSalesOrderForCancel(string reason, long cancelledBy);
        bool UpdateSalesOrderForIsSettled(bool value);
        bool UpdateSalesOrderByCollectionInCollectedAmountIncrease(CurrencyConvertedAmount convertedAmount);
        bool UpdateSalesOrderByCollectionInCollectedAmountDecrease(CurrencyConvertedAmount convertedAmount);
    }
}