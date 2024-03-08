using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementReceive
    {
        IQueryable<Task_ReplacementReceive> SelectTaskReplacementReceiveAll();        
    }
}