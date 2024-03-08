using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskGoodsReceiveDetailSerial
    {
        IQueryable<Task_GoodsReceiveDetailSerial> SelectGoodsReceiveDetailSerialAll();
    }
}