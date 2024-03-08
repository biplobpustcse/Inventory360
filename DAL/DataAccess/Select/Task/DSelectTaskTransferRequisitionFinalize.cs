using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskTransferRequisitionFinalize : ISelectTaskTransferRequisitionFinalize
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskTransferRequisitionFinalize(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferRequisitionFinalize> SelectStockTransferRequisitionFinalizeAll()
        {
            return _db.Task_TransferRequisitionFinalize.Where(x => x.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferRequisitionFinalize> SelectStockTransferRequisitionFinalize(Guid Id)
        {
            return _db.Task_TransferRequisitionFinalize.Where(x => x.CompanyId == _companyId).WhereIf(!String.IsNullOrEmpty(Id.ToString()),x=>x.RequisitionId==Id);
        }
    }
}