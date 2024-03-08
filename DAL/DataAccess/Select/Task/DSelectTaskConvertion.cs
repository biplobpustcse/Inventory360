using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskConvertion : ISelectTaskConvertion
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskConvertion(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_Convertion> SelectTaskConvertionAll()
        {
            return _db.Task_Convertion.Where(x => x.CompanyId == _companyId);
        }
    }
}