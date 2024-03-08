using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupPriceDetail : IInsertSetupPriceDetail
    {
        private Inventory360Entities _db;
        private Setup_PriceDetail _entity;

        public DInsertSetupPriceDetail(long priceId, decimal lowerQty, decimal upperQty, long entryBy, CurrencyConvertedAmount amountEntity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_PriceDetail
            {
                PriceId = priceId,
                Price = amountEntity.BaseAmount,
                Price1Rate = amountEntity.Currency1Rate,
                Price1 = amountEntity.Currency1Amount,
                Price2Rate = amountEntity.Currency2Rate,
                Price2 = amountEntity.Currency2Amount,
                LowerRangeQty = lowerQty,
                UpperRangeQty = upperQty,
                EntryBy = entryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPriceDetail()
        {
            try
            {
                _db.Setup_PriceDetail.Add(_entity);
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