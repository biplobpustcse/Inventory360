using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskReplacementReceive
    {
        private CommonRecordInformation<dynamic> SelectReplacementReceive(string query,string replacementReceiveStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskReplacementReceive iSelectTaskReplacementReceive = new DSelectTaskReplacementReceive(companyId);
                var transferOrderLists = iSelectTaskReplacementReceive.SelectTaskReplacementReceiveAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ReceiveNo.ToLower().Contains(query.ToLower())
                    
                    //|| x.Setup_Supplier.Name.ToLower().Contains(query.ToLower())
                    //|| x.Setup_Supplier.Code.ToLower().Contains(query.ToLower())
                    //|| x.Setup_Supplier.PhoneNo.ToLower().Contains(query.ToLower())
                    )
                    .WhereIf(!string.IsNullOrEmpty(replacementReceiveStatus),x=>x.Approved == replacementReceiveStatus)
                    .Select(s => new
                    {
                        s.ReceiveId,
                        s.ReceiveNo,
                        s.ReceiveDate,
                        //SupplierName = s.Setup_Supplier.Name,
                        //SupplierCode = s.Setup_Supplier.Code,
                        //SupplierPhoneNo = s.Setup_Customer.PhoneNo
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.ReceiveDate)
                    .ThenByDescending(t => t.ReceiveNo)
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
        public object SelectReplacementReceiveLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectReplacementReceive(query, string.Empty, locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}