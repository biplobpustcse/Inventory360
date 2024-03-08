using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ManagerSetupCompany
    {
        public List<CommonSetupCompany> SelectAllCompany()
        {
            ISelectSetupCompany _Interface_Setup = new DSelectSetupCompany();
            return _Interface_Setup.SelectCompanyAll()
                .Select(s => new CommonSetupCompany
                {
                    CompanyId = s.CompanyId,
                    Code = s.Code,
                    Name = s.Name,
                    Address = s.Address,
                    PhoneNo = (s.PhoneNo == null ? "" : s.PhoneNo),
                    FaxNo = (s.FaxNo == null ? "" : s.FaxNo),
                    OpeningDate = s.OpeningDate
                })
                .OrderBy(o => o.Name)
                .ToList();
        }

        public DateTime SelectCompanyOpeningDate(long _CompanyId)
        {
            ISelectSetupCompany _Interface_Setup = new DSelectSetupCompany();
            return _Interface_Setup.SelectCompanyAll()
                .Where(x => x.CompanyId == _CompanyId)
                .Select(s => s.OpeningDate)
                .FirstOrDefault();
        }
    }
}