using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskComplainReceiveDetail_Problem : IInsertTaskComplainReceiveDetail_Problem
    {
        private Inventory360Entities _db;
        private Task_ComplainReceiveDetail_Problem _entity;

        public DInsertTaskComplainReceiveDetail_Problem(CommonComplainReceiveDetail_Problem entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ComplainReceiveDetail_Problem
            {
                ReceiveDetailProblemId = entity.ReceiveDetailProblemId,
                ReceiveDetailId = entity.ReceiveDetailId,
                ProblemId = entity.ProblemId,
                Note = entity.Note,                
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertComplainReceiveDetail_Problem()
        {
            try
            {
                _db.Task_ComplainReceiveDetail_Problem.Add(_entity);
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