using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskRequisitionFinalizeDetail : IUpdateTaskRequisitionFinalizeDetail
    {
        private Inventory360Entities _db;

        public DUpdateTaskRequisitionFinalizeDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateRequisitionDetailForOrderedQuantityIncrease(Guid requisitionDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_RequisitionFinalizeDetail _findEntity = _db.Task_RequisitionFinalizeDetail
                    .Where(x => x.RequisitionDetailId == requisitionDetailId
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
        public bool UpdateRequisitionDetailForOrderedQuantityDecrease(Guid requisitionDetailId, long productId, long unitTypeId, decimal quantity, long? productDimensionId)
        {
            try
            {
                Task_RequisitionFinalizeDetail _findEntity = _db.Task_RequisitionFinalizeDetail
                    .Where(x => x.RequisitionDetailId == requisitionDetailId
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