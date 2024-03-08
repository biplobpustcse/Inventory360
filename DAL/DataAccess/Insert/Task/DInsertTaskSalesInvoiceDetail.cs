using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesInvoiceDetail : IInsertTaskSalesInvoiceDetail
    {
        private Inventory360Entities _db;
        private Task_SalesInvoiceDetail _entity;

        public DInsertTaskSalesInvoiceDetail(CommonTaskSalesInvoiceDetail entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesInvoiceDetail
            {
                InvoiceDetailId = entity.InvoiceDetailId,
                InvoiceId = entity.InvoiceId,
                ChallanId = entity.ChallanId,
                WareHouseId = entity.WarehouseId == 0 ? null : entity.WarehouseId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                PrimaryUnitTypeId = entity.PrimaryUnitTypeId,
                SecondaryUnitTypeId = entity.SecondaryUnitTypeId,
                TertiaryUnitTypeId = entity.TertiaryUnitTypeId,
                SecondaryConversionRatio = entity.SecondaryConversionRatio,
                TertiaryConversionRatio = entity.TertiaryConversionRatio,
                Quantity = entity.Quantity,
                Price = entity.Price,
                Price1 = entity.Price1,
                Price2 = entity.Price2,
                Discount = entity.Discount,
                Discount1 = entity.Discount1,
                Discount2 = entity.Discount2,
                Cost = entity.Cost,
                Cost1 = entity.Cost1,
                Cost2 = entity.Cost2
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesInvoiceDetail()
        {
            try
            {
                _db.Task_SalesInvoiceDetail.Add(_entity);
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