using Inventory360DataModel;
using Inventory360Entity;
using DAL.DataAccess.Insert.Setup;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupAccountsSetup : DInsertSetupAccountsSetupBase, IUpdateSetupAccountsSetup
    {
        public DUpdateSetupAccountsSetup(CommonSetupAccountsSetup entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _entity = _db.Setup_Accounts.Find(entity.AccountsId);
            _entity.AccountsSubsidiaryId = entity.AccountsSubsidiaryId;
            _entity.Name = entity.AccountsName;
            _entity.CategorizationId = entity.CategorizationId;
            _entity.OpeningBalance = entity.OpeningBalance;
            _entity.BalanceType = GetBalanceTypeByAccountsGroup(entity.CompanyId, entity.AccountsGroupId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateAccountsSetup()
        {
            try
            {
                _db.Entry(_entity).State = EntityState.Modified;
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
