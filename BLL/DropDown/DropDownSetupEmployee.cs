using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupEmployee
    {
        private List<CommonResultList> SelectAllEmployee(long companyId, string role, string query)
        {
            List<CommonResultList> results = new List<CommonResultList>();
            ISelectSetupEmployee iSelectSetupEmployee = new DSelectSetupEmployee(companyId);

            results = iSelectSetupEmployee.SelectEmployeeAll()
                .Where(x => x.IsActive)
                .WhereIf(!string.IsNullOrEmpty(role), x => x.Role == role)
                .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                    || x.Name.ToLower().Contains(query.ToLower())
                    || x.ContactNo.ToLower().Contains(query.ToLower()))
                .Take(10)
                .Select(s => new CommonResultList
                {
                    Item = s.Code + " # " + s.ContactNo + " # " + s.Name,
                    Value = s.EmployeeId.ToString()
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

        public List<CommonResultList> SelectEmployeeSalesPersonByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                return SelectAllEmployee(companyId, CommonEnum.EmployeeRole.SP.ToString(), query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectAllEmployeeByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                return SelectAllEmployee(companyId, string.Empty, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}