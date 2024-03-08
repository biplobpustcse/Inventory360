using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskReceiveFinalizeNos : IDeleteTaskReceiveFinalizeNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskReceiveFinalizeNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteReceiveFinalizeNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_ReceiveFinalizeNos
                    .RemoveRange(
                        _db.Task_ReceiveFinalizeNos
                        .Where(x => x.FinalizeNo.ToLower().StartsWith(prefix.ToLower())
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