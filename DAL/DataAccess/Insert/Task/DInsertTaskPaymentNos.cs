using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPaymentNos : IInsertTaskPaymentNos
    {
        private Inventory360Entities _db;
        private Task_PaymentNos _entity;

        public DInsertTaskPaymentNos(string paymentNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PaymentNos
            {
                Id = Guid.NewGuid(),
                PaymentNo = paymentNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPaymentNos()
        {
            try
            {
                _db.Task_PaymentNos.Add(_entity);
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