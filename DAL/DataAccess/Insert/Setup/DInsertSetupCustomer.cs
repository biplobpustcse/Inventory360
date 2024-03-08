using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupCustomer : IInsertSetupCustomer
    {
        private Inventory360Entities _db;
        private Setup_Customer _entity;

        public DInsertSetupCustomer(CommonSetupCustomer entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_Customer
            {
                CustomerId = entity.CustomerId,
                CustomerGroupId = entity.CustomerGroupId,
                Code = entity.Code,
                Name = entity.Name,
                Address = entity.Address,
                PhoneNo = entity.PhoneNo,
                Fax = entity.Fax,
                Email = entity.Email,
                PhoneNo1 = entity.PhoneNo1,
                PhoneNo2 = entity.PhoneNo2,
                SalesPersonId = entity.SalesPersonId,
                IsCombined = entity.IsCombined,
                IsActive = true,
                Type = entity.Type,
                ContactPerson = entity.ContactPerson,
                ContactPersonMobile = entity.ContactPersonMobile,
                ProfessionId = entity.ProfessionId,
                Designation = entity.Designation,
                ReferenceName = entity.ReferenceName,
                ReferenceContactNo = entity.ReferenceContactNo,
                SupplierId = entity.SupplierId,
                CompanyId = entity.CompanyId,
                LocationId = entity.LocationId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
                SelectedCurrency = entity.SelectedCurrency,
                TransactionType = entity.TransactionType,
                IsWalkIn = entity.IsWalkIn
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomer()
        {
            try
            {
                _db.Setup_Customer.Add(_entity);
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