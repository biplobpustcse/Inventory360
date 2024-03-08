using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskReplacementClaimNos : IDeleteTaskReplacementClaimNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskReplacementClaimNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteReplacementClaimNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_ReplacementClaimNos
                    .RemoveRange(
                        _db.Task_ReplacementClaimNos
                        .Where(x => x.ClaimNo.ToLower().StartsWith(prefix.ToLower())
                        && x.Year == year
                        && x.CompanyId == companyId)
                    );
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