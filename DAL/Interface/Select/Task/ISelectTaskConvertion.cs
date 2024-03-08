using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskConvertion
    {
        IQueryable<Task_Convertion> SelectTaskConvertionAll();        
    }
}