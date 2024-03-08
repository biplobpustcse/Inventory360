using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupPrice : IUpdateSetupPrice
    {
        private Inventory360Entities _db;
        private Setup_Price _findEntity;

        public DUpdateSetupPrice(CommonSetupPrice entity, CurrencyConvertedAmount amountEntity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Price.Find(entity.PriceId);
            _findEntity.PriceTypeId = entity.PriceTypeId;
            _findEntity.ProductId = entity.ProductId;
            _findEntity.UnitTypeId = entity.UnitTypeId;
            _findEntity.Price = amountEntity.BaseAmount;
            _findEntity.Price1Rate = amountEntity.Currency1Rate;
            _findEntity.Price1 = amountEntity.Currency1Amount;
            _findEntity.Price2Rate = amountEntity.Currency2Rate;
            _findEntity.Price2 = amountEntity.Currency2Amount;
            _findEntity.SelectedCurrency = entity.Currency;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdatePrice()
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