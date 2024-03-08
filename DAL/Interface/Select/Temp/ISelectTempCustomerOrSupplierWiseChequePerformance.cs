using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Temp
{
    public interface ISelectTempCustomerOrSupplierWiseChequePerformance
    {
        IQueryable<TempCustomerOrSupplierWiseChequePerformance> SelectTempCustomerOrSupplierWiseChequePerformance();
    }
}