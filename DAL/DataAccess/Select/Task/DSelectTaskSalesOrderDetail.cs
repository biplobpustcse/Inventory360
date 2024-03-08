using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskSalesOrderDetail : ISelectTaskSalesOrderDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskSalesOrderDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_SalesOrderDetail> SelectSalesOrderDetailAll()
        {
            return _db.Task_SalesOrderDetail
                .Where(x => x.Task_SalesOrder.CompanyId == _companyId);
        }
    }
}