using System;

namespace DAL.Interface.Update.Task
{
    public interface IUpdateTaskSalesOrderDetail
    {
        bool UpdateSalesOrderDetailForDeliveredQuantityIncrease(Guid salesOrderId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
        bool UpdateSalesOrderDetailForDeliveredQuantityDecrease(Guid salesOrderId, long productId, long unitTypeId, decimal quantity, long? productDimensionId);
    }
}