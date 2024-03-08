using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Configuration
{
    public interface ISelectConfigurationPaymentMode
    {
        IQueryable<Configuration_PaymentMode> SelectPaymentModeAll();
    }
}