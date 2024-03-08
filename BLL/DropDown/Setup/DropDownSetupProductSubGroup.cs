using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Setup
{
    public class DropDownSetupProductSubGroup
    {
        public List<CommonResultList> SelectProductSubGroupByCompanyIdAndGroupId(long companyId, long productGroupId, string query)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                initialList.Add(new CommonResultList { Item = "Select One...", Value = "0", IsSelected = true });

                ISelectSetupProductSubGroup iSelectSetupProductSubGroup = new DSelectSetupProductSubGroup(companyId);
                List<CommonResultList> result = iSelectSetupProductSubGroup.SelectProductSubGroupAll()
                    .WhereIf(productGroupId != 0, x => x.ProductGroupId == productGroupId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.ProductSubGroupId.ToString()
                    })
                    .ToList();

                initialList.AddRange(result);

                return initialList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}