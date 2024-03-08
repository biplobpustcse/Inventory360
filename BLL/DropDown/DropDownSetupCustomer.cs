using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSetupCustomer
    {
        public List<CommonResultList> SelectAllCustomerByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                List<CommonResultList> results = new List<CommonResultList>();
                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);

                results = iSelectSetupCustomer.SelectCustomerAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.PhoneNo.ToLower().Contains(query.ToLower()))
                    .Take(10)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Code + " # " + s.PhoneNo + " # " + s.Name,
                        Value = s.CustomerId.ToString()
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectCustomerGroupAllByCompanyIdForDropdown(long companyId)
        {
            try
            {
                List<CommonResultList> results = new List<CommonResultList>();
                ISelectSetupCustomerGroup iSelectSetupCustomerGroup = new DSelectSetupCustomerGroup(companyId);

                results = iSelectSetupCustomerGroup.SelectCustomerGroupAll()
                    .Select(s => new CommonResultList
                    {
                        Item =  s.Name,
                        Value = s.CustomerGroupId.ToString()
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectOnlyBuyerByCompanyIdForDropdown(long companyId, string query)
        {
            try
            {
                List<CommonResultList> results = new List<CommonResultList>();
                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);

                results = iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.Type == CommonEnum.CustomerType.B.ToString())
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.PhoneNo.ToLower().Contains(query.ToLower()))
                    .Take(10)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Code + " # " + s.PhoneNo + " # " + s.Name,
                        Value = s.CustomerId.ToString()
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<CommonResultList> SelectAllCustomerByCustomerGroupForDropdown(long companyId, string query, long customerGroupId)
        {
            try
            {
                List<CommonResultList> results = new List<CommonResultList>();
                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);

                results = iSelectSetupCustomer.SelectCustomerAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.PhoneNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(customerGroupId != 0 ,x=>x.CustomerGroupId == customerGroupId)
                    .Take(10)
                    .Select(s => new CommonResultList
                    {
                        Item = s.Code + " # " + s.PhoneNo + " # " + s.Name,
                        Value = s.CustomerId.ToString()
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}