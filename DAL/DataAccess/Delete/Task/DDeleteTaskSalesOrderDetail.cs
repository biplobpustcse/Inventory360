using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskSalesOrderDetail : IDeleteTaskSalesOrderDetail
    {
        private Inventory360Entities _db;

        public DDeleteTaskSalesOrderDetail()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteSalesOrderDetail(Guid salesOrderId, long companyId)
        {
            try
            {
                _db.Task_SalesOrderDetail
                    .RemoveRange(
                        _db.Task_SalesOrderDetail
                        .Where(x => x.SalesOrderId.Equals(salesOrderId)
                        && x.Task_SalesOrder.CompanyId == companyId)
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