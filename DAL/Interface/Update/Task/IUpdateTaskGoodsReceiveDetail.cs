using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskGoodsReceiveDetail
    {
        bool UpdateGoodsReceiveDetailForReceiveFinalizeQuantityIncrease(Guid receiveId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateGoodsReceiveDetailForReceiveFinalizeQuantityDecrease(Guid receiveId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}