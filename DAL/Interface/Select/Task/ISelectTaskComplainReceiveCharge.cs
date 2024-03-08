using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskComplainReceiveCharge
    {
        IQueryable<Task_ComplainReceive_Charge> SelectTaskComplainReceiveChargeAll();
    }
}