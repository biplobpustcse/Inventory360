using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementReceiveNos : IInsertTaskReplacementReceiveNos
    {
        private Inventory360Entities _db;
        private Task_ReplacementReceiveNos _entity;

        public DInsertTaskReplacementReceiveNos(string ReceiveNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementReceiveNos
            {
                Id = Guid.NewGuid(),
                ReceiveNo = ReceiveNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReplacementReceiveNos()
        {
            try
            {
                _db.Task_ReplacementReceiveNos.Add(_entity);
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