using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupTransport : IInsertSetupTransport
    {
        private Inventory360Entities _db;
        private Setup_Transport _entity;

        public DInsertSetupTransport(CommonSetupTransport entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Transport
            {
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTransport()
        {
            try
            {
                _db.Setup_Transport.Add(_entity);
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