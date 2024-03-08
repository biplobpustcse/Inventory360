using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskItemRequisitionNos : IInsertTaskItemRequisitionNos
    {
        private Inventory360Entities _db;
        private Task_ItemRequisitionNos _entity;

        public DInsertTaskItemRequisitionNos(string requisitionNo, long year, long companyId)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ItemRequisitionNos
            {
                Id = Guid.NewGuid(),
                RequisitionNo = requisitionNo,
                Year = year,
                CompanyId = companyId
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertItemRequisitionNos()
        {
            try
            {
                _db.Task_ItemRequisitionNos.Add(_entity);
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