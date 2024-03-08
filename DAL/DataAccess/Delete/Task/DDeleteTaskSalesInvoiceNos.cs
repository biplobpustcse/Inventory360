using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskSalesInvoiceNos : IDeleteTaskSalesInvoiceNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskSalesInvoiceNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteSalesInvoiceNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_SalesInvoiceNos
                    .RemoveRange(
                        _db.Task_SalesInvoiceNos
                        .Where(x => x.InvoiceNo.ToLower().StartsWith(prefix.ToLower())
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