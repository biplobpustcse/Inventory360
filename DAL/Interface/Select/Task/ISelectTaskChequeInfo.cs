using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskChequeInfo
    {
        IQueryable<Task_ChequeInfo> SelectChequeInfoAll();
    }
}