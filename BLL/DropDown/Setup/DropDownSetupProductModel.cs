using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown.Setup
{
    public class DropDownSetupProductModel
    {
        public List<CommonResultList> SelectProductModelForDropdown(long groupId, long subGroupId, long categoryId, long brandId, long companyId)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                initialList.Add(new CommonResultList { Item = "Select One...", Value = "", IsSelected = true });

                List<CommonResultList> result = iSelectSetupProduct.SelectProductAll()
                    .WhereIf(groupId > 0, x => x.ProductGroupId == groupId)
                    .WhereIf(subGroupId > 0, x => x.ProductSubGroupId == subGroupId)
                    .WhereIf(categoryId > 0, x => x.ProductCategoryId == categoryId)
                    .WhereIf(brandId > 0, x => x.BrandId == brandId)
                    .OrderBy(o => o.Model)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Model.ToString(),
                        Value = s.Model.ToString()
                    })
                    .Distinct()
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