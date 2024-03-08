using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskConvertionNos
    {
        IQueryable<Task_ConvertionNos> SelectConvertionNosAll();
    }
}