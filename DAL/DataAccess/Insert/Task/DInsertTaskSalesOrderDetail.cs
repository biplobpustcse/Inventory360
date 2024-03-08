using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;
using Inventory360DataModel;
using Inventory360DataModel.Task;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesOrderDetail : IInsertTaskSalesOrderDetail
    {
        private Inventory360Entities _db;
        private Task_SalesOrderDetail _entity;

        public DInsertTaskSalesOrderDetail(CommonTaskSalesOrderDetail entity, CurrencyConvertedAmount priceAmountForDetail, CurrencyConvertedAmount discountAmountForDetail)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesOrderDetail
            {
                SalesOrderDetailId = entity.SalesOrderDetailId,
                SalesOrderId = entity.SalesOrderId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                PrimaryUnitTypeId = entity.PrimaryUnitTypeId,
                SecondaryUnitTypeId = entity.SecondaryUnitTypeId,
                TertiaryUnitTypeId = entity.TertiaryUnitTypeId,
                SecondaryConversionRatio = entity.SecondaryConversionRatio,
                TertiaryConversionRatio = entity.TertiaryConversionRatio,
                Quantity = entity.Quantity,
                Price = priceAmountForDetail.BaseAmount,
                Price1 = priceAmountForDetail.Currency1Amount,
                Price2 = priceAmountForDetail.Currency2Amount,
                Discount = discountAmountForDetail.BaseAmount,
                Discount1 = discountAmountForDetail.Currency1Amount,
                Discount2 = discountAmountForDetail.Currency2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesOrderDetail()
        {
            try
            {
                _db.Task_SalesOrderDetail.Add(_entity);
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