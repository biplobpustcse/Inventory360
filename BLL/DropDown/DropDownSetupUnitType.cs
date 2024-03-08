using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupUnitType
    {
        public List<CommonResultList> SelectUnitTypeByCompanyId(long companyId, string query)
        {
            try
            {
                ISelectSetupUnitType iSelectSetupUnitType = new DSelectSetupUnitType(companyId);

                return iSelectSetupUnitType.SelectUnitTypeAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.UnitTypeId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectProductWiseUnitTypeByProductId(long companyId, long productId, string query)
        {
            try
            {
                List<CommonResultList> items = new List<CommonResultList>();
                ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);

                var selectedProduct = iSelectSetupProduct.SelectProductAll()
                    .Where(x => x.ProductId == productId)
                    .Select(s => new
                    {
                        PrimaryUnitTypeId = s.PrimaryUnitTypeId,
                        PrimaryUnitTypeName = s.Setup_UnitType.Name,
                        SecondaryUnitTypeId = s.SecondaryUnitTypeId,
                        SecondaryUnitTypeName = s.Setup_UnitType1.Name,
                        TertiaryUnitTypeId = s.TertiaryUnitTypeId,
                        TertiaryUnitTypeName = s.Setup_UnitType2.Name
                    })
                    .FirstOrDefault();

                items.Add(new CommonResultList { Item = selectedProduct.PrimaryUnitTypeName, Value = selectedProduct.PrimaryUnitTypeId.ToString(), IsSelected = true });

                if (!string.IsNullOrEmpty(selectedProduct.SecondaryUnitTypeName))
                {
                    items.Add(new CommonResultList { Item = selectedProduct.SecondaryUnitTypeName, Value = selectedProduct.SecondaryUnitTypeId.ToString() });
                }

                if (!string.IsNullOrEmpty(selectedProduct.TertiaryUnitTypeName))
                {
                    items.Add(new CommonResultList { Item = selectedProduct.TertiaryUnitTypeName, Value = selectedProduct.TertiaryUnitTypeId.ToString() });
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}