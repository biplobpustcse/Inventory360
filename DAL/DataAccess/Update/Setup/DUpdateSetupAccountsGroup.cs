using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupAccountsGroup : IUpdateSetupAccountsGroup
    {
        private Inventory360Entities _db;
        private Setup_AccountsGroup _findEntity;

        public DUpdateSetupAccountsGroup(CommonSetupAccountsGroup entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_AccountsGroup.Find(entity.AccountsGroupId);
            _findEntity.Code = entity.Code;
            _findEntity.Name = entity.Name;
            _findEntity.BalanceType = entity.BalanceType;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateAccountsGroup()
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
