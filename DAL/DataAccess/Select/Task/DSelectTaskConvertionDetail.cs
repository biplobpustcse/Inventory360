using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskConvertionDetail : ISelectTaskConvertionDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskConvertionDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ConvertionDetail> SelectTaskConvertionDetailAll()
        {
            return _db.Task_ConvertionDetail.Where(x => x.Task_Convertion.CompanyId == _companyId);
        }
    }
}