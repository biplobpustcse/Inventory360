using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupEmployee : IUpdateSetupEmployee
    {
        private Inventory360Entities _db;
        private Setup_Employee _findEntity;

        public DUpdateSetupEmployee(CommonSetupEmployee entity)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Employee.Find(entity.EmployeeId);
            _findEntity.Name = entity.Name;
            _findEntity.IsActive = entity.IsActive;
            _findEntity.DesignationId = entity.DesignationId;
            _findEntity.ContactNo = entity.ContactNo;
            _findEntity.Role = entity.Role;
            _findEntity.Email = entity.Email;
            _findEntity.NIDNo = entity.NIDNo;
            _findEntity.PassportNo = entity.PassportNo;
            _findEntity.AccountsId = entity.AccountsId;
            _findEntity.BankId = entity.BankId;
            _findEntity.BankAccountNo = entity.BankAccountNo;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateEmployee()
        {
            try
            {
                _db.Entry(_findEntity).State = EntityState.Modified;
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
