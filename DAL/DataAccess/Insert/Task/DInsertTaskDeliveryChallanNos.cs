using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskDeliveryChallanNos : IInsertTaskDeliveryChallanNos
    {
        private Inventory360Entities _db;
        private Task_DeliveryChallanNos _entity;

        public DInsertTaskDeliveryChallanNos(string challanNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_DeliveryChallanNos
            {
                Id = Guid.NewGuid(),
                ChallanNo = challanNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertDeliveryChallanNos()
        {
            try
            {
                _db.Task_DeliveryChallanNos.Add(_entity);
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