using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskReceiveFinalize : IUpdateTaskReceiveFinalize
    {
        private Inventory360Entities _db;
        private Task_ReceiveFinalize _findEntity;

        public DUpdateTaskReceiveFinalize(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_ReceiveFinalize.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateReceiveFinalizeForApprove(long approvedBy)
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
        public bool UpdateReceiveFinalizeForCancel(string reason, long cancelledBy)
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
        public bool UpdateReceiveFinalizeForIsSettled(bool value)
        {
            try
            {
                _findEntity.IsSettledByPayment = value;

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
        public bool UpdateReceiveFinalizeForVoucherId(Guid voucherId)
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

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateReceiveFinalizeByAmount(decimal finalizeAmount, decimal finalize1Amount, decimal finalize2Amount)
        {
            _findEntity.FinalizeAmount = finalizeAmount;
            _findEntity.Finalize1Amount = finalize1Amount;
            _findEntity.Finalize2Amount = finalize2Amount;

            _db.Entry(_findEntity).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateReceiveFinalizeByPaidAmountIncrease(CurrencyConvertedAmount convertedAmount)
        {
            _findEntity.PaidAmount = _findEntity.PaidAmount + convertedAmount.BaseAmount;
            _findEntity.Paid1Amount = _findEntity.Paid1Amount + convertedAmount.Currency1Amount;
            _findEntity.Paid2Amount = _findEntity.Paid2Amount + convertedAmount.Currency2Amount;

            _db.Entry(_findEntity).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateReceiveFinalizeByPaidAmountDecrease(CurrencyConvertedAmount convertedAmount)
        {
            _findEntity.PaidAmount = _findEntity.PaidAmount - convertedAmount.BaseAmount;
            _findEntity.Paid1Amount = _findEntity.Paid1Amount - convertedAmount.Currency1Amount;
            _findEntity.Paid2Amount = _findEntity.Paid2Amount - convertedAmount.Currency2Amount;

            _db.Entry(_findEntity).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }
    }
}