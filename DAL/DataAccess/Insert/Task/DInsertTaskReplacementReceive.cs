using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementReceive : IInsertTaskReplacementReceive
    {
        private Inventory360Entities _db;
        private Task_ReplacementReceive _entity;

        public DInsertTaskReplacementReceive(CommonTaskReplacementReceive entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementReceive
            {
                ReceiveId = entity.ReceiveId,
                ReceiveNo = entity.ReceiveNo,
                ReceiveDate = (DateTime)MyConversion.ConvertDateStringToDate(entity.ReceiveDate) + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = entity.Currency1Rate,
                Currency2Rate = entity.Currency2Rate,                
                RequestedBy = entity.RequestedBy,
                Remarks = entity.Remarks,
                Approved = "N",
                TotalChargeAmount = entity.TotalChargeAmount,
                TotalChargeAmount1 = entity.TotalChargeAmount1,
                TotalChargeAmount2 = entity.TotalChargeAmount2,
                TotalDiscount = entity.TotalDiscount,
                TotalDiscount1 = entity.TotalDiscount1,
                TotalDiscount2 = entity.TotalDiscount2,
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
            };                             
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReplacementReceive()
        {
            try
            {
                _db.Task_ReplacementReceive.Add(_entity);
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