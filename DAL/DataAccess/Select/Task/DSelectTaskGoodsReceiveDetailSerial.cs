using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskGoodsReceiveDetailSerial : ISelectTaskGoodsReceiveDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskGoodsReceiveDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_GoodsReceiveDetailSerial> SelectGoodsReceiveDetailSerialAll()
        {
            return _db.Task_GoodsReceiveDetailSerial
                .Where(x => x.Task_GoodsReceiveDetail.Task_GoodsReceive.CompanyId == _companyId);
        }
    }
}