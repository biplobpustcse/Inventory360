using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskComplainReceiveNos : IInsertTaskComplainReceiveNos
    {
        private Inventory360Entities _db;
        private Task_ComplainReceiveNos _entity;

        public DInsertTaskComplainReceiveNos(string ReceiveNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ComplainReceiveNos
            {
                Id = Guid.NewGuid(),
                ReceiveNo = ReceiveNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertComplainReceiveNos()
        {
            try
            {
                _db.Task_ComplainReceiveNos.Add(_entity);
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