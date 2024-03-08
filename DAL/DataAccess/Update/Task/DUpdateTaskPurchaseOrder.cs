using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskPurchaseOrder : IUpdateTaskPurchaseOrder
    {
        private Inventory360Entities _db;
        private Task_PurchaseOrder _findEntity;

        public DUpdateTaskPurchaseOrder(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_PurchaseOrder.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdatePurchaseOrderForApprove(long approvedBy)
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
        public bool UpdatePurchaseOrderForCancel(string reason, long cancelledBy)
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
        public bool UpdatePurchaseOrderForIsSettled(bool value)
        {
            try
            {
                _findEntity.IsSettledByGoodsReceive = value;

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
        public bool UpdatePurchaseOrderByPaymentInPaidAmountIncrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.PaidAmount = _findEntity.PaidAmount + convertedAmount.BaseAmount;
                _findEntity.Paid1Amount = _findEntity.Paid1Amount + convertedAmount.Currency1Amount;
                _findEntity.Paid2Amount = _findEntity.Paid2Amount + convertedAmount.Currency2Amount;

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
        public bool UpdatePurchaseOrderByPaymentInPaidAmountDecrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.PaidAmount = _findEntity.PaidAmount - convertedAmount.BaseAmount;
                _findEntity.Paid1Amount = _findEntity.Paid1Amount - convertedAmount.Currency1Amount;
                _findEntity.Paid2Amount = _findEntity.Paid2Amount - convertedAmount.Currency2Amount;

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