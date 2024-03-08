using Inventory360Entity;
using DAL.Interface.Delete.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Delete.Task
{
    public class DDeleteTaskVoucher : IDeleteTaskVoucher
    {
        private Inventory360Entities _db;

        public DDeleteTaskVoucher()
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;
        }

        // https://stackoverflow.com/questions/2519866/how-do-i-delete-multiple-rows-in-entity-framework-without-foreach
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool DeleteVoucher(Guid voucherId, long companyId)
        {
            try
            {
                _db.Task_Voucher
                    .RemoveRange(_db.Task_Voucher.Where(x => x.VoucherId == voucherId && x.CompanyId == companyId));
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