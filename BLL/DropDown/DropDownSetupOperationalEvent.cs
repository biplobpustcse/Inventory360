using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupOperationalEvent
    {
        public List<CommonResultList> SelectOperationalEventByCompanyId(string query)
        {
            try
            {
                ISelectSetupOperationalEvent iSelectSetupOperationalEvent = new DSelectSetupOperationalEvent();

                var lists = iSelectSetupOperationalEvent.SelectOperationalEventAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.EventName.ToLower().Contains(query.ToLower()))
                    .GroupBy(g => g.EventName)
                    .Select(s => s.FirstOrDefault());

                return lists.Select(s => new CommonResultList
                {
                    Item = s.EventName,
                    Value = s.EventName
                })
                .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectOperationalSubEventByCompanyId(string eventName, string query)
        {
            try
            {
                ISelectSetupOperationalEvent iSelectSetupOperationalEvent = new DSelectSetupOperationalEvent();

                var lists = iSelectSetupOperationalEvent.SelectOperationalEventAll()
                    .Where(x => x.EventName.ToLower().Equals(eventName.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.SubEventName.ToLower().Contains(query.ToLower()))
                    .GroupBy(g => g.SubEventName)
                    .Select(s => s.FirstOrDefault());

                return lists.Select(s => new CommonResultList
                {
                    Item = s.SubEventName,
                    Value = s.SubEventName
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