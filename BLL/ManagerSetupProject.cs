using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ManagerSetupProject
    {
        public List<CommonResultList> SelectProject(long _CompanyId)
        {
            List<CommonResultList> _List = new List<CommonResultList>();
            try
            {
                ISelectSetupProject _Interface_Setup = new DSelectSetupProject(_CompanyId);
                var projectList = _Interface_Setup.SelectProjectAll()
                    .Where(x => x.IsActive == "Y")
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.ProjectId.ToString()
                    })
                    .ToList();

                return projectList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}