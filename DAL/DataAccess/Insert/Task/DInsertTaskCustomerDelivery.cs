using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCustomerDelivery : IInsertTaskCustomerDelivery
    {
        private Inventory360Entities _db;
        private Task_CustomerDelivery _entity;

        public DInsertTaskCustomerDelivery(CommonTaskCustomerDelivery entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CustomerDelivery
            {
                DeliveryId = entity.DeliveryId,
                DeliveryNo = entity.DeliveryNo,
                DeliveryDate = (DateTime)MyConversion.ConvertDateStringToDate(entity.DeliveryDate) + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = entity.Currency1Rate,
                Currency2Rate = entity.Currency2Rate,                
                CustomerId = entity.CustomerId,
                RequestedBy = entity.RequestedBy,
                TotalAmount = entity.TotalAmount,
                TotalAmount1 = entity.TotalAmount1,
                TotalAmount2 = entity.TotalAmount2,
                TotalChargeAmount = entity.TotalChargeAmount,
                TotalChargeAmount1 = entity.TotalChargeAmount1,
                TotalChargeAmount2 = entity.TotalChargeAmount2,
                Discount = entity.Discount,
                Discount1 = entity.Discount1,
                Discount2 = entity.Discount2,
                Remarks = entity.Remarks,
                Approved = "N",
                ApprovedBy = entity.ApprovedBy,
                ApprovedDate = entity.ApprovedDate,
                CancelReason = entity.CancelReason,
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
            };                             
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomerDelivery()
        {
            try
            {
                _db.Task_CustomerDelivery.Add(_entity);
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