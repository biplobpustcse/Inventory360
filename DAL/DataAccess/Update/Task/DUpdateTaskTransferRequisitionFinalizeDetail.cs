using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskTransferRequisitionFinalizeDetail : IUpdateTaskTransferRequisitionFinalizeDetail
    {
        private Inventory360Entities _db;

        public DUpdateTaskTransferRequisitionFinalizeDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateTransferRequisitionDetailForOrderedQuantityIncrease(Guid requisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_TransferRequisitionFinalizeDetail _findEntity = _db.Task_TransferRequisitionFinalizeDetail
                    .Where(x => x.RequisitionId == requisitionId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.OrderedQuantity = _findEntity.OrderedQuantity + quantity;

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
        public bool UpdateTransferRequisitionDetailForOrderedQuantityDecrease(Guid requisitionId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_TransferRequisitionFinalizeDetail _findEntity = _db.Task_TransferRequisitionFinalizeDetail
                    .Where(x => x.RequisitionId == requisitionId
                        && x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId)
                    .FirstOrDefault();

                _findEntity.OrderedQuantity = _findEntity.OrderedQuantity - quantity;

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