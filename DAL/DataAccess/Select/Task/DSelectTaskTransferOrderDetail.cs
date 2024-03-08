using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskTransferOrderDetail : ISelectTaskTransferOrderDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskTransferOrderDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferOrderDetail> SelectTransferOrderDetailAll()
        {
            return _db.Task_TransferOrderDetail
                .Where(x => x.Task_TransferOrder.CompanyId == _companyId);
        }
    }
}