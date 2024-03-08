using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskGoodsReceiveDetail
    {
        IQueryable<Task_GoodsReceiveDetail> SelectGoodsReceiveDetailAll();
    }
}