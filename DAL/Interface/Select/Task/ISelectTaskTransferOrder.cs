using Inventory360DataModel.Task;
using Inventory360Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Interface.Select.Task
{
    public interface ISelectTaskTransferOrder
    {
        IQueryable<Task_TransferOrder> SelectTaskTransferOrderAll();
        IQueryable<Task_TransferOrder> SelectTaskTransferOrder(Guid Id);
        List<CommonProductWarehouseByLocation> SelectProductWarehouseByLocation(long companyId, long locationId, long productid, string orderId);
        object SelectProductDetailInfo(long companyId, long locationId, long productid, string orderId);
    }
}