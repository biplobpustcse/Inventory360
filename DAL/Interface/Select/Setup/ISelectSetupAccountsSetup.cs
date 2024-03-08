using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupAccountsSetup
    {
        IQueryable<Setup_Accounts> SelectAccountsSetupAll();
    }
}
