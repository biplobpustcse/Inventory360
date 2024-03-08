using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementClaimNos : IInsertTaskReplacementClaimNos
    {
        private Inventory360Entities _db;
        private Task_ReplacementClaimNos _entity;

        public DInsertTaskReplacementClaimNos(string ClaimNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementClaimNos
            {
                Id = Guid.NewGuid(),
                ClaimNo = ClaimNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReplacementClaimNos()
        {
            try
            {
                _db.Task_ReplacementClaimNos.Add(_entity);
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