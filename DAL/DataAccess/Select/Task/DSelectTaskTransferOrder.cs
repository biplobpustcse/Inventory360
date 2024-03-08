using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskTransferOrder : ISelectTaskTransferOrder
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskTransferOrder(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferOrder> SelectTaskTransferOrderAll()
        {
            return _db.Task_TransferOrder.Where(x => x.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferOrder> SelectTaskTransferOrder(Guid Id)
        {
            return _db.Task_TransferOrder.Where(x => x.CompanyId == _companyId).WhereIf(!String.IsNullOrEmpty(Id.ToString()),x=>x.OrderId==Id);
        }
        public List<CommonProductWarehouseByLocation> SelectProductWarehouseByLocation(long companyId, long locationId, long productid, string orderId)
        {
            List<CommonProductWarehouseByLocation> list = new List<CommonProductWarehouseByLocation>();
            Guid orderid = new Guid(orderId);

            var locWiseStock =(from location in _db.Setup_Location
                               join stock in _db.Stock_CurrentStock on location.LocationId equals stock.LocationId
                               join transferDetail in _db.Task_TransferOrderDetail on stock.ProductId equals transferDetail.ProductId
                               join product in _db.Setup_Product on stock.ProductId equals product.ProductId
                               where transferDetail.ProductDimensionId == stock.ProductDimensionId
                               && stock.UnitTypeId == transferDetail.UnitTypeId                                
                               group new { stockQuantity=stock.Quantity} by new
                               {
                                   location.LocationId,
                                   location.Name,
                               } into item
                               select new
                               {
                                   location = item.Key.LocationId,
                                   locationName = item.Key.Name,
                                   stockQuantity = item.Sum(x => x.stockQuantity)
                               }).ToList();

            foreach(var vm in locWiseStock)
            {
                CommonProductWarehouseByLocation itme = new CommonProductWarehouseByLocation
                {
                    IsSelected=false,
                    WareHouseId=vm.location,
                    WarehouseName=vm.locationName,
                    StockQuantity=vm.stockQuantity,
                    DeliveryQuantity=0                    
                };
                list.Add(itme);
            }

            return list;
        }
        public object SelectProductDetailInfo(long companyId, long locationId, long productid, string orderId)
        {
            try
            {
                Guid ordId = new Guid(orderId);
                var obj = (from orderDetail in _db.Task_TransferOrderDetail.Where(x => x.OrderId == ordId && x.ProductId== productid)
                           join product in _db.Setup_Product on orderDetail.ProductId equals product.ProductId
                           join dimention in _db.Setup_ProductDimension on orderDetail.ProductDimensionId equals dimention.ProductDimensionId
                           join size in _db.Setup_Size on dimention.SizeId equals size.SizeId into prdSize from m in prdSize.DefaultIfEmpty()
                           join color in _db.Setup_Color on dimention.ColorId equals color.ColorId into prdColor from c in prdColor.DefaultIfEmpty()
                           join style in _db.Setup_Style on dimention.StyleId equals style.StyleId into prdStyle from s in prdStyle.DefaultIfEmpty()
                           select new
                           {
                               ProductId=product.ProductId,
                               ProductDimensionId=orderDetail.ProductDimensionId,
                               UnitTypeId=orderDetail.UnitTypeId,
                               ProductName=product.Name,
                               DimensionName=c.Name+"_"+m.Name+"_"+s.Name,
                               Quantity=orderDetail.Quantity,
                               RemainQuantity= orderDetail.Quantity-orderDetail.ChallanQuantity

                           }).ToList();
                return obj;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}