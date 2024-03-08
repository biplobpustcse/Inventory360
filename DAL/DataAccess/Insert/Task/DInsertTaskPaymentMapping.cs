using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPaymentMapping : IInsertTaskPaymentMapping
    {
        private Inventory360Entities _db;
        private Task_PaymentMapping _entity;

        public DInsertTaskPaymentMapping(CommonTaskPaymentMapping entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PaymentMapping
            {
                MappingId = Guid.NewGuid(),
                PaymentId = entity.PaymentId,
                OrderId = entity.OrderId,
                FinalizeId = entity.FinalizeId,
                Amount = convertedAmount.BaseAmount,
                Amount1 = convertedAmount.Currency1Amount,
                Amount2 = convertedAmount.Currency2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPaymentMapping()
        {
            try
            {
                _db.Task_PaymentMapping.Add(_entity);
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