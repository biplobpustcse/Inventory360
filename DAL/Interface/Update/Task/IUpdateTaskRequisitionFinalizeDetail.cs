using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskRequisitionFinalizeDetail
    {
        bool UpdateRequisitionDetailForOrderedQuantityIncrease(Guid requisitionDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateRequisitionDetailForOrderedQuantityDecrease(Guid requisitionDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}