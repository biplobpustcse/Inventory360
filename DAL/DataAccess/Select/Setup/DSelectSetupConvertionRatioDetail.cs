using Inventory360Entity;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Setup
{
    public class DSelectSetupConvertionRatioDetail : ISelectSetupConvertionRatioDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSetupConvertionRatioDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailAll()
        {
            return _db.Setup_ConvertionRatioDetail
                .Where(x => x.Setup_ConvertionRatio.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailById(Guid convertionRatioDetailId)
        {
            return _db.Setup_ConvertionRatioDetail
                .Where(x => x.ConvertionRatioDetailId == convertionRatioDetailId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailByConvertionRatioId(Guid convertionRatioId)
        {
            return _db.Setup_ConvertionRatioDetail
                .Where(x => x.ConvertionRatioId == convertionRatioId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Setup_ConvertionRatioDetail> SelectConvertionRatioDetailWithoutCheckingCompany()
        {
            return _db.Setup_ConvertionRatioDetail;
        }
    }
}