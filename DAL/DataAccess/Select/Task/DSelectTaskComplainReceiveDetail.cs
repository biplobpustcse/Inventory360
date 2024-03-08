using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskComplainReceiveDetail : ISelectTaskComplainReceiveDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskComplainReceiveDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceiveDetail> SelectTaskComplainReceiveDetailAll()
        {
            return _db.Task_ComplainReceiveDetail.Where(x => x.Task_ComplainReceive.CompanyId == _companyId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceiveDetail_SpareProduct> SelectTaskComplainReceiveDetail_SpareProductAll()
        {
            return _db.Task_ComplainReceiveDetail_SpareProduct.Where(x => x.Task_ComplainReceiveDetail.Task_ComplainReceive.CompanyId == _companyId);
        }
    }
}