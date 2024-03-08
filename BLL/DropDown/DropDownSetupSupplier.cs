using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupSupplier
    {
        public List<CommonResultList> SelectAllSupplierByCompanyIdForDropdown(long companyId, string query)
        {
            List<CommonResultList> results = new List<CommonResultList>();
            ISelectSetupSupplier iSelectSetupSupplier = new DSelectSetupSupplier(companyId);

            results = iSelectSetupSupplier.SelectSupplierAll()
                .Where(x => x.IsActive)
                .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                    || x.Name.ToLower().Contains(query.ToLower())
                    || x.Phone.ToLower().Contains(query.ToLower()))
                .Take(10)
                .Select(s => new CommonResultList
                {
                    Item = s.Code + " # " + s.Phone + " # " + s.Name,
                    Value = s.SupplierId.ToString()
                })
                .OrderBy(o => o.Item)
                .ToList();

            if (results.Count > 0)
            {
                return results;
            }
            else
            {
                return new List<CommonResultList> {
                        new CommonResultList {
                            Item = "No record(s) found...",
                            Value = "0"
                        }
                    };
            }
        }
    }
}