using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupCustomer : IUpdateSetupCustomer
    {
        private Inventory360Entities _db;
        private Setup_Customer _findEntity;

        public DUpdateSetupCustomer(long id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Customer.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCustomer(CommonSetupCustomer entity)
        {
            try
            {
                _findEntity.CustomerGroupId = entity.CustomerGroupId;
                _findEntity.Code = entity.Code;
                _findEntity.Name = entity.Name;
                _findEntity.Address = entity.Address;
                _findEntity.PhoneNo = entity.PhoneNo;
                _findEntity.Fax = entity.Fax;
                _findEntity.Email = entity.Email;
                _findEntity.PhoneNo1 = entity.PhoneNo1;
                _findEntity.PhoneNo2 = entity.PhoneNo2;
                _findEntity.SalesPersonId = entity.SalesPersonId;
                _findEntity.IsCombined = entity.IsCombined;
                _findEntity.IsActive = entity.IsActive;
                _findEntity.Type = entity.Type;
                _findEntity.ContactPerson = entity.ContactPerson;
                _findEntity.ContactPersonMobile = entity.ContactPersonMobile;
                _findEntity.ProfessionId = entity.ProfessionId;
                _findEntity.Designation = entity.Designation;
                _findEntity.ReferenceName = entity.ReferenceName;
                _findEntity.ReferenceContactNo = entity.ReferenceContactNo;
                _findEntity.SupplierId = entity.SupplierId;
                _findEntity.EditedBy = entity.EntryBy;
                _findEntity.EditedDate = DateTime.Now;
                _findEntity.TransactionType = entity.TransactionType;
                _findEntity.IsWalkIn = entity.IsWalkIn;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCustomerOpeningByCollectionInCollectedAmountAsIncrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.CollectedAmount = _findEntity.CollectedAmount + convertedAmount.BaseAmount;
                _findEntity.Collected1Amount = _findEntity.Collected1Amount + convertedAmount.Currency1Amount;
                _findEntity.Collected2Amount = _findEntity.Collected2Amount + convertedAmount.Currency2Amount;

                _db.Entry(_findEntity).State = EntityState.Modified;
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateCustomerOpeningByCollectionInCollectedAmountAsDecrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.CollectedAmount = _findEntity.CollectedAmount - convertedAmount.BaseAmount;
                _findEntity.Collected1Amount = _findEntity.Collected1Amount - convertedAmount.Currency1Amount;
                _findEntity.Collected2Amount = _findEntity.Collected2Amount - convertedAmount.Currency2Amount;

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