using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskGoodsReceive : IInsertTaskGoodsReceive
    {
        private Inventory360Entities _db;
        private Task_GoodsReceive _entity;

        public DInsertTaskGoodsReceive(CommonTaskGoodsReceive entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_GoodsReceive
            {
                ReceiveId = entity.ReceiveId,
                ReceiveNo = entity.ReceiveNo,
                ReceiveDate = entity.ReceiveDate + DateTime.Now.TimeOfDay,
                ReferenceNo = entity.ReferenceNo,
                ReferenceDate = entity.ReferenceDate,
                SupplierId = entity.SupplierId,
                OrderId = entity.OrderId,
                Remarks = entity.Remarks,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = convertedAmount.Currency1Rate,
                Currency2Rate = convertedAmount.Currency2Rate,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertGoodsReceive()
        {
            try
            {
                _db.Task_GoodsReceive.Add(_entity);
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