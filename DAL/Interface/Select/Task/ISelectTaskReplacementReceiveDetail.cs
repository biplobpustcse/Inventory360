using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementReceiveDetail
    {
        IQueryable<Task_ReplacementReceiveDetail> SelectReplacementReceiveDetailAll();
    }
}