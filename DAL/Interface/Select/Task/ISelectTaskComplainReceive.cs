using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskComplainReceive
    {
        IQueryable<Task_ComplainReceive> SelectComplainReceiveAll();
        IQueryable<Task_ComplainReceive> SelectTaskComplainReceive(Guid Id);
        IQueryable<Task_ComplainReceiveDetail_Problem> SelectTaskComplainReceiveDetailProblemAll();
        IQueryable<Task_ComplainReceiveDetail_Problem> SelectTaskComplainReceiveDetailProblemByComRcvDtlId(Guid Id);        
        IQueryable<Task_ComplainReceive_Charge> SelectTaskComplainReceiveChargeByComRcvlId(Guid Id);
        object SelectRMAProductNameByComplainReceive(long companyId, string query, Guid complainReceiveId);
    }
}