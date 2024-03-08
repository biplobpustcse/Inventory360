using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskCollectionMapping : ISelectTaskCollectionMapping
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskCollectionMapping(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_CollectionMapping> SelectCollectionMappingAll()
        {
            return _db.Task_CollectionMapping
                .Where(x => x.Task_Collection.CompanyId == _companyId);
        }
    }
}