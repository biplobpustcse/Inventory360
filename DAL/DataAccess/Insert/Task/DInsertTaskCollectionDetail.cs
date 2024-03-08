using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCollectionDetail : IInsertTaskCollectionDetail
    {
        private Inventory360Entities _db;
        private Task_CollectionDetail _entity;

        public DInsertTaskCollectionDetail(CommonTaskCollectionDetail entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CollectionDetail
            {
                CollectionDetailId = entity.CollectionDetailId,
                CollectionId = entity.CollectionId,
                PaymentModeId = entity.PaymentModeId,
                Amount = convertedAmount.BaseAmount,
                Amount1 = convertedAmount.Currency1Amount,
                Amount2 = convertedAmount.Currency2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCollectionDetail()
        {
            try
            {
                _db.Task_CollectionDetail.Add(_entity);
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