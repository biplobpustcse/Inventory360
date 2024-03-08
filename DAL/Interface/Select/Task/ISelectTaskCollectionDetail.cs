using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCollectionDetail
    {
        IQueryable<Task_CollectionDetail> SelectCollectionDetailAll();
    }
}