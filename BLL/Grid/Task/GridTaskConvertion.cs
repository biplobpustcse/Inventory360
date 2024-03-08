using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskConvertion
    {
        private CommonRecordInformation<dynamic> SelectConvertion(string query,string approvalStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskConvertion iSelectTaskConvertion = new DSelectTaskConvertion(companyId);
                var transferOrderLists = iSelectTaskConvertion.SelectTaskConvertionAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ConvertionNo.ToLower().Contains(query.ToLower())    
                    )
                    .WhereIf(!string.IsNullOrEmpty(approvalStatus),x=>x.Approved == approvalStatus)
                    .Select(s => new
                    {
                        s.ConvertionId,
                        s.ConvertionNo,
                        s.ConvertionDate,
                        s.ApprovedDate,                        
                        s.Remarks
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.ConvertionDate)
                    .ThenByDescending(t => t.ConvertionNo)
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
        public object SelectConvertionList(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectConvertion(query,string.Empty, locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectUnApprovedConvertionLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectConvertion(query, "N", locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}