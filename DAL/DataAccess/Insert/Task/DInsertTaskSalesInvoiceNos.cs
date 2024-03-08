using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesInvoiceNos : IInsertTaskSalesInvoiceNos
    {
        private Inventory360Entities _db;
        private Task_SalesInvoiceNos _entity;

        public DInsertTaskSalesInvoiceNos(string salesInvoiceNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesInvoiceNos
            {
                Id = Guid.NewGuid(),
                InvoiceNo = salesInvoiceNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesInvoiceNos()
        {
            try
            {
                _db.Task_SalesInvoiceNos.Add(_entity);
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