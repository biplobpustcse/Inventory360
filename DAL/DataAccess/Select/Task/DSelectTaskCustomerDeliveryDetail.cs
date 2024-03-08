using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskCustomerDeliveryDetail : ISelectTaskCustomerDeliveryDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskCustomerDeliveryDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_CustomerDeliveryDetail> SelectCustomerDeliveryDetailAll()
        {
            return _db.Task_CustomerDeliveryDetail.Where(x => x.Task_CustomerDelivery.CompanyId == _companyId);
        }
    }
}