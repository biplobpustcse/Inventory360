using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupTransport
    {
        public List<CommonResultList> SelectTransportByCompanyId(long companyId, string query)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectSetupTransport iSelectSetupTransport = new DSelectSetupTransport(companyId);

                initialList.Add(new CommonResultList { Item = "N/A", Value = "0", IsSelected = true });

                List<CommonResultList> result = iSelectSetupTransport.SelectTransportAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Name.ToLower().Contains(query.ToLower()))
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.TransportId.ToString()
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