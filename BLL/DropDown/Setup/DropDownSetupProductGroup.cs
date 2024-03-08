using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Setup
{
    public class DropDownSetupProductGroup
    {
        public List<CommonResultList> SelectProductGroupByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectSetupProductGroup iSelectSetupProductGroup = new DSelectSetupProductGroup(companyId);

                initialList.Add(new CommonResultList { Item = "Select One...", Value = "0", IsSelected = true });

                List<CommonResultList> result = iSelectSetupProductGroup.SelectProductGroupAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()) || x.Code.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.ProductGroupId.ToString()
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