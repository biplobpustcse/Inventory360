using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskGoodsReceiveDetail : ISelectTaskGoodsReceiveDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskGoodsReceiveDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_GoodsReceiveDetail> SelectGoodsReceiveDetailAll()
        {
            return _db.Task_GoodsReceiveDetail
                .Where(x => x.Task_GoodsReceive.CompanyId == _companyId);
        }
    }
}