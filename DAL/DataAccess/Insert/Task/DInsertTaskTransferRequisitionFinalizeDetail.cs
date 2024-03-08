using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskTransferRequisitionFinalizeDetail : IInsertTaskTransferRequisitionFinalizeDetail
    {
        private Inventory360Entities _db;
        private Task_TransferRequisitionFinalizeDetail _entity;

        public DInsertTaskTransferRequisitionFinalizeDetail(CommonTransferRequisitionFinalizeDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_TransferRequisitionFinalizeDetail
            {
                RequisitionDetailId = entity.RequisitionDetailId,
                RequisitionId = entity.RequisitionId,
                ItemRequisitionId = entity.ItemRequisitionId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTransferRequisitionFinalizeDetail()
        {
            try
            {
                _db.Task_TransferRequisitionFinalizeDetail.Add(_entity);
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