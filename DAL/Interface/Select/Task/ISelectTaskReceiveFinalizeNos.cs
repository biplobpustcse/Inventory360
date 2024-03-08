using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReceiveFinalizeNos
    {
        IQueryable<Task_ReceiveFinalizeNos> SelectReceiveFinalizeNosAll();
    }
}