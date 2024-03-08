namespace DAL.Interface.Update.Stock
{
    public interface IUpdateStockAdjustment
    {
        bool UpdateStockAdjustmentForApprove(long approvedBy);
        bool UpdateStockAdjustmentForCancel(string reason, long cancelledBy);
    }
}