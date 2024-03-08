using Inventory360Entity;
using DAL.Interface.Select.Security;
using System;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Security
{
    public class DSelectSecurityUser : ISelectSecurityUser
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectSecurityUser(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Security_User> SelectSecurityUser()
        {
            try
            {
                return _db.Security_User
                    .Where(x => x.CompanyId == _companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Security_UserLocation> SelectSecurityUserLocation()
        {
            try
            {
                return _db.Security_UserLocation
                    .Where(x => x.Security_User.CompanyId == _companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}