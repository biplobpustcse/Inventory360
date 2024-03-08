using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Task
{
    public class DropDownPurchaseOrder
    {
        public List<CommonResultList> SelectPurchaseOrderNoByCompanyId(string query, long companyId)
        {
            try
            {
                ISelectTaskPurchaseOrder iSelectTaskPurchaseOrder = new DSelectTaskPurchaseOrder(companyId);

                return iSelectTaskPurchaseOrder.SelectPurchaseOrderAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.OrderNo.ToLower().Contains(query.ToLower()))
                    .Where(x => x.Approved.Equals("A") && !x.IsSettledByGoodsReceive)
                    .OrderBy(o => o.OrderNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.OrderNo.ToString(),
                        Value = s.OrderId.ToString()
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