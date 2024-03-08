using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupAccountsSubsidiary
    {
        IQueryable<Setup_AccountsSubsidiary> SelectAccountsSubsidiaryAll();
    }
}
