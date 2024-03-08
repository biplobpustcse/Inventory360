using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskVoucherDetail : IInsertTaskVoucherDetail
    {
        private Inventory360Entities _db;
        private Task_VoucherDetail _entity;

        public DInsertTaskVoucherDetail(Guid voucherId, CommonTaskVoucherDetail entity, CurrencyConvertedVoucherAmount amountEntity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_VoucherDetail
            {
                VoucherDetailId = Guid.NewGuid(),
                VoucherId = voucherId,
                AccountsId = entity.AccountsId,
                ProjectId = entity.ProjectId,
                Particulars = entity.Particulars,
                Debit = amountEntity.BaseCurrencyDebit,
                Credit = amountEntity.BaseCurrencyCredit,
                Currency1Rate = amountEntity.Currency1Rate,
                Currency1Debit = amountEntity.Currency1Debit,
                Currency1Credit = amountEntity.Currency1Credit,
                Currency2Rate = amountEntity.Currency2Rate,
                Currency2Debit = amountEntity.Currency2Debit,
                Currency2Credit = amountEntity.Currency2Credit
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertVoucherDetail()
        {
            try
            {
                _db.Task_VoucherDetail.Add(_entity);
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