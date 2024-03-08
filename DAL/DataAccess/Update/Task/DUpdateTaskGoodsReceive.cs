using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskGoodsReceive : IUpdateTaskGoodsReceive
    {
        private Inventory360Entities _db;
        private Task_GoodsReceive _findEntity;

        public DUpdateTaskGoodsReceive(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_GoodsReceive.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateGoodsReceiveForApprove(long approvedBy)
        {
            try
            {
                _findEntity.Approved = "A";
                _findEntity.ApprovedBy = approvedBy;
                _findEntity.ApprovedDate = DateTime.Now;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateGoodsReceiveForCancel(string reason, long cancelledBy)
        {
            try
            {
                _findEntity.Approved = "C";
                _findEntity.ApprovedBy = cancelledBy;
                _findEntity.ApprovedDate = DateTime.Now;
                _findEntity.CancelReason = reason;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateGoodsReceiveForIsSettled(bool value)
        {
            try
            {
                _findEntity.IsSettledByRecFinalize = value;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateGoodsReceiveForVoucherId(Guid voucherId)
        {
            try
            {
                _findEntity.VoucherId = voucherId;

                _db.Entry(_findEntity).State = EntityState.Modified;
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