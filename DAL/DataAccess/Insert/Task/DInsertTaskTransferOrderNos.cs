using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskTransferOrderNos : IInsertTaskTransferOrderNos
    {
        private Inventory360Entities _db;
        private Task_TransferOrderNos _entity;

        public DInsertTaskTransferOrderNos(string finalizeNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_TransferOrderNos
            {
                Id = Guid.NewGuid(),
                OrderNo = finalizeNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertTaskTransferOrderNos()
        {
            try
            {
                _db.Task_TransferOrderNos.Add(_entity);
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