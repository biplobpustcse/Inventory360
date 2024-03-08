using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskComplainReceive : IInsertTaskComplainReceive
    {
        private Inventory360Entities _db;
        private Task_ComplainReceive _entity;

        public DInsertTaskComplainReceive(CommonComplainReceive entity, CurrencyConvertedAmount totalChargeAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ComplainReceive
            {
                ReceiveId = entity.ReceiveId,
                ReceiveNo = entity.ReceiveNo,
                ReceiveDate = entity.ReceiveDate + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = totalChargeAmount.Currency1Rate,
                Currency2Rate = totalChargeAmount.Currency2Rate,
                AgainstPreviousSales = entity.AgainstPreviousSales,
                CustomerId = entity.CustomerId,
                RequestedBy = entity.RequestedBy,
                TotalChargeAmount = totalChargeAmount.BaseAmount,
                TotalCharge1Amount = totalChargeAmount.Currency1Amount,
                TotalCharge2Amount = totalChargeAmount.Currency2Amount,
                Remarks = entity.Remarks,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
            };                             
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertComplainReceive()
        {
            try
            {
                _db.Task_ComplainReceive.Add(_entity);
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