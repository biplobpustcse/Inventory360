using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPostedVoucher : IInsertTaskPostedVoucher
    {
        private Inventory360Entities _db;
        private Task_PostedVoucher _entity;

        public DInsertTaskPostedVoucher(CommonTaskPostedVoucher entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PostedVoucher
            {
                PostedVoucherId = Guid.NewGuid(),
                Date = entity.VoucherDate,
                VoucherDetailId = entity.VoucherDetailId,
                VoucherType = entity.VoucherType,
                AccountsId = entity.AccountsId,
                ProjectId = entity.ProjectId,
                Amount = entity.Amount,
                Currency1Rate = entity.Rate1,
                Currency1Amount = entity.Amount1,
                Currency2Rate = entity.Rate2,
                Currency2Amount = entity.Amount2,
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPostedVoucher()
        {
            try
            {
                _db.Task_PostedVoucher.Add(_entity);
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
