using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskGoodsReceiveNos
    {
        IQueryable<Task_GoodsReceiveNos> SelectGoodsReceiveNosAll();
    }
}