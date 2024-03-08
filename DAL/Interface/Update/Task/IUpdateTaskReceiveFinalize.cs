using Inventory360DataModel;
using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskReceiveFinalize
    {
        bool UpdateReceiveFinalizeForApprove(long approvedBy);
        bool UpdateReceiveFinalizeForCancel(string reason, long cancelledBy);
        bool UpdateReceiveFinalizeForIsSettled(bool value);
        bool UpdateReceiveFinalizeForVoucherId(Guid voucherId);
        bool UpdateReceiveFinalizeByAmount(decimal finalizeAmount, decimal finalize1Amount, decimal finalize2Amount);
        bool UpdateReceiveFinalizeByPaidAmountIncrease(CurrencyConvertedAmount convertedAmount);
        bool UpdateReceiveFinalizeByPaidAmountDecrease(CurrencyConvertedAmount convertedAmount);
    }
}