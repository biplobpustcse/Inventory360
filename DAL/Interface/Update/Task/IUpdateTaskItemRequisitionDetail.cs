using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskItemRequisitionDetail
    {
        bool UpdateItemRequisitionDetailForFinalizedQuantityIncrease(Guid itemRequisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateItemRequisitionDetailForFinalizedQuantityDecrease(Guid itemRequisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}