using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskReceiveFinalizeDetail
    {
        IQueryable<Task_ReceiveFinalizeDetail> SelectReceiveFinalizeDetailAll();
    }
}