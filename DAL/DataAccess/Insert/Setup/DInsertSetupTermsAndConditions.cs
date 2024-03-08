using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupTermsAndConditions : IInsertSetupTermsAndConditions
    {
        private Inventory360Entities _db;
        private Setup_TermsAndConditions _entity;

        public DInsertSetupTermsAndConditions(CommonSetupTermsAndConditions entity, long operationalEventId)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_TermsAndConditions
            {
                OperationalEventId = operationalEventId,
                TemplateHeaderId = entity.TemplateHeaderId,
                Detail = entity.Detail,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTermsAndConditions()
        {
            try
            {
                _db.Setup_TermsAndConditions.Add(_entity);
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