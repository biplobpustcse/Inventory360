using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskPurchaseOrderNos : IInsertTaskPurchaseOrderNos
    {
        private Inventory360Entities _db;
        private Task_PurchaseOrderNos _entity;

        public DInsertTaskPurchaseOrderNos(string orderNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_PurchaseOrderNos
            {
                Id = Guid.NewGuid(),
                OrderNo = orderNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertPurchaseOrderNos()
        {
            try
            {
                _db.Task_PurchaseOrderNos.Add(_entity);
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