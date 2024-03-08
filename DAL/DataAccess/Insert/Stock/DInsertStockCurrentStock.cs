using Inventory360Entity;
using DAL.Interface.Insert.Stock;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Stock
{
    public class DInsertStockCurrentStock : IInsertStockCurrentStock
    {
        private Inventory360Entities _db;
        private Stock_CurrentStock _entity;

        public DInsertStockCurrentStock(Guid stockId, long productId, long unitTypeId, decimal quantity, decimal cost, decimal cost1, decimal cost2, long locationId, long companyId,string referenceNo, DateTime referenceDate, long? dimensionId, long? wareHouseId)
        {
            _db = new Inventory360Entities();
            _entity = new Stock_CurrentStock
            {
                CurrentStockId = stockId,
                ProductId = productId,
                ProductDimensionId = dimensionId == 0 ? null : dimensionId,
                UnitTypeId = unitTypeId,
                Quantity = quantity,
                Cost = cost,
                Cost1 = cost1,
                Cost2 = cost2,
                LocationId = locationId,
                WareHouseId = wareHouseId == 0 ? null : wareHouseId,
                CompanyId = companyId,
                EntryDate = DateTime.Now,
                ReferenceNo = referenceNo,
                ReferenceDate = referenceDate
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCurrentStock()
        {
            try
            {
                _db.Stock_CurrentStock.Add(_entity);
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