using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskPaymentDetail
    {
        bool UpdatePaymentDetailForVoucherId(Guid voucherId);
    }
}