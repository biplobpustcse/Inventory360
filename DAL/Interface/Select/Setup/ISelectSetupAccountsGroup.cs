using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupAccountsGroup
    {
        IQueryable<Setup_AccountsGroup> SelectAccountsGroupAll();
    }
}
