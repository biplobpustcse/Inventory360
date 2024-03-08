using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCharge
    {
        IQueryable<Setup_Charge> SelectAllCharge();        
    }
}