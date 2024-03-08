using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCollectionNos : IInsertTaskCollectionNos
    {
        private Inventory360Entities _db;
        private Task_CollectionNos _entity;

        public DInsertTaskCollectionNos(string collectionNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CollectionNos
            {
                Id = Guid.NewGuid(),
                CollectionNo = collectionNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCollectionNos()
        {
            try
            {
                _db.Task_CollectionNos.Add(_entity);
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