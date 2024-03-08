using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskSalesOrderDetail : IUpdateTaskSalesOrderDetail
    {
        private Inventory360Entities _db;

        public DUpdateTaskSalesOrderDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSalesOrderDetailForDeliveredQuantityIncrease(Guid salesOrderId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_SalesOrderDetail _findEntity = _db.Task_SalesOrderDetail
                    .Where(x => x.SalesOrderId == salesOrderId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.DeliveredQuantity = _findEntity.DeliveredQuantity + quantity;

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
        public bool UpdateSalesOrderDetailForDeliveredQuantityDecrease(Guid salesOrderId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_SalesOrderDetail _findEntity = _db.Task_SalesOrderDetail
                    .Where(x => x.SalesOrderId == salesOrderId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.DeliveredQuantity = _findEntity.DeliveredQuantity - quantity;

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