using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPaymentDetail : IInsertTaskPaymentDetail
    {
        private Inventory360Entities _db;
        private Task_PaymentDetail _entity;

        public DInsertTaskPaymentDetail(CommonTaskPaymentDetail entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PaymentDetail
            {
                PaymentDetailId = entity.PaymentDetailId,
                PaymentId = entity.PaymentId,
                PaymentModeId = entity.PaymentModeId,
                Amount = convertedAmount.BaseAmount,
                Amount1 = convertedAmount.Currency1Amount,
                Amount2 = convertedAmount.Currency2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPaymentDetail()
        {
            try
            {
                _db.Task_PaymentDetail.Add(_entity);
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