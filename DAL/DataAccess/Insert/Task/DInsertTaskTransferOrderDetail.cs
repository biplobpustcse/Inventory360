using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskTransferOrderDetail : IInsertTaskTransferOrderDetail
    {
        private Inventory360Entities _db;
        private Task_TransferOrderDetail _entity;

        public DInsertTaskTransferOrderDetail(CommonTransferOrderDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_TransferOrderDetail
            {
                OrderDetailId = entity.TransferOrderDetailId,
                OrderId = entity.TransferOrderId,
                RequisitionId = entity.RequisitionFinalizeId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTaskTransferOrderDetail()
        {
            try
            {
                _db.Task_TransferOrderDetail.Add(_entity);
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