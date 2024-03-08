using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupProduct : ISelectSetupProduct
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupProduct(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Product> SelectProductAll()
        {
            return _db.Setup_Product
                .Where(x => x.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_Product> SelectProductWithoutCheckingCompany()
        {
            return _db.Setup_Product;
        }
        public object SelectProductAvailableSerial(long companyId, long locationId, long productid, string orderId)
        {
           Guid orderGuid = new Guid(orderId);
            var serialList = (from stock in _db.Stock_CurrentStock//.Where(x => x.CompanyId == companyId && x.ProductId == productid)
                              join stockSerial in _db.Stock_CurrentStockSerial on stock.CurrentStockId equals stockSerial.CurrentStockId
                              join transferDetail in _db.Task_TransferOrderDetail//.Where(x => x.OrderId == orderGuid) 
                              on new { stock.ProductId, stock.ProductDimensionId, stock.UnitTypeId } equals new { transferDetail.ProductId, transferDetail.ProductDimensionId, transferDetail.UnitTypeId }
                              select new
                              {
                                  isSelected=false,
                                  SerailNo = stockSerial.Serial,
                                  StockId= stock.CurrentStockId ,
                                  StockDetailId= stockSerial.CurrentStockSerialId
                              }
                            );

            return serialList.ToList();
        }
        public object SelectProductWarehouseByLocationForSerialProduct(long companyId, long locationId, long productid, string orderId)
        {
            Guid orderGuid = new Guid(orderId);
            var warehouseList = (from location in _db.Setup_Location
                              join stock in _db.Stock_CurrentStock on location.LocationId equals stock.LocationId//.Where(x => x.CompanyId == companyId && x.ProductId == productid)
                              join stockSerial in _db.Stock_CurrentStockSerial on stock.CurrentStockId equals stockSerial.CurrentStockId
                              join transferDetail in _db.Task_TransferOrderDetail//.Where(x => x.OrderId == orderGuid) 
                              on new { stock.ProductId, stock.ProductDimensionId, stock.UnitTypeId } equals new { transferDetail.ProductId, transferDetail.ProductDimensionId, transferDetail.UnitTypeId }
                              //where(location.CompanyId==companyId && stock.ProductId==productid && transferDetail.OrderId==orderGuid)
                              select new
                              {
                                  isSelected = false,
                                  Name = location.Name,
                                  LocationId = location.LocationId
                              }
                            );

            return warehouseList.ToList();
        }
        public object SelectWarehouseBasedSerialNo(long companyId, long locationId, long productid,long warehouseId,string orderId)
        {
            Guid orderGuid = new Guid(orderId);
            var serialList = (from stock in _db.Stock_CurrentStock//.Where(x => x.CompanyId == companyId && x.ProductId == productid)
                              join stockSerial in _db.Stock_CurrentStockSerial on stock.CurrentStockId equals stockSerial.CurrentStockId
                              join transferDetail in _db.Task_TransferOrderDetail//.Where(x => x.OrderId == orderGuid) 
                              on new { stock.ProductId, stock.ProductDimensionId, stock.UnitTypeId } equals new { transferDetail.ProductId, transferDetail.ProductDimensionId, transferDetail.UnitTypeId }
                              //where(stock.CompanyId==companyId && stock.ProductId==productid  && stock.LocationId==warehouseId && transferDetail.OrderId==orderGuid)
                              select new
                              {
                                  isSelected = false,
                                  SerailNo = stockSerial.Serial,
                                  StockId = stock.CurrentStockId,
                                  StockDetailId = stockSerial.CurrentStockSerialId
                              }
                            );

            return serialList.ToList();
        }
        public object SelectProductStockInReferenceInfo(long companyId, long productId, string serial,long supplierId)
        {
            var data = (from RMAstock in _db.Stock_RMAStock.Where(x=>x.ProductId == productId && x.CompanyId == companyId)
                              join RMAstockSerial in _db.Stock_RMAStockSerial.Where(x => x.Serial == serial) on RMAstock.RMAStockId equals RMAstockSerial.RMAStockId
                              join GoodsReceiveDetail in _db.Task_GoodsReceiveDetail on RMAstock.ProductId equals GoodsReceiveDetail.ProductId                              
                              join ReceiveDetailSerial in _db.Task_GoodsReceiveDetailSerial on new { GoodsReceiveDetail.ReceiveDetailId, RMAstockSerial.Serial } equals new { ReceiveDetailSerial.ReceiveDetailId, ReceiveDetailSerial.Serial }
                              join GoodsReceive in _db.Task_GoodsReceive.WhereIf(supplierId !=0,x=>x.SupplierId == supplierId) on GoodsReceiveDetail.ReceiveId equals GoodsReceive.ReceiveId
                              join Supplier in _db.Setup_Supplier on GoodsReceive.SupplierId equals Supplier.SupplierId
                              select new
                              {
                                  isSelected = false,
                                  GoodsReceive.SupplierId,
                                  SupplierName = Supplier.Name,
                                  SupplierAddress = Supplier.Address,
                                  StockInBy = "Purchase",
                                  StockInRefNo = GoodsReceive.ReceiveNo,
                                  StockInRefDate = GoodsReceive.ReceiveDate,
                                  GoodsReceive.ReferenceNo,
                                  GoodsReceive.ReferenceDate
                              }
                            ).FirstOrDefault();
            if (data != null)
            {
                return data;
            }
            else
            {
                var data2 = (from RMAstock in _db.Stock_RMAStock.Where(x => x.ProductId == productId && x.CompanyId == companyId)
                             join RMAstockSerial in _db.Stock_RMAStockSerial.Where(x => x.Serial == serial) on RMAstock.RMAStockId equals RMAstockSerial.RMAStockId
                             join ImportedStockInDetail in _db.Task_ImportedStockInDetail on RMAstock.ProductId equals ImportedStockInDetail.ProductId
                             join ImportedStockInDetailSerial in _db.Task_ImportedStockInDetailSerial on new { ImportedStockInDetail.StockInDetailId, RMAstockSerial.Serial } equals new { ImportedStockInDetailSerial.StockInDetailId, ImportedStockInDetailSerial.Serial }
                             join ImportedStockIn in _db.Task_ImportedStockIn on ImportedStockInDetail.StockInId equals ImportedStockIn.StockInId
                             join LIMStockIn in _db.Task_LIMStockIn on ImportedStockIn.LIMStockInId equals LIMStockIn.StockInId
                             join ProformaInvoice in _db.Task_ProformaInvoice.WhereIf(supplierId != 0, x => x.SupplierId == supplierId) on LIMStockIn.ProformaInvoiceId equals ProformaInvoice.InvoiceId
                             join LC in _db.Task_LCOpening on LIMStockIn.ProformaInvoiceId equals LC.ProformaInvoiceId
                             join Supplier in _db.Setup_Supplier on ProformaInvoice.SupplierId equals Supplier.SupplierId
                             select new
                             {
                                 isSelected = false,
                                 ProformaInvoice.SupplierId,
                                 SupplierName = Supplier.Name,
                                 SupplierAddress = Supplier.Address,
                                 StockInBy = "Import",
                                 StockInRefNo = ImportedStockIn.StockInNo,
                                 StockInRefDate = ImportedStockIn.StockInDate,
                                 ReferenceNo = LC.LCNo,
                                 ReferenceDate = LC.LCDate
                             }
                                ).FirstOrDefault();
                return data2;
            }
        }

    }
}