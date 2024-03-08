using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360Entity;
using DAL.Interface.Insert.Task;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Task
{
    public class DInsertTaskReplacementClaim : IInsertTaskReplacementClaim
    {
        private Inventory360Entities _db;
        private Task_ReplacementClaim _entity;

        public DInsertTaskReplacementClaim(CommonReplacementClaim entity)
        {
            _db = new Inventory360Entities();
            _entity = new Task_ReplacementClaim
            {
                ClaimId = entity.ClaimId,
                ClaimNo = entity.ClaimNo,
                ClaimDate = (DateTime)MyConversion.ConvertDateStringToDate(entity.ClaimDate) + DateTime.Now.TimeOfDay,
                SelectedCurrency = entity.SelectedCurrency,
                Currency1Rate = entity.Currency1Rate,
                Currency2Rate = entity.Currency2Rate,
                SupplierId = entity.SupplierId,
                RequestedBy = entity.RequestedBy,
                IsSettledByReplacementReceive = entity.IsSettledByReplacementReceive,
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
        public bool InsertReplacementClaim()
        {
            try
            {
                _db.Task_ReplacementClaim.Add(_entity);
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