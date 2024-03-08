using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskChequeInfo : IInsertTaskChequeInfo
    {
        private Inventory360Entities _db;
        private Task_ChequeInfo _entity;

        public DInsertTaskChequeInfo(long bankId, string chequeNo, DateTime chequeDate, long locationId, long companyId, long entryBy, CurrencyConvertedAmount convertedAmount, Guid? collectionDetailId, Guid? paymentDetailId, Guid? entryVoucherId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ChequeInfo
            {
                ChequeInfoId = Guid.NewGuid(),
                CollectionDetailId = collectionDetailId,
                PaymentDetailId = paymentDetailId,
                EntryVoucherId = entryVoucherId,
                BankId = bankId,
                ChequeNo = chequeNo,
                ChequeDate = chequeDate,
                Amount = convertedAmount.BaseAmount,
                Amount1 = convertedAmount.Currency1Amount,
                Amount2 = convertedAmount.Currency2Amount,
                LocationId = locationId,
                CompanyId = companyId,
                EntryBy = entryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertChequeInfo()
        {
            try
            {
                _db.Task_ChequeInfo.Add(_entity);
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