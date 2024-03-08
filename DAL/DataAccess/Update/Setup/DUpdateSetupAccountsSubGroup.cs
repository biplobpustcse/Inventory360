using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupAccountsSubGroup : IUpdateSetupAccountsSubGroup
    {
        private Inventory360Entities _db;
        private Setup_AccountsSubGroup _findEntity;

        public DUpdateSetupAccountsSubGroup(CommonSetupAccountsSubGroup entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_AccountsSubGroup.Find(entity.AccountsSubGroupId);
            _findEntity.AccountsGroupId = entity.AccountsGroupId;
            _findEntity.Name = entity.SubGroupName;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateAccountsSubGroup()
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
