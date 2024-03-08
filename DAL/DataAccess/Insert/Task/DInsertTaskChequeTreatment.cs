using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskChequeTreatment : IInsertTaskChequeTreatment
    {
        private Inventory360Entities _db;
        private Task_ChequeTreatment _entity;

        public DInsertTaskChequeTreatment(CommonTaskChequeTreatment entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ChequeTreatment
            {
                TreatmentId = entity.TreatmentId,
                ChequeInfoId = entity.ChequeInfoId,
                Status = entity.Status,
                StatusDate = entity.StatusDate + DateTime.Now.TimeOfDay,
                TreatmentBankId = entity.TreatmentBankId,
                VoucherId = entity.VoucherId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertChequeTreatment()
        {
            try
            {
                _db.Task_ChequeTreatment.Add(_entity);
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