using Inventory360DataModel;
using Inventory360DataModel.Setup;
using BLL.Common;
using DAL.DataAccess.Select.Configuration;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.Select.Configuration;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Grid.Setup
{
    public class GridSetupCustomer
    {
        public CommonRecordInformation<CommonSetupCustomer> SelectCustomerForGrid(string query, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectConfigurationCode iSelectConfigurationCode = new DSelectConfigurationCode(companyId);
                var codeInfo = iSelectConfigurationCode.SelectCodeAll()
                    .Where(x => x.FormName.ToLower().Equals("Customer".ToLower()))
                    .Select(s => new
                    {
                        s.IsAutoCode,
                        s.IsCodeVisible
                    })
                    .FirstOrDefault();

                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                var customerLists = iSelectSetupCustomer.SelectCustomerAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.PhoneNo.ToLower().Contains(query.ToLower()))
                    .Select(s => new CommonSetupCustomer
                    {
                        CustomerId = s.CustomerId,
                        Code = s.Code,
                        Name = s.Name,
                        PhoneNo = s.PhoneNo,
                        IsActive = s.IsActive
                    });

                var pagedData = new CommonRecordInformation<CommonSetupCustomer>();
                pagedData.TotalNumberOfRecords = customerLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.OthersData = new
                {
                    IsAutoCode = codeInfo == null ? false : codeInfo.IsAutoCode,
                    IsCodeVisible = codeInfo == null ? false : codeInfo.IsCodeVisible
                };
                pagedData.Data = customerLists
                    .OrderBy(o => o.Name)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonSetupCustomerFinancialInfo SelectCustomerFinancialInfoByCompanyIdAndCustomerId(long id, long companyId)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                var customerItem = iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.CustomerId == id)
                    .Select(s => new CommonSetupCustomerFinancialInfo
                    {
                        Code = s.Code,
                        Name = s.Name,
                        BaseCurrency = s.Setup_Company.BaseCurrency,
                        SelectedCurrency = s.SelectedCurrency,
                        ExchangeRate = currencyInfo.BaseCurrency == s.SelectedCurrency ? 1 : (currencyInfo.Currency1 == s.SelectedCurrency ? s.Currency1Rate : s.Currency2Rate),
                        OpeningBalance = currencyInfo.BaseCurrency == s.SelectedCurrency ? s.OpeningBalance : (currencyInfo.Currency1 == s.SelectedCurrency ? s.OpeningBalance1 : s.OpeningBalance2),
                        OpeningDate = s.OpeningDate,
                        ChequeDishonourBalance = currencyInfo.BaseCurrency == s.SelectedCurrency ? s.ChequeDishonourBalance : (currencyInfo.Currency1 == s.SelectedCurrency ? s.ChequeDishonourBalance1 : s.ChequeDishonourBalance2),
                        ChequeDishonourOpeningDate = s.ChequeDishonourOpeningDate,
                        CreditLimit = currencyInfo.BaseCurrency == s.SelectedCurrency ? s.CreditLimit : (currencyInfo.Currency1 == s.SelectedCurrency ? s.CreditLimit1 : s.CreditLimit2),
                        LedgerLimit = currencyInfo.BaseCurrency == s.SelectedCurrency ? s.LedgerLimit : (currencyInfo.Currency1 == s.SelectedCurrency ? s.LedgerLimit1 : s.LedgerLimit2),
                        CreditAllowedDays = s.CreditAllowedDays,
                        IsLocked = s.IsLocked,
                        IsRMALocked = s.IsRMALocked,
                        AccountsId = s.AccountsId,
                        AccountsName = s.AccountsId == null ? string.Empty : s.Setup_Accounts.Name
                    })
                    .FirstOrDefault();

                if (customerItem != null)
                {
                    return customerItem;
                }
                else
                {
                    throw new Exception("No record found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonSetupCustomer SelectCustomerByCustomerIdForEdit(long id, long companyId)
        {
            try
            {
                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                var customerItem = iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.CustomerId == id)
                    .Select(s => new CommonSetupCustomer
                    {
                        CustomerGroupId = s.CustomerGroupId,
                        Code = s.Code,
                        Name = s.Name,
                        Address = s.Address,
                        PhoneNo = s.PhoneNo,
                        Fax = s.Fax,
                        Email = s.Email,
                        PhoneNo1 = s.PhoneNo1,
                        PhoneNo2 = s.PhoneNo2,
                        SalesPersonId = s.SalesPersonId,
                        SalesPersonName = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        IsCombined = s.IsCombined,
                        IsActive = s.IsActive,
                        Type = s.Type,
                        ContactPerson = s.ContactPerson,
                        ContactPersonMobile = s.ContactPersonMobile,
                        ProfessionId = s.ProfessionId,
                        Designation = s.Designation,
                        ReferenceName = s.ReferenceName,
                        ReferenceContactNo = s.ReferenceContactNo,
                        TransactionType = s.TransactionType,
                        IsWalkIn = s.IsWalkIn
                    })
                    .FirstOrDefault();

                if (customerItem != null)
                {
                    return customerItem;
                }
                else
                {
                    throw new Exception("No record found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonSetupCustomerShortInfo SelectCustomerShortInfoByCustomerId(long id, long companyId)
        {
            try
            {
                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                var customerItem = iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.CustomerId == id)
                    .Select(s => new CommonSetupCustomerShortInfo
                    {
                        GroupName = s.Setup_CustomerGroup.Name,
                        ContactNo = s.PhoneNo,
                        Address = s.Address,
                        SalesPersonId = s.SalesPersonId,
                        SalesPersonName = s.Setup_Employee.Code + " # " + s.Setup_Employee.ContactNo + " # " + s.Setup_Employee.Name,
                        IsCashTypeTransaction = (s.TransactionType == "Cash"),
                        IsWalkIn = s.IsWalkIn
                    })
                    .FirstOrDefault();

                if (customerItem != null)
                {
                    return customerItem;
                }
                else
                {
                    throw new Exception("No record found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectAllCustomer(string query, long groupId, long companyId)
        {
            try
            {
                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                return iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.IsActive)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.PhoneNo.ToLower().Contains(query.ToLower())
                        || x.Address.ToLower().Contains(query.ToLower()))
                    .WhereIf(groupId > 0, x => x.CustomerGroupId == groupId)
                    .Select(s => new
                    {
                        isSelected = false,
                        Group = s.Setup_CustomerGroup.Name,
                        Id = s.CustomerId,
                        s.Code,
                        s.Name,
                        s.Address,
                        s.PhoneNo,
                        Particulars = string.Empty,
                        Amount = 0
                    })
                    .OrderBy(o => new { o.Group, o.Name })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}