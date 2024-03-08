using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskItemRequisitionDetail : IInsertTaskItemRequisitionDetail
    {
        private Inventory360Entities _db;
        private Task_ItemRequisitionDetail _entity;

        public DInsertTaskItemRequisitionDetail(CommonTaskItemRequisitionDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ItemRequisitionDetail
            {
                RequisitionDetailId = entity.RequisitionDetailId,
                RequisitionId = entity.RequisitionId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity,
                RequiredDate = entity.RequiredDate,
                Reason = entity.Reason
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertItemRequisitionDetail()
        {
            try
            {
                _db.Task_ItemRequisitionDetail.Add(_entity);
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