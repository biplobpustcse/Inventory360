using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupCashBankIdentification : IInsertSetupCashBankIdentification
    {
        private Inventory360Entities _db;
        private Setup_AccountsCashBankIdentification _entity;

        public DInsertSetupCashBankIdentification(CommonSetupCashBankIdentification entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_AccountsCashBankIdentification
            {
                IdentificationType = entity.IdentificationType,
                AccountsControlId = entity.AccountsControlId == 0 ? null : entity.AccountsControlId,
                AccountsSubsidiaryId = entity.AccountsSubsidiaryId == 0 ? null : entity.AccountsSubsidiaryId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCashBankIdentification()
        {
            try
            {
                _db.Setup_AccountsCashBankIdentification.Add(_entity);
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
