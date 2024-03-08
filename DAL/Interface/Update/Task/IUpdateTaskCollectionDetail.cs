using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskCollectionDetail
    {
        bool UpdateCollectionDetailForVoucherId(Guid voucherId);
    }
}