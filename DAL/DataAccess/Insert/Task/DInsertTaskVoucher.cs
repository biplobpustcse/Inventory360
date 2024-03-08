using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskVoucher : IInsertTaskVoucher
    {
        private Inventory360Entities _db;
        private Task_Voucher _entity;

        public DInsertTaskVoucher(CommonTaskVoucher entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_Voucher
            {
                VoucherId = entity.VoucherId,
                VoucherNo = entity.VoucherNo,
                VoucherType = entity.VoucherType,
                OperationType = entity.OperationType,
                Date = entity.Date + DateTime.Now.TimeOfDay,
                Description = entity.Description,
                Approved = "N",
                Posted = false,
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
                SelectedCurrency = entity.Currency
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertVoucher()
        {
            try
            {
                _db.Task_Voucher.Add(_entity);
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
