using Inventory360Entity;
using DAL.Interface.Update.Stock;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateStockItemTransferDetail : IUpdateStockItemTransferDetail
    {
        private Inventory360Entities _db;

        public DUpdateStockItemTransferDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateItemRequisitionDetailForFinalizedQuantityIncrease(Guid itemRequisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_ItemRequisitionDetail _findEntity = _db.Task_ItemRequisitionDetail
                    .Where(x => x.RequisitionId == itemRequisitionId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.FinalizedQuantity = _findEntity.FinalizedQuantity + quantity;

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
        public bool UpdateItemRequisitionDetailForFinalizedQuantityDecrease(Guid itemRequisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_ItemRequisitionDetail _findEntity = _db.Task_ItemRequisitionDetail
                    .Where(x => x.RequisitionId == itemRequisitionId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.FinalizedQuantity = _findEntity.FinalizedQuantity - quantity;

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