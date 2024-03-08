using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskGoodsReceive
    {
        bool UpdateGoodsReceiveForApprove(long approvedBy);
        bool UpdateGoodsReceiveForCancel(string reason, long cancelledBy);
        bool UpdateGoodsReceiveForIsSettled(bool value);
        bool UpdateGoodsReceiveForVoucherId(Guid voucherId);
    }
}