using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskCollectionDetail : ISelectTaskCollectionDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskCollectionDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_CollectionDetail> SelectCollectionDetailAll()
        {
            return _db.Task_CollectionDetail
                .Where(x => x.Task_Collection.CompanyId == _companyId);
        }
    }
}