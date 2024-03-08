using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskGoodsReceiveDetail : IUpdateTaskGoodsReceiveDetail
    {
        private Inventory360Entities _db;

        public DUpdateTaskGoodsReceiveDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateGoodsReceiveDetailForReceiveFinalizeQuantityIncrease(Guid receiveId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_GoodsReceiveDetail _findEntity = _db.Task_GoodsReceiveDetail
                    .Where(x => x.ReceiveId == receiveId
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
        public bool UpdateGoodsReceiveDetailForReceiveFinalizeQuantityDecrease(Guid receiveId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_GoodsReceiveDetail _findEntity = _db.Task_GoodsReceiveDetail
                    .Where(x => x.ReceiveId == receiveId
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