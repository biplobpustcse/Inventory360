using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReceiveFinalizeNos : IInsertTaskReceiveFinalizeNos
    {
        private Inventory360Entities _db;
        private Task_ReceiveFinalizeNos _entity;

        public DInsertTaskReceiveFinalizeNos(string finalizeNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReceiveFinalizeNos
            {
                Id = Guid.NewGuid(),
                FinalizeNo = finalizeNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReceiveFinalizeNos()
        {
            try
            {
                _db.Task_ReceiveFinalizeNos.Add(_entity);
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