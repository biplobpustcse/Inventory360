using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupAccountsGroup : IInsertSetupAccountsGroup
    {
        private Inventory360Entities _db;
        private Setup_AccountsGroup _entity;

        public DInsertSetupAccountsGroup(CommonSetupAccountsGroup entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_AccountsGroup
            {
                Code = entity.Code,
                Name = entity.Name,
                BalanceType = entity.BalanceType,
                IsDefault = "N",
                CompanyId = entity.CompanyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertAccountsGroup()
        {
            try
            {
                _db.Setup_AccountsGroup.Add(_entity);
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