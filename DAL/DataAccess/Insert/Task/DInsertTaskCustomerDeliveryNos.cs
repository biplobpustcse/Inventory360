using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskCustomerDeliveryNos : IInsertTaskCustomerDeliveryNos
    {
        private Inventory360Entities _db;
        private Task_CustomerDeliveryNos _entity;

        public DInsertTaskCustomerDeliveryNos(string DeliveryNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_CustomerDeliveryNos
            {
                Id = Guid.NewGuid(),
                DeliveryNo = DeliveryNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomerDeliveryNos()
        {
            try
            {
                _db.Task_CustomerDeliveryNos.Add(_entity);
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