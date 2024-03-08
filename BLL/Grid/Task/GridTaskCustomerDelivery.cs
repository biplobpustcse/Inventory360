using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskCustomerDelivery
    {
        private CommonRecordInformation<dynamic> SelectCustomerDelivery(string query,string ComplainReceiveStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskCustomerDelivery iSelectTaskCustomerDelivery = new DSelectTaskCustomerDelivery(companyId);
                var transferOrderLists = iSelectTaskCustomerDelivery.SelectTaskCustomerDeliveryAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.DeliveryNo.ToLower().Contains(query.ToLower()) 
                    || x.Setup_Customer.Name.ToLower().Contains(query.ToLower())
                    || x.Setup_Customer.Code.ToLower().Contains(query.ToLower())
                    || x.Setup_Customer.PhoneNo.ToLower().Contains(query.ToLower())
                    )                   
                    .Select(s => new
                    {
                        s.DeliveryId,
                        s.DeliveryNo,
                        s.DeliveryDate,
                        CustomerName = s.Setup_Customer.Name,
                        CustomerCode = s.Setup_Customer.Code,
                        CustomerPhoneNo = s.Setup_Customer.PhoneNo,
                        s.TotalChargeAmount
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.DeliveryDate)
                    .ThenByDescending(t => t.DeliveryNo)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectCustomerDeliveryLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectCustomerDelivery(query, string.Empty, locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}