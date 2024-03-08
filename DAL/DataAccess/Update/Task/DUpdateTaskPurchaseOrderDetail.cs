using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskPurchaseOrderDetail : IUpdateTaskPurchaseOrderDetail
    {
        private Inventory360Entities _db;

        public DUpdateTaskPurchaseOrderDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdatePurchaseOrderDetailForReceivedQuantityIncrease(Guid orderDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_PurchaseOrderDetail _findEntity = _db.Task_PurchaseOrderDetail
                    .Where(x => x.OrderDetailId == orderDetailId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.ReceivedQuantity = _findEntity.ReceivedQuantity + quantity;

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
        public bool UpdatePurchaseOrderDetailForReceivedQuantityDecrease(Guid orderDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_PurchaseOrderDetail _findEntity = _db.Task_PurchaseOrderDetail
                    .Where(x => x.OrderDetailId == orderDetailId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.ReceivedQuantity = _findEntity.ReceivedQuantity - quantity;

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