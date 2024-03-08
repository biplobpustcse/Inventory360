using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskSalesInvoice : IUpdateTaskSalesInvoice
    {
        private Inventory360Entities _db;
        private Task_SalesInvoice _findEntity;

        public DUpdateTaskSalesInvoice(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_SalesInvoice.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSalesInvoiceByAmountAndDiscount(decimal invoiceAmount, decimal invoice1Amount, decimal invoice2Amount, decimal invoiceDiscount, decimal invoice1Discount, decimal invoice2Discount, decimal commission, decimal commission1, decimal commission2)
        {
            _findEntity.InvoiceAmount = invoiceAmount;
            _findEntity.Invoice1Amount = invoice1Amount;
            _findEntity.Invoice2Amount = invoice2Amount;
            _findEntity.InvoiceDiscount = invoiceDiscount;
            _findEntity.Invoice1Discount = invoice1Discount;
            _findEntity.Invoice2Discount = invoice2Discount;
            _findEntity.CommissionAmount = commission;
            _findEntity.Commission1Amount = commission1;
            _findEntity.Commission2Amount = commission2;

            _db.Entry(_findEntity).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSalesInvoiceByCollectedAmountIncrease(CurrencyConvertedAmount convertedAmount)
        {
            _findEntity.CollectedAmount = _findEntity.CollectedAmount + convertedAmount.BaseAmount;
            _findEntity.Collected1Amount = _findEntity.Collected1Amount + convertedAmount.Currency1Amount;
            _findEntity.Collected2Amount = _findEntity.Collected2Amount + convertedAmount.Currency2Amount;

            _db.Entry(_findEntity).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSalesInvoiceByCollectedAmountDecrease(CurrencyConvertedAmount convertedAmount)
        {
            _findEntity.CollectedAmount = _findEntity.CollectedAmount - convertedAmount.BaseAmount;
            _findEntity.Collected1Amount = _findEntity.Collected1Amount - convertedAmount.Currency1Amount;
            _findEntity.Collected2Amount = _findEntity.Collected2Amount - convertedAmount.Currency2Amount;

            _db.Entry(_findEntity).State = EntityState.Modified;
            _db.SaveChanges();

            return true;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSalesInvoiceForApprove(long approvedBy)
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
        public bool UpdateSalesInvoiceForCancel(string reason, long cancelledBy)
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
        public bool UpdateSalesInvoiceForVoucherId(Guid voucherId)
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
        public bool UpdateSalesInvoiceForIsSettled(bool value)
        {
            try
            {
                _findEntity.IsSettledByCollection = value;

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