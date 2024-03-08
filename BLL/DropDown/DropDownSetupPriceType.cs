using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupPriceType
    {
        public List<CommonResultList> SelectPriceTypeByCompanyId(long companyId, string query)
        {
            try
            {
                ISelectSetupPriceType iSelectSetupPriceType = new DSelectSetupPriceType(companyId);

                return iSelectSetupPriceType.SelectPriceTypeAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.PriceTypeId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SelectPriceTypeIsDetailByPriceTypeIdAndCompanyId(long id, long companyId)
        {
            try
            {
                ISelectSetupPriceType iSelectSetupPriceType = new DSelectSetupPriceType(companyId);

                return iSelectSetupPriceType.SelectPriceTypeAll()
                    .Where(x => x.PriceTypeId == id)
                    .Select(s => s.IsDetailPrice)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}