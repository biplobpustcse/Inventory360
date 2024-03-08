using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReplacementClaimNos
    {
        IQueryable<Task_ReplacementClaimNos> SelectReplacementClaimNosAll();
    }
}