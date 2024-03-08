using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementClaimDetail
    {
        IQueryable<Task_ReplacementClaimDetail> SelectReplacementClaimDetailAll();
    }
}