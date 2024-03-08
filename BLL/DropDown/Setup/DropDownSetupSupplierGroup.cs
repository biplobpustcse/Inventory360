using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Setup
{
    public class DropDownSetupSupplierGroup
    {
        public List<CommonResultList> SelectSupplierGroupByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                initialList.Add(new CommonResultList { Item = "Select One...", Value = string.Empty, IsSelected = true });

                ISelectSetupSupplierGroup iSelectSetupSupplierGroup = new DSelectSetupSupplierGroup(companyId);
                List<CommonResultList> result = iSelectSetupSupplierGroup.SelectSupplierGroupAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.SupplierGroupId.ToString()
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
