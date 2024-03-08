using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupPriceType : ISelectSetupPriceType
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupPriceType(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_PriceType> SelectPriceTypeAll()
        {
            return _db.Setup_PriceType
                .Where(x => x.CompanyId == _companyId);
        }
    }
}