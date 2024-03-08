using System;

namespace DAL.Interface.Update.Stock
{
    public interface IUpdateStockCurrentStock
    {
        bool UpdateCurrentStockQuantityByIncrease(out Guid stockId, long companyId, long locationId, long productId, long unitTypeId, decimal quantity, long? productDimensionId, long? wareHouseId, decimal cost = 0, decimal cost1 = 0, decimal cost2 = 0, string referenceNo = "");
        bool UpdateCurrentStockQuantityByDecrease(long companyId, long locationId, long productId, long unitTypeId, decimal quantity, long? productDimensionId, long? wareHouseId, Guid goodsReceiveId, Guid importedStockInId, string referenceNo);
        bool UpdateCurrentStockQuantityByDecrease(Guid stockId, decimal quantity);
        bool UpdateCurrentStockQuantityByIncrease(Guid stockId, decimal quantity);
        bool UpdateCurrentStockCost(Guid stockId, decimal cost, decimal cost1, decimal cost2);
        bool UpdateCurrentStockReference(Guid stockId, Guid? GoodsReceiveId, Guid? ImportedStockInId, long? SupplierId, string ReferenceNo, DateTime ReferenceDate);
    }
}