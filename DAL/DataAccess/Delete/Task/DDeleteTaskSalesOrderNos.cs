using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskSalesOrderNos : IDeleteTaskSalesOrderNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskSalesOrderNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteSalesOrderNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_SalesOrderNos
                    .RemoveRange(
                        _db.Task_SalesOrderNos
                        .Where(x => x.SalesOrderNo.ToLower().StartsWith(prefix.ToLower())
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