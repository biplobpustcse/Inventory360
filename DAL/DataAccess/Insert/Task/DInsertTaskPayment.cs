using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPayment : IInsertTaskPayment
    {
        private Inventory360Entities _db;
        private Task_Payment _entity;

        public DInsertTaskPayment(CommonTaskPayment entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_Payment
            {
                PaymentId = entity.PaymentId,
                PaymentNo = entity.PaymentNo,
                PaymentDate = entity.PaymentDate + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = convertedAmount.Currency1Rate,
                Currency2Rate = convertedAmount.Currency2Rate,
                PaidAmount = convertedAmount.BaseAmount,
                PaidAmount1 = convertedAmount.Currency1Amount,
                PaidAmount2 = convertedAmount.Currency2Amount,
                SupplierId = entity.SupplierId,
                PaidBy = entity.PaidBy,
                ReferenceNo = entity.ReferenceNo,
                Remarks = entity.Remarks,
                OperationTypeId = entity.OperationTypeId,
                OperationalEventId = entity.OperationalEventId,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPayment()
        {
            try
            {
                _db.Task_Payment.Add(_entity);
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