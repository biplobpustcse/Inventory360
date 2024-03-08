using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskCollectionMapping
    {
        IQueryable<Task_CollectionMapping> SelectCollectionMappingAll();
    }
}