using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupCustomer
    {
        IQueryable<Setup_Customer> SelectCustomerAll();
        IQueryable<Setup_Customer> SelectCustomerWithoutCheckingCompany();
    }
}