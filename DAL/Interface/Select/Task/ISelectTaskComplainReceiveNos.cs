using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskComplainReceiveNos
    {
        IQueryable<Task_ComplainReceiveNos> SelectComplainReceiveNosAll();
    }
}