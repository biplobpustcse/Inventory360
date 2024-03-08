using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCollection : IInsertTaskCollection
    {
        private Inventory360Entities _db;
        private Task_Collection _entity;

        public DInsertTaskCollection(CommonTaskCollection entity, CurrencyConvertedAmount convertedAmount)
        {
            _db = new Inventory360Entities();
            _entity = new Task_Collection
            {
                CollectionId = entity.CollectionId,
                CollectionNo = entity.CollectionNo,
                CollectionDate = entity.CollectionDate + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = convertedAmount.Currency1Rate,
                Currency2Rate = convertedAmount.Currency2Rate,
                CollectedAmount = convertedAmount.BaseAmount,
                CollectedAmount1 = convertedAmount.Currency1Amount,
                CollectedAmount2 = convertedAmount.Currency2Amount,
                CustomerId = entity.CustomerId,
                SalesPersonId = entity.SalesPersonId,
                CollectedBy = entity.CollectedBy,
                MRNo = entity.MRNo,
                Remarks = entity.Remarks,
                OperationTypeId = entity.OperationTypeId,
                OperationalEventId = entity.OperationalEventId,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCollection()
        {
            try
            {
                _db.Task_Collection.Add(_entity);
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