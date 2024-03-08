using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCompany
    {
        IQueryable<Setup_Company> SelectCompanyAll();
    }
}