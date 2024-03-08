using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupBank
    {
        IQueryable<Setup_Bank> SelectBankAll();
        IQueryable<Setup_Bank> SelectBankWithoutCheckingCompany();
    }
}
