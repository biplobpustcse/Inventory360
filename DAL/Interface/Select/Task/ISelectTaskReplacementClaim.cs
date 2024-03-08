using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementClaim
    {
        IQueryable<Task_ReplacementClaim> SelectTaskReplacementClaimAll();
    }
}