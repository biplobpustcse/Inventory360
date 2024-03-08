using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskTransferOrderNos : IDeleteTaskTransferOrderNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskTransferOrderNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteTaskTransferOrderNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_TransferOrderNos
                    .RemoveRange(
                        _db.Task_TransferOrderNos
                        .Where(x => x.OrderNo.ToLower().StartsWith(prefix.ToLower())
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