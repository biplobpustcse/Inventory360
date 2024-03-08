using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPurchaseOrder : IInsertTaskPurchaseOrder
    {
        private Inventory360Entities _db;
        private Task_PurchaseOrder _entity;

        public DInsertTaskPurchaseOrder(CommonTaskPurchaseOrder entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PurchaseOrder
            {
                OrderId = entity.OrderId,
                OrderNo = entity.OrderNo,
                OrderDate = entity.OrderDate + DateTime.Now.TimeOfDay,
                PurchaseType = entity.PurchaseType,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = convertedAmount.Currency1Rate,
                Currency2Rate = convertedAmount.Currency2Rate,
                OrderAmount = convertedAmount.BaseAmount,
                Order1Amount = convertedAmount.Currency1Amount,
                Order2Amount = convertedAmount.Currency2Amount,
                SupplierId = entity.SupplierId,
                ReferenceNo = entity.ReferenceNo,
                ReferenceDate = entity.ReferenceDate,
                PaymentModeId = entity.PaymentModeId,
                Remarks = entity.Remarks,
                TermsAndConditionsId = entity.TermsAndConditionsId == 0 ? null : entity.TermsAndConditionsId,
                TermsAndConditionsDetail = entity.TermsAndConditionsDetail,
                PaymentTermsId = entity.PaymentTermsId == 0 ? null : entity.PaymentTermsId,
                PaymentTermsDetail = entity.PaymentTermsDetail,
                ShipmentType = entity.ShipmentType,
                DeliveryTo = entity.DeliveryTo,
                DeliveryContactNo = entity.DeliveryContactNo,
                DeliveryDate = entity.DeliveryDate,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPurchaseOrder()
        {
            try
            {
                _db.Task_PurchaseOrder.Add(_entity);
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