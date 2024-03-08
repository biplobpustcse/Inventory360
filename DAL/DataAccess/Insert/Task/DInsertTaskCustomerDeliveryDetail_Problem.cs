using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCustomerDeliveryDetail_Problem : IInsertTaskCustomerDeliveryDetail_Problem
    {
        private Inventory360Entities _db;
        private Task_CustomerDeliveryDetail_Problem _entity;

        public DInsertTaskCustomerDeliveryDetail_Problem(CommonTaskCustomerDeliveryDetail_Problem entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CustomerDeliveryDetail_Problem
            {
                DeliveryDetailProblemId = entity.DeliveryDetailProblemId,
                DeliveryDetailId = entity.DeliveryDetailId,
                ProblemId = entity.ProblemId,
                Note = entity.Note,                
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomerDeliveryDetail_Problem()
        {
            try
            {
                _db.Task_CustomerDeliveryDetail_Problem.Add(_entity);
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