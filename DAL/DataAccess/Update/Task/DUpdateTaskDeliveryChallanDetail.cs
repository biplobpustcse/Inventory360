using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskDeliveryChallanDetail : IUpdateTaskDeliveryChallanDetail
    {
        private Inventory360Entities _db;

        public DUpdateTaskDeliveryChallanDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateDeliveryChallanDetailForInvoicedQuantityIncrease(Guid challanId, long? warehouseId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_DeliveryChallanDetail _findEntity = _db.Task_DeliveryChallanDetail
                    .Where(x => x.ChallanId == challanId
                        && x.WareHouseId == (warehouseId == 0 ? null : warehouseId)
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.InvoicedQuantity = _findEntity.InvoicedQuantity + quantity;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateDeliveryChallanDetailForInvoicedQuantityDecrease(Guid challanId, long? warehouseId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_DeliveryChallanDetail _findEntity = _db.Task_DeliveryChallanDetail
                    .Where(x => x.ChallanId == challanId
                        && x.WareHouseId == (warehouseId == 0 ? null : warehouseId)
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.InvoicedQuantity = _findEntity.InvoicedQuantity - quantity;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}