using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ManagerSetupLocation
    {
        #region Select
        public List<CommonResultList> SelectAllLocationByCompanyId(long companyId)
        {
            try
            {
                ISelectSetupLocation iSelectSetupLocation = new DSelectSetupLocation(companyId);
                return iSelectSetupLocation.SelectLocationAll()
                    .OrderBy(o => o.Name)
                        .Select(s => new CommonResultList
                        {
                            Item = s.Name.ToString(),
                            Value = s.LocationId.ToString()
                        })
                        .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectLoginLocationByCompanyId(long companyId)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                initialList.Add(new CommonResultList { Item = "Select One...", Value = "0", IsSelected = true });

                ISelectSetupLocation iSelectSetupLocation = new DSelectSetupLocation(companyId);
                List<CommonResultList> result = iSelectSetupLocation.SelectLocationAll()
                    .Where(x => x.IsLoginLocation == "Y")
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.LocationId.ToString()
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

        public List<CommonResultList> SelectWarehouseByLocationIdAndCompanyId(long locationId, long companyId)
        {
            try
            {
                List<CommonResultList> initialList = new List<CommonResultList>();
                ISelectSetupLocation iSelectSetupLocation = new DSelectSetupLocation(companyId);

                initialList.Add(new CommonResultList { Item = "N/A", Value = "0", IsSelected = true });

                List<CommonResultList> result = iSelectSetupLocation.SelectLocationAll()
                    .Where(x => x.IsWareHouse == true && x.MasterLocationId == locationId)
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.LocationId.ToString()
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

        public object SelectLocationByCompanyIdExceptOwnLocation(long companyId, long locationId)
        {
            try
            {
                ISelectSetupLocation iSelectSetupLocation = new DSelectSetupLocation(companyId);
                return iSelectSetupLocation.SelectLocationAll()
                    .Where(x => x.IsLoginLocation == "N")
                    .Where(x=>x.LocationId!= locationId)
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.LocationId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CommonResultList> SelectNonLoginLocationByCompanyId(long companyId)
        {
            try
            {
                ISelectSetupLocation iSelectSetupLocation = new DSelectSetupLocation(companyId);
                return iSelectSetupLocation.SelectLocationAll()
                    .Where(x => x.IsLoginLocation == "N")
                    .OrderBy(o => o.Name)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Name.ToString(),
                        Value = s.LocationId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}