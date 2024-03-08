using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskSalesOrder : IUpdateTaskSalesOrder
    {
        private Inventory360Entities _db;
        private Task_SalesOrder _findEntity;

        public DUpdateTaskSalesOrder(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_SalesOrder.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSalesOrder(CommonTaskSalesOrder entity, CurrencyConvertedAmount orderAmount, CurrencyConvertedAmount discountAmount)
        {
            try
            {
                _findEntity.OrderDate = entity.OrderDate.Date;
                _findEntity.SalesPersonId = entity.SalesPersonId;
                _findEntity.SalesType = entity.SalesType;
                _findEntity.ReferenceNo = entity.ReferenceNo;
                _findEntity.ReferenceDate = entity.ReferenceDate;
                _findEntity.OperationTypeId = entity.OperationTypeId;
                _findEntity.TermsAndConditionsId = entity.TermsAndConditionsId == 0 ? null : entity.TermsAndConditionsId;
                _findEntity.TermsAndConditionsDetail = entity.TermsAndConditionsDetail;
                _findEntity.Remarks = entity.Remarks;
                _findEntity.ShipmentType = entity.ShipmentType;
                _findEntity.ApxShipmentDate = entity.ApxShipmentDate;
                _findEntity.ShipmentMode = entity.ShipmentMode;
                _findEntity.DeliveryFromId = entity.DeliveryFromId;
                _findEntity.WareHouseId = entity.WareHouseId == 0 ? null : entity.WareHouseId;
                _findEntity.PaymentModeId = entity.PaymentModeId;
                _findEntity.PromisedDate = entity.PromisedDate;
                _findEntity.PaymentTermsId = entity.PaymentTermsId == 0 ? null : entity.PaymentTermsId;
                _findEntity.PaymentTermsDetail = entity.PaymentTermsDetail;
                _findEntity.OrderAmount = orderAmount.BaseAmount;
                _findEntity.Order1Amount = orderAmount.Currency1Amount;
                _findEntity.Order2Amount = orderAmount.Currency2Amount;
                _findEntity.OrderDiscount = discountAmount.BaseAmount;
                _findEntity.Order1Discount = discountAmount.Currency1Amount;
                _findEntity.Order2Discount = discountAmount.Currency2Amount;

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
        public bool UpdateSalesOrderForApprove(long approvedBy)
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
        public bool UpdateSalesOrderForCancel(string reason, long cancelledBy)
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
        public bool UpdateSalesOrderForIsSettled(bool value)
        {
            try
            {
                _findEntity.IsSettledByChallan = value;

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
        public bool UpdateSalesOrderByCollectionInCollectedAmountIncrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.CollectedAmount = _findEntity.CollectedAmount + convertedAmount.BaseAmount;
                _findEntity.Collected1Amount = _findEntity.Collected1Amount + convertedAmount.Currency1Amount;
                _findEntity.Collected2Amount = _findEntity.Collected2Amount + convertedAmount.Currency2Amount;

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
        public bool UpdateSalesOrderByCollectionInCollectedAmountDecrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.CollectedAmount = _findEntity.CollectedAmount - convertedAmount.BaseAmount;
                _findEntity.Collected1Amount = _findEntity.Collected1Amount - convertedAmount.Currency1Amount;
                _findEntity.Collected2Amount = _findEntity.Collected2Amount - convertedAmount.Currency2Amount;

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