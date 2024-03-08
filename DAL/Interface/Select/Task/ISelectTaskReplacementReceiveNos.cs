using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementReceiveNos
    {
        IQueryable<Task_ReplacementReceiveNos> SelectReplacementReceiveNosAll();
    }
}