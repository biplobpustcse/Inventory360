using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesOrder : IInsertTaskSalesOrder
    {
        private Inventory360Entities _db;
        private Task_SalesOrder _entity;

        public DInsertTaskSalesOrder(CommonTaskSalesOrder entity, CurrencyConvertedAmount orderAmount, CurrencyConvertedAmount discountAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesOrder
            {
                SalesOrderId = entity.SalesOrderId,
                SalesOrderNo = entity.SalesOrderNo,
                OrderDate = entity.OrderDate.Date,
                CustomerId = entity.CustomerId,
                BuyerId = entity.BuyerId == 0 ? null : entity.BuyerId,
                SalesPersonId = entity.SalesPersonId,
                SalesType = entity.SalesType,
                ReferenceNo = entity.ReferenceNo,
                ReferenceDate = entity.ReferenceDate,
                OperationTypeId = entity.OperationTypeId,
                TermsAndConditionsId = entity.TermsAndConditionsId == 0 ? null : entity.TermsAndConditionsId,
                TermsAndConditionsDetail = entity.TermsAndConditionsDetail,
                Remarks = entity.Remarks,
                ShipmentType = entity.ShipmentType,
                ApxShipmentDate = entity.ApxShipmentDate,
                ShipmentMode = entity.ShipmentMode,
                DeliveryFromId = entity.DeliveryFromId,
                WareHouseId = entity.WareHouseId == 0 ? null : entity.WareHouseId,
                PaymentModeId = entity.PaymentModeId,
                PromisedDate = entity.PromisedDate,
                PaymentTermsId = entity.PaymentTermsId == 0 ? null : entity.PaymentTermsId,
                PaymentTermsDetail = entity.PaymentTermsDetail,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = orderAmount.Currency1Rate,
                Currency2Rate = orderAmount.Currency2Rate,
                OrderAmount = orderAmount.BaseAmount,
                Order1Amount = orderAmount.Currency1Amount,
                Order2Amount = orderAmount.Currency2Amount,
                OrderDiscount = discountAmount.BaseAmount,
                Order1Discount = discountAmount.Currency1Amount,
                Order2Discount = discountAmount.Currency2Amount,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesOrder()
        {
            try
            {
                _db.Task_SalesOrder.Add(_entity);
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