using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Stock
{
    public  class GridCurrentStock
    {
        public object SelectAllSerialByProductForGrid(long productId, long? dimensionId, long unitTypeId, long locationId, long? warehouseId, long companyId, string query)
        {
            try
            {
                // Purchase Return
                ISelectTaskPurchaseReturnDetailSerial iSelectTaskPurchaseReturnDetailSerial = new DSelectTaskPurchaseReturnDetailSerial(companyId);
                var purchaseReturnSerialLists = iSelectTaskPurchaseReturnDetailSerial.SelectPurchaseReturnDetailSerialAll()
                    .Where(x => x.Task_PurchaseReturnDetail.Task_PurchaseReturn.Approved.Equals("N")
                        && x.Task_PurchaseReturnDetail.ProductId == productId
                        && x.Task_PurchaseReturnDetail.ProductDimensionId == (dimensionId == 0 ? null : dimensionId)
                        && x.Task_PurchaseReturnDetail.UnitTypeId == unitTypeId
                        && x.Task_PurchaseReturnDetail.Task_PurchaseReturn.LocationId == locationId
                        && x.Task_PurchaseReturnDetail.WarehouseId == (warehouseId == 0 ? null : warehouseId))
                    .Select(s => s.Serial)
                    .ToList();

                // Transfer Challan
                ISelectTaskTransferChallanDetailSerial iSelectTaskTransferChallanDetailSerial = new DSelectTaskTransferChallanDetailSerial(companyId);
                var transferChallanSerialLists = iSelectTaskTransferChallanDetailSerial.SelectTransferChallanDetailSerialAll()
                    .Where(x => x.Task_TransferChallanDetail.Task_TransferChallan.Approved.Equals("N")
                        && x.Task_TransferChallanDetail.Task_TransferChallan.TransferFromStockType.Equals(CommonEnum.ProductStockType.Current.ToString())
                        && x.Task_TransferChallanDetail.ProductId == productId
                        && x.Task_TransferChallanDetail.ProductDimensionId == (dimensionId == 0 ? null : dimensionId)
                        && x.Task_TransferChallanDetail.UnitTypeId == unitTypeId
                        && x.Task_TransferChallanDetail.Task_TransferChallan.LocationId == locationId
                        && x.Task_TransferChallanDetail.WareHouseId == (warehouseId == 0 ? null : warehouseId))
                    .Select(s => s.Serial)
                    .ToList();

                ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);
                return iSelectStockCurrentStockSerial.SelectCurrentStockSerialAll()
                    .Where(x => x.Stock_CurrentStock.ProductId == productId
                        && x.Stock_CurrentStock.ProductDimensionId == (dimensionId == 0 ? null : dimensionId)
                        && x.Stock_CurrentStock.UnitTypeId == unitTypeId
                        && x.Stock_CurrentStock.LocationId == locationId
                        && x.Stock_CurrentStock.WareHouseId == (warehouseId == 0 ? null : warehouseId))
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Serial.ToLower().Contains(query.ToLower()))
                    .WhereIf(purchaseReturnSerialLists.Count > 0, x => !purchaseReturnSerialLists.Contains(x.Serial))
                    .WhereIf(transferChallanSerialLists.Count > 0, x => !transferChallanSerialLists.Contains(x.Serial))
                    .OrderBy(o => o.Serial)
                    .Select(s => new
                    {
                        IsSelected = false,
                        s.Serial,
                        s.AdditionalSerial
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
