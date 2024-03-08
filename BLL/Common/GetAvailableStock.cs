using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Common
{
    public static class GetAvailableStock
    {
        public static decimal GetAvailableStockByProductExceptSelectedSalesOrder(long productId, long unitTypeId, long primaryUnitTypeId, long locationId, long? warehouseId, long companyId, long? productDimensionId, Guid? salesOrderId)
        {
            try
            {
                // Current Stock
                ISelectStockCurrentStock iSelectStockCurrentStock = new DSelectStockCurrentStock(companyId);
                decimal productWiseCurrentStock = iSelectStockCurrentStock.SelectCurrentStockAll()
                    .Where(x => x.ProductId == productId
                        && x.UnitTypeId == primaryUnitTypeId
                        && x.LocationId == locationId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.WareHouseId == (warehouseId == 0 ? null : warehouseId))
                    .Select(s => s.Quantity)
                    .DefaultIfEmpty(0)
                    .FirstOrDefault();

                // Approved Sales Order
                ISelectTaskSalesOrderDetail iSelectTaskSalesOrderDetail = new DSelectTaskSalesOrderDetail(companyId);
                decimal productWiseNonDeliveredQty = iSelectTaskSalesOrderDetail.SelectSalesOrderDetailAll()
                    .Where(x => x.ProductId == productId
                        && x.UnitTypeId == unitTypeId
                        && x.Task_SalesOrder.LocationId == locationId
                        && x.Task_SalesOrder.Approved.Equals("A")
                        && !x.Task_SalesOrder.IsSettledByChallan
                        && x.Task_SalesOrder.WareHouseId == (warehouseId == 0 ? null : warehouseId)
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId))
                    .WhereIf(salesOrderId != null, x=> x.SalesOrderId != salesOrderId)
                    .Select(s => s.Quantity - s.DeliveredQuantity)
                    .DefaultIfEmpty(0)
                    .Sum();

                return (productWiseCurrentStock - productWiseNonDeliveredQty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}