using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Stock;
using System;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Stock
{
    public class DUpdateStockCurrentStock : IUpdateStockCurrentStock
    {
        private Inventory360Entities _db;

        public DUpdateStockCurrentStock()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCurrentStockQuantityByIncrease(out Guid stockId, long companyId, long locationId, long productId, long primaryUnitTypeId, decimal quantity, long? productDimensionId, long? wareHouseId, decimal cost = 0, decimal cost1 = 0, decimal cost2 = 0, string referenceNo = "")
        {
            try
            {
                Stock_CurrentStock _findEntity = _db.Stock_CurrentStock
                    .Where(x => x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == primaryUnitTypeId
                        && x.CompanyId == companyId
                        && x.LocationId == locationId
                        && x.WareHouseId == (wareHouseId == 0 ? null : wareHouseId))
                    .WhereIf(!string.IsNullOrEmpty(referenceNo), x => x.ReferenceNo.Trim().ToLower().Equals(referenceNo.Trim().ToLower()))
                    .FirstOrDefault();

                if (_findEntity != null)
                {
                    _findEntity.Quantity = _findEntity.Quantity + quantity;
                    if (cost > 0) _findEntity.Cost = cost;
                    if (cost1 > 0) _findEntity.Cost1 = cost1;
                    if (cost2 > 0) _findEntity.Cost2 = cost2;

                    _db.Entry(_findEntity).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                stockId = _findEntity.CurrentStockId;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCurrentStockQuantityByDecrease(long companyId, long locationId, long productId, long unitTypeId, decimal quantity, long? productDimensionId, long? wareHouseId, Guid goodsReceiveId, Guid importedStockInId, string referenceNo)
        {
            try
            {
                Stock_CurrentStock _findEntity = _db.Stock_CurrentStock
                    .Where(x => x.ProductId == productId
                        && x.ProductDimensionId == (productDimensionId == 0 ? null : productDimensionId)
                        && x.UnitTypeId == unitTypeId
                        && x.CompanyId == companyId
                        && x.LocationId == locationId
                        && x.WareHouseId == (wareHouseId == 0 ? null : wareHouseId))
                    .WhereIf(goodsReceiveId != Guid.Empty, x => x.GoodsReceiveId == goodsReceiveId)
                    .WhereIf(importedStockInId != Guid.Empty, x => x.ImportedStockInId == importedStockInId)
                    .WhereIf(!string.IsNullOrEmpty(referenceNo), x => x.ReferenceNo.Trim().ToLower().Equals(referenceNo.Trim().ToLower()))
                    .FirstOrDefault();

                if (_findEntity != null)
                {
                    _findEntity.Quantity = _findEntity.Quantity - quantity;

                    _db.Entry(_findEntity).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCurrentStockQuantityByDecrease(Guid stockId, decimal quantity)
        {
            try
            {
                Stock_CurrentStock _findEntity = _db.Stock_CurrentStock
                    .Where(x => x.CurrentStockId == stockId)
                    .FirstOrDefault();

                if (_findEntity != null)
                {
                    _findEntity.Quantity = _findEntity.Quantity - quantity;

                    _db.Entry(_findEntity).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCurrentStockQuantityByIncrease(Guid stockId, decimal quantity)
        {
            try
            {
                Stock_CurrentStock _findEntity = _db.Stock_CurrentStock
                    .Where(x => x.CurrentStockId == stockId)
                    .FirstOrDefault();

                if (_findEntity != null)
                {
                    _findEntity.Quantity = _findEntity.Quantity + quantity;

                    _db.Entry(_findEntity).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCurrentStockCost(Guid stockId, decimal cost, decimal cost1, decimal cost2)
        {
            try
            {
                Stock_CurrentStock _findEntity = _db.Stock_CurrentStock
                    .Where(x => x.CurrentStockId == stockId)
                    .FirstOrDefault();

                if (_findEntity != null)
                {
                    _findEntity.Cost = cost;
                    _findEntity.Cost1 = cost1;
                    _findEntity.Cost2 = cost2;

                    _db.Entry(_findEntity).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCurrentStockReference(Guid stockId, Guid? GoodsReceiveId, Guid? ImportedStockInId, long? SupplierId, string ReferenceNo, DateTime ReferenceDate)
        {
            try
            {
                Stock_CurrentStock _findEntity = _db.Stock_CurrentStock
                    .Where(x => x.CurrentStockId == stockId)
                    .FirstOrDefault();

                if (_findEntity != null)
                {
                    _findEntity.GoodsReceiveId = GoodsReceiveId;
                    _findEntity.ImportedStockInId = ImportedStockInId;
                    _findEntity.SupplierId = SupplierId;
                    _findEntity.ReferenceNo = ReferenceNo;
                    _findEntity.ReferenceDate = ReferenceDate;

                    _db.Entry(_findEntity).State = EntityState.Modified;
                    _db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}