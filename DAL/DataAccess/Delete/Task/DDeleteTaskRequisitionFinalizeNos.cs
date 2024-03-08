using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskRequisitionFinalizeNos : IDeleteTaskRequisitionFinalizeNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskRequisitionFinalizeNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteRequisitionFinalizeNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_RequisitionFinalizeNos
                    .RemoveRange(
                        _db.Task_RequisitionFinalizeNos
                        .Where(x => x.RequisitionNo.ToLower().StartsWith(prefix.ToLower())
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