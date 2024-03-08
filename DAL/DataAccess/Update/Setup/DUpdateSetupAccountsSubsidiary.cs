using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupAccountsSubsidiary : IUpdateSetupAccountsSubsidiary
    {
        private Inventory360Entities _db;
        private Setup_AccountsSubsidiary _findEntity;

        public DUpdateSetupAccountsSubsidiary(CommonSetupAccountsSubsidiary entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_AccountsSubsidiary.Find(entity.AccountsSubsidiaryId);
            _findEntity.AccountsControlId = entity.AccountsControlId;
            _findEntity.Name = entity.SubsidiaryName;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateAccountsSubsidiary()
        {
            try
            {
                _db.Entry(_findEntity).State = EntityState.Modified;
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
