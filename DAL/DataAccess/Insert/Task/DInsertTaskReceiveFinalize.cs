using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReceiveFinalize : IInsertTaskReceiveFinalize
    {
        private Inventory360Entities _db;
        private Task_ReceiveFinalize _entity;

        public DInsertTaskReceiveFinalize(CommonTaskGoodsReceiveFinalize entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReceiveFinalize
            {
                FinalizeId = entity.FinalizeId,
                FinalizeNo = entity.FinalizeNo,
                FinalizeDate = entity.FinalizeDate + DateTime.Now.TimeOfDay,
                SupplierId = entity.SupplierId,
                SelectedCurrency = entity.SelectedCurrency,
                Approved = "N",
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertReceiveFinalize()
        {
            try
            {
                _db.Task_ReceiveFinalize.Add(_entity);
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