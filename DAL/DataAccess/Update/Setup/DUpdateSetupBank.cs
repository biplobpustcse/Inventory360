using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupBank : IUpdateSetupBank
    {
        private Inventory360Entities _db;
        private Setup_Bank _findEntity;

        public DUpdateSetupBank(CommonSetupBank entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Bank.Find(entity.BankId);
            _findEntity.Name = entity.Name;
            _findEntity.Address = entity.Address;
            _findEntity.IsOwnBank = entity.IsOwnBank;
            _findEntity.Branch = string.IsNullOrEmpty(entity.BankBranch) ? null : entity.BankBranch;
            _findEntity.BankAccountNo = string.IsNullOrEmpty(entity.BankACNo) ? null : entity.BankACNo;
            _findEntity.AccountsId = entity.AccountsId == 0 ? null : entity.AccountsId;
            _findEntity.EditedBy = entity.EntryBy;
            _findEntity.EditedDate = DateTime.Now;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateBank()
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
