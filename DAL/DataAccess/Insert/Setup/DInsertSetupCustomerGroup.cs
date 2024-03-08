﻿using Inventory360DataModel;
using Inventory360Entity;
using DAL.Interface.Insert.Setup;
using System;
using System.ServiceModel;

namespace DAL.DataAccess.Insert.Setup
{
    public class DInsertSetupCustomerGroup : IInsertSetupCustomerGroup
    {
        private Inventory360Entities _db;
        private Setup_CustomerGroup _entity;

        public DInsertSetupCustomerGroup(CommonSetupCustomerGroup entity)
        {
            _db = new Inventory360Entities();
            _entity = new Setup_CustomerGroup
            {
                Name = entity.Name,
                CompanyId = entity.CompanyId,
                EntryBy = entity.EntryBy,
                EntryDate = DateTime.Now
            };
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public bool InsertCustomerGroup()
        {
            try
            {
                _db.Setup_CustomerGroup.Add(_entity);
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
