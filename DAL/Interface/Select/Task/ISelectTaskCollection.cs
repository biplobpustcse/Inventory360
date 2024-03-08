using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCollection
    {
        IQueryable<Task_Collection> SelectCollectionAll();
    }
}