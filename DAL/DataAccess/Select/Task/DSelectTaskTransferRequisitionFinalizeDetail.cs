using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskTransferRequisitionFinalizeDetail : ISelectTaskTransferRequisitionFinalizeDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskTransferRequisitionFinalizeDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferRequisitionFinalizeDetail> SelectRequisitionFinalizeDetailAll()
        {
            return _db.Task_TransferRequisitionFinalizeDetail
                .Where(x => x.Task_TransferRequisitionFinalize.CompanyId == _companyId);
        }
    }
}