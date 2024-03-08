using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskVoucherDetail : ISelectTaskVoucherDetail
    {
        private Inventory360Entities _db;
        private Guid _voucherId;

        public DSelectTaskVoucherDetail(Guid id)
        {
            _db = new Inventory360Entities();
            _voucherId = id;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_VoucherDetail> SelectVoucherDetailByVoucherId()
        {
            return _db.Task_VoucherDetail
                .Where(x => x.VoucherId == _voucherId);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_VoucherDetail> SelectVoucherDetail()
        {
            return _db.Task_VoucherDetail;
        }
    }
}
