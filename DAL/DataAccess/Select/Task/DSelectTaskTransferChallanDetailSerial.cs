using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskTransferChallanDetailSerial : ISelectTaskTransferChallanDetailSerial
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskTransferChallanDetailSerial(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_TransferChallanDetailSerial> SelectTransferChallanDetailSerialAll()
        {
            return _db.Task_TransferChallanDetailSerial
                .Where(x => x.Task_TransferChallanDetail.Task_TransferChallan.CompanyId == _companyId);
        }
    }
}