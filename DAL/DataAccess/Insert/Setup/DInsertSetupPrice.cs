using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupPrice : IInsertSetupPrice
    {
        private Inventory360Entities _db;
        private Setup_Price _entity;

        public DInsertSetupPrice(CommonSetupPrice entity, CurrencyConvertedAmount amountEntity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Price
            {
                PriceTypeId = entity.PriceTypeId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Price = amountEntity.BaseAmount,
                Price1Rate = amountEntity.Currency1Rate,
                Price1 = amountEntity.Currency1Amount,
                Price2Rate = amountEntity.Currency2Rate,
                Price2 = amountEntity.Currency2Amount,
                SelectedCurrency = entity.Currency,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPrice(out long priceId)
        {
            try
            {
                _db.Setup_Price.Add(_entity);
                _db.SaveChanges();

                priceId = _entity.PriceId;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}