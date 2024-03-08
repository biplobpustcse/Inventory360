using Inventory360DataModel;
using Inventory360DataModel.Setup;
using Inventory360Entity;
using DAL.Interface.Update.Setup;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Setup
{
    public class DUpdateSetupSupplier : IUpdateSetupSupplier
    {
        private Inventory360Entities _db;
        private Setup_Supplier _findEntity;

        public DUpdateSetupSupplier(long id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Setup_Supplier.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateSupplier(CommonSetupSupplier entity)
        {
            try
            {
                _findEntity.SupplierGroupId = entity.SupplierGroupId;
                _findEntity.Code = entity.Code;
                _findEntity.Name = entity.Name;
                _findEntity.Address = entity.Address;
                _findEntity.Phone = entity.Phone;
                _findEntity.Fax = entity.Fax;
                _findEntity.Email = entity.Email;
                _findEntity.URL = entity.URL;
                _findEntity.ContactPerson = entity.ContactPerson;
                _findEntity.ContactPersonMobile = entity.ContactPersonMobile;
                _findEntity.ProfessionId = entity.ProfessionId;
                _findEntity.Designation = entity.Designation;
                _findEntity.BankId = entity.BankId;
                _findEntity.BankAccountName = entity.BankAccountName;
                _findEntity.BankAccountNumber = entity.BankAccountNumber;
                _findEntity.IsActive = entity.IsActive;
                _findEntity.EditedBy = entity.EntryBy;
                _findEntity.EditedDate = DateTime.Now;

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
        public bool UpdateSupplierOpeningByPaymentInPaidAmountAsIncrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.PaidAmount = _findEntity.PaidAmount + convertedAmount.BaseAmount;
                _findEntity.Paid1Amount = _findEntity.Paid1Amount + convertedAmount.Currency1Amount;
                _findEntity.Paid2Amount = _findEntity.Paid2Amount + convertedAmount.Currency2Amount;

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
        public bool UpdateSupplierOpeningByPaymentInPaidAmountAsDecrease(CurrencyConvertedAmount convertedAmount)
        {
            try
            {
                _findEntity.PaidAmount = _findEntity.PaidAmount - convertedAmount.BaseAmount;
                _findEntity.Paid1Amount = _findEntity.Paid1Amount - convertedAmount.Currency1Amount;
                _findEntity.Paid2Amount = _findEntity.Paid2Amount - convertedAmount.Currency2Amount;

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