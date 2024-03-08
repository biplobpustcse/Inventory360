using Inventory360Entity;
using System;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskConvertionDetail
    {
        IQueryable<Task_ConvertionDetail> SelectTaskConvertionDetailAll();        
    }
}