using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCollectionMapping : IInsertTaskCollectionMapping
    {
        private Inventory360Entities _db;
        private Task_CollectionMapping _entity;

        public DInsertTaskCollectionMapping(CommonTaskCollectionMapping entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CollectionMapping
            {
                MappingId = Guid.NewGuid(),
                CollectionId = entity.CollectionId,
                SalesOrderId = entity.SalesOrderId,
                InvoiceId = entity.InvoiceId,
                Amount = convertedAmount.BaseAmount,
                Amount1 = convertedAmount.Currency1Amount,
                Amount2 = convertedAmount.Currency2Amount
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCollectionMapping()
        {
            try
            {
                _db.Task_CollectionMapping.Add(_entity);
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