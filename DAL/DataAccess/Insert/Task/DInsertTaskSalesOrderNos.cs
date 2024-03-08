using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskSalesOrderNos : IInsertTaskSalesOrderNos
    {
        private Inventory360Entities _db;
        private Task_SalesOrderNos _entity;

        public DInsertTaskSalesOrderNos(string salesOrderNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_SalesOrderNos
            {
                Id = Guid.NewGuid(),
                SalesOrderNo = salesOrderNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertSalesOrderNos()
        {
            try
            {
                _db.Task_SalesOrderNos.Add(_entity);
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