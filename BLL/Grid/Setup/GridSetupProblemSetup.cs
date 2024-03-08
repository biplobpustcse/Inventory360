using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Grid.Setup
{
    public class GridSetupProblemSetup
    {
        private object GetAllProblem(string eventName, string subEventName,long companyId)
        {
            try
            {
                ISelectSetupProblem iSelectSetupProblem = new DSelectSetupProblem(companyId);
                var problemLists = iSelectSetupProblem.SelectProblemAll()
                    .Where(x=>x.Configuration_OperationalEvent.EventName.Equals(eventName)
                        && x.Configuration_OperationalEvent.SubEventName.Equals(subEventName))
                    .Select(s => new
                    {
                        isSelected = false,
                        s.ProblemId,
                        s.Name,
                        Note = ""
                    });
                return problemLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private CommonRecordInformation<dynamic> SelectProblem(string query, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectSetupProblem iSelectSetupProblem = new DSelectSetupProblem(companyId);
                var collectionLists = iSelectSetupProblem.SelectProblemAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .Select(s => new
                    {
                        s.ProblemId,
                        s.Name,
                        EventName = s.Configuration_OperationalEvent.EventName,
                        SubEventName = s.Configuration_OperationalEvent.SubEventName
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = collectionLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = collectionLists
                    .OrderBy(o => new { o.EventName, o.SubEventName, o.Name })
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

        public object SelectProblemLists(string query, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectProblem(query, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetProblemForComplainReceive(long companyId)
        {
            try
            {
                return GetAllProblem(CommonEnum.OperationalEvent.RMA.ToString(), CommonEnum.OperationalSubEvent.ComplainReceive.ToString(), companyId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}