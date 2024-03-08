using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskDeliveryChallanDetail
    {
        bool UpdateDeliveryChallanDetailForInvoicedQuantityIncrease(Guid challanId, long? warehouseId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateDeliveryChallanDetailForInvoicedQuantityDecrease(Guid challanId, long? warehouseId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}