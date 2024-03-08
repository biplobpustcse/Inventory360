using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCashBankIdentification
    {
        IQueryable<Setup_AccountsCashBankIdentification> SelectCashAndBankIdentificationAll();
    }
}
