using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskDeliveryChallanDetail : ISelectTaskDeliveryChallanDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskDeliveryChallanDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_DeliveryChallanDetail> SelectDeliveryChallanDetailAll()
        {
            return _db.Task_DeliveryChallanDetail
                .Where(x => x.Task_DeliveryChallan.CompanyId == _companyId);
        }
    }
}