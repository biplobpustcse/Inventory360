using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskComplainReceiveDetail
    {
        IQueryable<Task_ComplainReceiveDetail> SelectTaskComplainReceiveDetailAll();
        IQueryable<Task_ComplainReceiveDetail_SpareProduct> SelectTaskComplainReceiveDetail_SpareProductAll();
    }
}