using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupConvertionRatio : ISelectSetupConvertionRatio
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupConvertionRatio(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ConvertionRatio> SelectConvertionRatioAll()
        {
            return _db.Setup_ConvertionRatio
                .Where(x => x.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ConvertionRatio> SelectConvertionRatioWithoutCheckingCompany()
        {
            return _db.Setup_ConvertionRatio;
        }
    }
}