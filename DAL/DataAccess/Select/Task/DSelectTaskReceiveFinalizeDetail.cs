﻿using Inventory360Entity;
using DAL.Interface.Select.Task;
using System.Linq;
using System.ServiceModel;

namespace DAL.DataAccess.Select.Task
{
    public class DSelectTaskReceiveFinalizeDetail : ISelectTaskReceiveFinalizeDetail
    {
        private Inventory360Entities _db;
        private long _companyId;

        public DSelectTaskReceiveFinalizeDetail(long companyId)
        {
            _db = new Inventory360Entities();
            _companyId = companyId;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        public IQueryable<Task_ReceiveFinalizeDetail> SelectReceiveFinalizeDetailAll()
        {
            return _db.Task_ReceiveFinalizeDetail
                .Where(x => x.Task_ReceiveFinalize.CompanyId == _companyId);
        }
    }
}