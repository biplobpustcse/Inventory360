using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Task
{
    public class DropDownItemRequisition
    {
        public List<CommonResultList> SelectItemRequisitionNoByCompanyId(string query, long companyId)
        {
            try
            {
                ISelectTaskItemRequisition iSelectTaskItemRequisition = new DSelectTaskItemRequisition(companyId);

                return iSelectTaskItemRequisition.SelectItemRequisitionAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.RequisitionNo.ToLower().Contains(query.ToLower()))
                    .Where(x => x.Approved.Equals("A") && !x.IsSettled)
                    .OrderBy(o => o.RequisitionNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.RequisitionNo.ToString(),
                        Value = s.RequisitionId.ToString()
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