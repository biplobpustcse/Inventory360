using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.DropDown.Task
{
    public class DropDownSalesOrderNo
    {
        public object SelectSalesOrderNoForDeliveryChallanByCompanyIdAndLocationId(string query, long companyId, long locationId)
        {
            try
            {
                ISelectTaskSalesOrder iSelectTaskSalesOrder = new DSelectTaskSalesOrder(companyId);

                return iSelectTaskSalesOrder.SelectSalesOrderAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.SalesOrderNo.ToLower().Contains(query.ToLower()))
                    .Where(x => x.DeliveryFromId == locationId && x.Approved.Equals("A") && x.IsSettledByChallan == false)
                    .OrderBy(o => o.SalesOrderNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.SalesOrderNo.ToString(),
                        Value = s.SalesOrderId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}