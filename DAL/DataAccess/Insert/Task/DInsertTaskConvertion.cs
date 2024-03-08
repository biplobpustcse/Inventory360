using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskConvertion : IInsertTaskConvertion
    {
        private Inventory360Entities _db;
        private Task_Convertion _entity;

        public DInsertTaskConvertion(CommonTaskConvertion entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_Convertion
            {
                ConvertionId = entity.ConvertionId,
                ConvertionNo = entity.ConvertionNo,
                ConvertionDate = entity.ConvertionDate,
                ConvertionType = entity.ConvertionType,
                ConvertionFor = entity.ConvertionFor,
                ConvertionRatioId = entity.ConvertionRatioId,
                Remarks = entity.Remarks,
                Approved = "N",                
                LocationId = entity.LocationId,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now,
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertConvertion()
        {
            try
            {
                _db.Task_Convertion.Add(_entity);
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