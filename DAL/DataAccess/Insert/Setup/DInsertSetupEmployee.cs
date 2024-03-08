using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupEmployee : IInsertSetupEmployee
    {
        private Inventory360Entities _db;
        private Setup_Employee _entity;

        public DInsertSetupEmployee(CommonSetupEmployee entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Employee
            {
                Code = entity.Code,
                Name = entity.Name,
                IsActive = entity.IsActive,
                DesignationId = entity.DesignationId,
                ContactNo = entity.ContactNo,
                Role = entity.Role,
                Email = entity.Email,
                NIDNo = entity.NIDNo,
                PassportNo = entity.PassportNo,
                AccountsId = entity.AccountsId,
                BankId = entity.BankId,
                BankAccountNo = entity.BankAccountNo,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertEmployee()
        {
            try
            {
                _db.Setup_Employee.Add(_entity);
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
