using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskCustomerDelivery : ISelectTaskCustomerDelivery
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskCustomerDelivery(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_CustomerDelivery> SelectTaskCustomerDeliveryAll()
        {
            return _db.Task_CustomerDelivery.Where(x => x.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_CustomerDelivery> SelectTaskCustomerDelivery(Guid Id)
        {
            return _db.Task_CustomerDelivery.Where(x => x.CompanyId == _companyId).WhereIf(!String.IsNullOrEmpty(Id.ToString()),x=>x.DeliveryId == Id);
        }
    }
}