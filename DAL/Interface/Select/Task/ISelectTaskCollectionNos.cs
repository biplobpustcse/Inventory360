using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCollectionNos
    {
        IQueryable<Task_CollectionNos> SelectCollectionNosAll();
    }
}