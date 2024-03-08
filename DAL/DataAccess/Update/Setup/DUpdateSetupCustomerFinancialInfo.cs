using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupCustomerFinancialInfo : IUpdateSetupCustomerFinancialInfo
    {
        private Inventory360Entities _db;
        private Setup_Customer _findEntity;

        public DUpdateSetupCustomerFinancialInfo(CommonSetupCustomerFinancialInfo entity, CurrencyConvertedAmount openingAmount, CurrencyConvertedAmount chequeDishonourAmount, CurrencyConvertedAmount creditLimit, CurrencyConvertedAmount ledgerLimit)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Customer.Find(entity.CustomerId);
            _findEntity.SelectedCurrency = entity.SelectedCurrency;
            _findEntity.Currency1Rate = openingAmount.Currency1Rate;
            _findEntity.Currency2Rate = openingAmount.Currency2Rate;
            _findEntity.OpeningBalance = openingAmount.BaseAmount;
            _findEntity.OpeningBalance1 = openingAmount.Currency1Amount;
            _findEntity.OpeningBalance2 = openingAmount.Currency2Amount;
            _findEntity.OpeningDate = entity.OpeningDate;
            _findEntity.ChequeDishonourBalance = chequeDishonourAmount.BaseAmount;
            _findEntity.ChequeDishonourBalance1 = chequeDishonourAmount.Currency1Amount;
            _findEntity.ChequeDishonourBalance2 = chequeDishonourAmount.Currency2Amount;
            _findEntity.ChequeDishonourOpeningDate = entity.ChequeDishonourOpeningDate;
            _findEntity.CreditLimit = creditLimit.BaseAmount;
            _findEntity.CreditLimit1 = creditLimit.Currency1Amount;
            _findEntity.CreditLimit2 = creditLimit.Currency2Amount;
            _findEntity.LedgerLimit = ledgerLimit.BaseAmount;
            _findEntity.LedgerLimit1 = ledgerLimit.Currency1Amount;
            _findEntity.LedgerLimit2 = ledgerLimit.Currency2Amount;
            _findEntity.CreditAllowedDays = entity.CreditAllowedDays;
            _findEntity.IsLocked = entity.IsLocked;
            _findEntity.IsRMALocked = entity.IsRMALocked;
            _findEntity.AccountsId = entity.AccountsId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCustomerFinancialInfo()
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