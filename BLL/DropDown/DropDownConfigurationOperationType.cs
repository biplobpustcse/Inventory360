using DAL.DataAccess.Select.Configuration;
using DAL.Interface.Select.Configuration;
using Inventory360DataModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownConfigurationOperationType
    {
        public List<CommonResultList> SelectOperationTypeForDropdown()
        {
            try
            {
                ISelectConfigurationOperationType iSelectConfigurationOperationType = new DSelectConfigurationOperationType();

                return iSelectConfigurationOperationType.SelectOperationTypeAll()
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.OperationTypeId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}