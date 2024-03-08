using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskSalesOrderDeliveryInfo : IDeleteTaskSalesOrderDeliveryInfo
    {
        private Inventory360Entities _db;

        public DDeleteTaskSalesOrderDeliveryInfo()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteSalesOrderDeliveryInfo(Guid salesOrderId, long companyId)
        {
            try
            {
                _db.Task_SalesOrderDeliveryInfo
                    .RemoveRange(
                        _db.Task_SalesOrderDeliveryInfo
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