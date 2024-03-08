using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownReplacementReceive
    {       
        public object SelectReplacementReceiveNumber(long companyId, long locationId,string query, string ApprovalStatus)
        {
            try
            {
                ISelectTaskReplacementReceive iSelectTaskReplacementReceive = new DSelectTaskReplacementReceive(companyId);

                return iSelectTaskReplacementReceive.SelectTaskReplacementReceiveAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ReceiveNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(ApprovalStatus),x=> x.Approved == ApprovalStatus)
                    .OrderBy(o => o.ReceiveNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.ReceiveNo.ToString(),
                        Value = s.ReceiveId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectReplacementReceiveNumberForDropdown(long companyId, long locationId, string query)
        {
            try {
                string ApprovalStatus = "";
                return SelectReplacementReceiveNumber(companyId, locationId, query, ApprovalStatus);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}