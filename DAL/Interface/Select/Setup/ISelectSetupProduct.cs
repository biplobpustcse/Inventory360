using Inventory360Entity;
using System.Linq;

namespace DAL.Interface.Select.Setup
{
    public interface ISelectSetupProduct
    {
        IQueryable<Setup_Product> SelectProductAll();
        IQueryable<Setup_Product> SelectProductWithoutCheckingCompany();
        object SelectProductAvailableSerial(long companyId, long locationId, long productid, string orderId);
        object SelectProductWarehouseByLocationForSerialProduct(long companyId, long locationId, long productid, string orderId);
        object SelectWarehouseBasedSerialNo(long companyId, long locationId, long productid, long warehouseId, string orderId);
        object SelectProductStockInReferenceInfo(long companyId, long productId, string serial,long supplierId);
    }
}