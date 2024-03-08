using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskTransferRequisitionFinalizeDetail
    {
        bool UpdateTransferRequisitionDetailForOrderedQuantityIncrease(Guid requisitionDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateTransferRequisitionDetailForOrderedQuantityDecrease(Guid requisitionDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}