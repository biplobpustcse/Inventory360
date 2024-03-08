using Inventory360Entity;
using DAL.Interface.Select.Temp;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Temp
{
    public class DSelectTempCustomerOrSupplierWiseChequePerformance : ISelectTempCustomerOrSupplierWiseChequePerformance
    {
        private Inventory360Entities _db;
        private long _companyId;
        private long _entryBy;

        public DSelectTempCustomerOrSupplierWiseChequePerformance(long companyId,long entryBy)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
            _entryBy = entryBy;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<TempCustomerOrSupplierWiseChequePerformance> SelectTempCustomerOrSupplierWiseChequePerformance()
        {
            return _db.TempCustomerOrSupplierWiseChequePerformances
                .Where(x => x.CompanyId == _companyId && x.EntryBy == _entryBy);
        }
    }
}