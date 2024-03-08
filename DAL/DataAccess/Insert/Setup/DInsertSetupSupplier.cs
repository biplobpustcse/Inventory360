using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupSupplier : IInsertSetupSupplier
    {
        private Inventory360Entities _db;
        private Setup_Supplier _entity;

        public DInsertSetupSupplier(CommonSetupSupplier entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Supplier
            {
                SupplierId = entity.SupplierId,
                SupplierGroupId = entity.SupplierGroupId,
                Code = entity.Code,
                Name = entity.Name,
                Address = entity.Address,
                Phone = entity.Phone,
                Fax = entity.Fax,
                Email = entity.Email,
                URL = entity.URL,
                ContactPerson = entity.ContactPerson,
                ContactPersonMobile = entity.ContactPersonMobile,
                ProfessionId = entity.ProfessionId,
                Designation = entity.Designation,
                BankId = entity.BankId,
                BankAccountName = entity.BankAccountName,
                BankAccountNumber = entity.BankAccountNumber,
                IsActive = true,
                CompanyId = entity.CompanyId,
                LocationId = entity.LocationId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
                SelectedCurrency = entity.SelectedCurrency
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSupplier(out long id)
        {
            id = 0;
            try
            {
                _db.Setup_Supplier.Add(_entity);
                _db.SaveChanges();

                id = _entity.SupplierId;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}