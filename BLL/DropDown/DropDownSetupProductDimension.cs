using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupProductDimension
    {
        public object SelectProductWiseDimensionByProductId(long companyId, long productId, string query)
        {
            try
            {
                ISelectSetupProductDimension iSelectSetupProductDimension = new DSelectSetupProductDimension(companyId);

                return iSelectSetupProductDimension.SelectProductDimensionAll()
                    .Where(x => x.ProductId == productId)
                    .Select(s => new CommonResultList
                    {
                        Item = ("Measurement : " + s.Setup_Measurement.Name + " # Size : " + s.Setup_Size.Name + " # Style : " + s.Setup_Style.Name + " # Color : " + s.Setup_Color.Name),
                        Value = s.ProductDimensionId.ToString()
                    })
                    .OrderBy(o => o.Item)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}