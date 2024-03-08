using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupTemplateHeader : IInsertSetupTemplateHeader
    {
        private Inventory360Entities _db;
        private Setup_TemplateHeader _entity;

        public DInsertSetupTemplateHeader(CommonSetupTemplateHeader entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_TemplateHeader
            {
                Name = entity.Name,
                Description = entity.Description,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTemplateHeader()
        {
            try
            {
                _db.Setup_TemplateHeader.Add(_entity);
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