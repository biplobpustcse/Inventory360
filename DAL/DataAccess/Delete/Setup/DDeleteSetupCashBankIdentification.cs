using DAL.Interface.Delete.Setup;
using Inventory360Entity;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Setup
{
    public class DDeleteSetupCashBankIdentification : IDeleteSetupCashBankIdentification
    {
        private Inventory360Entities _db;

        public DDeleteSetupCashBankIdentification()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteCashBankIdentification(long id, long companyId)
        {
            try
            {
                _db.Setup_AccountsCashBankIdentification
                    .RemoveRange(
                        _db.Setup_AccountsCashBankIdentification
                        .Where(x => x.IdentificationId == id
                        && x.CompanyId == companyId)
                    );
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}