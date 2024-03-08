using Inventory360Entity;
using System.Linq;
using System;
using Inventory360DataModel.Task;
using System.Collections.Generic;

namespace DAL.Interface.Select.Stock
{
    public interface ISelectStockRMAStock
    {
        IQueryable<Stock_RMAStock> SelectRMAStockAll();
        object SelectRMAProductBySerialFromComplainReceive(long companyId, long locationId, string serial, long productId, Guid complainReceiveId);
    }
}