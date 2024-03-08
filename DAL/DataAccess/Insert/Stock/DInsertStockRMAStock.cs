using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Stock;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Stock
{
    public class DInsertStockRMAStock : IInsertStockRMAStock
    {
        private Inventory360Entities _db;
        private Stock_RMAStock _entity;

        public DInsertStockRMAStock(Guid RMAStockId, string ReferenceNo, DateTime ReferenceDate, long productId, long unitTypeId, decimal quantity, decimal cost, decimal cost1, decimal cost2, long locationId, long companyId, long? dimensionId, long? wareHouseId)
        {
            _db = new Inventory360Entities();
            _entity = new Stock_RMAStock
            {
                RMAStockId = RMAStockId,
                ReferenceNo = ReferenceNo,
                ReferenceDate = ReferenceDate,
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
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertRMAStock()
        {
            try
            {
                _db.Stock_RMAStock.Add(_entity);
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