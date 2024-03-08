using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertStockAdjustmentDetail : IInsertStockAdjustmentDetail
    {
        private Inventory360Entities _db;
        private Task_StockAdjustmentDetail _entity;

        public DInsertStockAdjustmentDetail(CommonTaskStockAdjustmentDetail entity, CurrencyConvertedAmount priceAmountForDetail)
        {
            _db = new Inventory360Entities();
            _entity = new Task_StockAdjustmentDetail
            {
                AdjustmentDetailId = entity.AdjustmentDetailId,
                AdjustmentId = entity.AdjustmentId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity,
                IncreaseDecrease = entity.OperationType,
                WareHouseId = entity.WarehouseId == 0 ? null : entity.WarehouseId,
                Cost = priceAmountForDetail.BaseAmount,
                Cost1 = priceAmountForDetail.Currency1Amount,
                Cost2 = priceAmountForDetail.Currency2Amount,
                PrimaryUnitTypeId = entity.PrimaryUnitTypeId,
                SecondaryUnitTypeId = entity.SecondaryUnitTypeId,
                TertiaryUnitTypeId = entity.TertiaryUnitTypeId,
                SecondaryConversionRatio = entity.SecondaryConversionRatio,
                TertiaryConversionRatio = entity.TertiaryConversionRatio
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertStockAdjustmentDetail()
        {
            try
            {
                _db.Task_StockAdjustmentDetail.Add(_entity);
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