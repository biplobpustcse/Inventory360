using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Select.Task;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskComplainReceive : ISelectTaskComplainReceive
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskComplainReceive(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceive> SelectComplainReceiveAll()
        {
            return _db.Task_ComplainReceive.Where(x => x.CompanyId == _companyId);
        }       
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceive> SelectTaskComplainReceive(Guid Id)
        {
            return _db.Task_ComplainReceive.Where(x => x.CompanyId == _companyId).WhereIf(!String.IsNullOrEmpty(Id.ToString()),x=>x.ReceiveId == Id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceiveDetail_Problem> SelectTaskComplainReceiveDetailProblemAll()
        {
            return _db.Task_ComplainReceiveDetail_Problem.Where(x => x.Task_ComplainReceiveDetail.Task_ComplainReceive.CompanyId == _companyId);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceiveDetail_Problem> SelectTaskComplainReceiveDetailProblemByComRcvDtlId(Guid id)
        {
            return _db.Task_ComplainReceiveDetail_Problem.Where(x => x.ReceiveDetailId == id);
        }
        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ComplainReceive_Charge> SelectTaskComplainReceiveChargeByComRcvlId(Guid id)
        {
            return _db.Task_ComplainReceive_Charge.Where(x => x.ReceiveId == id);
        }
        public object SelectRMAProductNameByComplainReceive(long companyId, string query, Guid complainReceiveId)
        {
            var value = (from rma in _db.Stock_RMAStock
                         join product in _db.Setup_Product.WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                         on rma.ProductId equals product.ProductId
                         join cr in _db.Task_ComplainReceiveDetail
                         .WhereIf(complainReceiveId != Guid.Empty, x => x.Task_ComplainReceive.ReceiveId == complainReceiveId)
                          on rma.ProductId equals cr.ProductId into complainRcv
                         from complainReceive in complainRcv.DefaultIfEmpty()
                         select new
                         {
                             Item = "[" + product.Code + "] " + product.Name.ToString(),
                             Value = rma.ProductId.ToString(),
                             isSerialProduct = product.SerialAvailable,
                             product.Code,
                             complainReceiveId = complainReceive.ReceiveId,
                             complainReceive.Cost,
                             complainReceive.Cost1,
                             complainReceive.Cost2
                         }).ToList();

            return value;
        }
    }
}