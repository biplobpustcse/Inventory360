using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupSize
    {
        IQueryable<Setup_Size> SelectSizeAll();
    }
}