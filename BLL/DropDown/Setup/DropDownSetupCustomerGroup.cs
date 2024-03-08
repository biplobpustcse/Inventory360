using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Setup
{
    public class DropDownSetupCustomerGroup
    {
        public List<CommonResultList> SelectCustomerGroupByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                initialList.Add(new CommonResultList { Item = "Select One...", Value = string.Empty, IsSelected = true });

                ISelectSetupCustomerGroup iSelectSetupCustomerGroup = new DSelectSetupCustomerGroup(companyId);
                List<CommonResultList> result = iSelectSetupCustomerGroup.SelectCustomerGroupAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.CustomerGroupId.ToString()
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