using System;

namespace DAL.Interface.Update.Stock
{
    public interface IUpdateStockItemTransferDetail
    {
        bool UpdateItemRequisitionDetailForFinalizedQuantityIncrease(Guid itemRequisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateItemRequisitionDetailForFinalizedQuantityDecrease(Guid itemRequisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}