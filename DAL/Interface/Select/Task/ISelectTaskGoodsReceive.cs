using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskGoodsReceive
    {
        IQueryable<Task_GoodsReceive> SelectGoodsReceiveAll();
    }
}