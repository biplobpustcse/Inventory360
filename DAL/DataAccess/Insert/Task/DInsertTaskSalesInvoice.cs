using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesInvoice : IInsertTaskSalesInvoice
    {
        private Inventory360Entities _db;
        private Task_SalesInvoice _entity;

        public DInsertTaskSalesInvoice(CommonTaskSalesInvoice entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesInvoice
            {
                InvoiceId = entity.InvoiceId,
                InvoiceNo = entity.InvoiceNo,
                InvoiceDate = entity.InvoiceDate + DateTime.Now.TimeOfDay,
                CustomerId = entity.CustomerId,
                BuyerId = entity.BuyerId,
                SelectedCurrency = entity.SelectedCurrency,
                InvoiceOperationType = entity.InvoiceType,
                InvoiceDiscountType = entity.InvoiceDiscountType,
                CommissionType = entity.CommissionType,
                TermsAndConditionsId = entity.TermsAndConditionsId == 0 ? null : entity.TermsAndConditionsId,
                TermsAndConditionsDetail = entity.TermsAndConditionsDetail,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesInvoice()
        {
            try
            {
                _db.Task_SalesInvoice.Add(_entity);
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