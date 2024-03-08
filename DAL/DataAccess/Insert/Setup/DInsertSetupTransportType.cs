using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupTransportType : IInsertSetupTransportType
    {
        private Inventory360Entities _db;
        private Setup_TransportType _entity;

        public DInsertSetupTransportType(CommonSetupTransportType entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_TransportType
            {
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTransportType()
        {
            try
            {
                _db.Setup_TransportType.Add(_entity);
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