using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupSupplierFinancialInfo : IUpdateSetupSupplierFinancialInfo
    {
        private Inventory360Entities _db;
        private Setup_Supplier _findEntity;

        public DUpdateSetupSupplierFinancialInfo(CommonSetupSupplierFinancialInfo entity, CurrencyConvertedAmount openingAmount)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Supplier.Find(entity.SupplierId);
            _findEntity.OpeningBalance = openingAmount.BaseAmount;
            _findEntity.OpeningBalance1 = openingAmount.Currency1Amount;
            _findEntity.OpeningBalance2 = openingAmount.Currency2Amount;
            _findEntity.OpeningDate = entity.OpeningDate;
            _findEntity.AccountsId = entity.AccountsId;
            _findEntity.SelectedCurrency = entity.SelectedCurrency;
            _findEntity.Currency1Rate = openingAmount.Currency1Rate;
            _findEntity.Currency2Rate = openingAmount.Currency2Rate;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSupplierFinancialInfo()
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