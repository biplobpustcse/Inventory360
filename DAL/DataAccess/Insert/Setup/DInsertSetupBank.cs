using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupBank : IInsertSetupBank
    {
        private Inventory360Entities _db;
        private Setup_Bank _entity;

        public DInsertSetupBank(CommonSetupBank entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Bank
            {
                BankId = entity.BankId,
                Name = entity.Name,
                Address = entity.Address,
                IsOwnBank = entity.IsOwnBank,
                Branch = string.IsNullOrEmpty(entity.BankBranch) ? null : entity.BankBranch,
                BankAccountNo = string.IsNullOrEmpty(entity.BankACNo) ? null : entity.BankACNo,
                AccountsId = entity.AccountsId == 0 ? null : entity.AccountsId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertBank()
        {
            try
            {
                _db.Setup_Bank.Add(_entity);
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
