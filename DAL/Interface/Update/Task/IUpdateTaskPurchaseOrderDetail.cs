using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskPurchaseOrderDetail
    {
        bool UpdatePurchaseOrderDetailForReceivedQuantityIncrease(Guid orderDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdatePurchaseOrderDetailForReceivedQuantityDecrease(Guid orderDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}