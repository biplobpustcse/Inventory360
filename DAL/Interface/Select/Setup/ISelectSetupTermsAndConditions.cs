using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupTermsAndConditions
    {
        IQueryable<Setup_TermsAndConditions> SelectTermsAndConditionsAll();
    }
}