﻿using Inventory360Entity;
using DAL.Interface.Update.Task;
using System;
using System.Data.Entity;
using System.ServiceModel;

namespace DAL.DataAccess.Update.Task
{
    public class DUpdateTaskChequeInfo : IUpdateTaskChequeInfo
    {
        private Inventory360Entities _db;
        private Task_ChequeInfo _findEntity;

        public DUpdateTaskChequeInfo(Guid id)
        {
            _db = new Inventory360Entities();
            _db.Configuration.LazyLoadingEnabled = false;

            // Initialize value
            _findEntity = _db.Task_ChequeInfo.Find(id);
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool UpdateChequeInfo(string status, DateTime statusDate)
        {
            try
            {
                _findEntity.Status = status;
                _findEntity.StatusDate = statusDate + DateTime.Now.TimeOfDay;

                _db.Entry(_findEntity).State = EntityState.Modified;
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