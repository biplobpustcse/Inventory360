using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskImportedStockInDetailSerial : ISelectTaskImportedStockInDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskImportedStockInDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ImportedStockInDetailSerial> SelectImportedStockInDetailSerialAll()
        {
            return _db.Task_ImportedStockInDetailSerial
                .Where(x => x.Task_ImportedStockInDetail.Task_ImportedStockIn.CompanyId == _companyId);
        }
    }
}