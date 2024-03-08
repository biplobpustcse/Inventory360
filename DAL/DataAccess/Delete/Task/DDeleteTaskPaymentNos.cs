using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskPaymentNos : IDeleteTaskPaymentNos
    {
        private Inventory360Entities _db;

        public DDeleteTaskPaymentNos()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeletePaymentNos(string prefix, long year, long companyId)
        {
            try
            {
                _db.Task_PaymentNos
                    .RemoveRange(
                        _db.Task_PaymentNos
                        .Where(x => x.PaymentNo.ToLower().StartsWith(prefix.ToLower())
                        && x.Year == year
                        && x.CompanyId == companyId)
                    );
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