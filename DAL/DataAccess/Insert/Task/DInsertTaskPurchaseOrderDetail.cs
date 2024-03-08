using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPurchaseOrderDetail : IInsertTaskPurchaseOrderDetail
    {
        private Inventory360Entities _db;
        private Task_PurchaseOrderDetail _entity;

        public DInsertTaskPurchaseOrderDetail(CommonTaskPurchaseOrderDetail entity, CurrencyConvertedAmount priceAmountForDetail)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PurchaseOrderDetail
            {
                OrderDetailId = entity.OrderDetailId,
                OrderId = entity.OrderId,
                RequisitionId = entity.RequisitionId,
                ProductId = entity.ProductId,
                ProductDimensionId = entity.ProductDimensionId == 0 ? null : entity.ProductDimensionId,
                UnitTypeId = entity.UnitTypeId,
                Quantity = entity.Quantity,
                Price = priceAmountForDetail.BaseAmount,
                Price1 = priceAmountForDetail.Currency1Amount,
                Price2 = priceAmountForDetail.Currency2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPurchaseOrderDetail()
        {
            try
            {
                _db.Task_PurchaseOrderDetail.Add(_entity);
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