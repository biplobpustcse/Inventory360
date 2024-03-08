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
    public class GridSetupSupplier
    {
        public CommonRecordInformation<CommonSetupSupplier> SelectSupplierByCompanyIdForGrid(string query, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectConfigurationCode iSelectConfigurationCode = new DSelectConfigurationCode(companyId);
                var codeInfo = iSelectConfigurationCode.SelectCodeAll()
                    .Where(x => x.FormName.ToLower().Equals("Supplier".ToLower()))
                    .Select(s => new
                    {
                        s.IsAutoCode,
                        s.IsCodeVisible
                    })
                    .FirstOrDefault();

                ISelectSetupSupplier iSelectSetupSupplier = new DSelectSetupSupplier(companyId);
                var supplierGroupLists = iSelectSetupSupplier.SelectSupplierAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.Phone.ToLower().Contains(query.ToLower()))
                    .Select(s => new CommonSetupSupplier
                    {
                        SupplierId = s.SupplierId,
                        Code = s.Code,
                        Name = s.Name,
                        Phone = s.Phone,
                        IsActive = s.IsActive
                    })
                    .ToList();

                var pagedData = new CommonRecordInformation<CommonSetupSupplier>();
                pagedData.TotalNumberOfRecords = supplierGroupLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.OthersData = new
                {
                    IsAutoCode = codeInfo == null ? false : codeInfo.IsAutoCode,
                    IsCodeVisible = codeInfo == null ? false : codeInfo.IsCodeVisible
                };
                pagedData.Data = supplierGroupLists
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

        public object SelectAllSupplier(string query, long groupId, long companyId)
        {
            try
            {
                ISelectSetupSupplier iSelectSetupSupplier = new DSelectSetupSupplier(companyId);
                return iSelectSetupSupplier.SelectSupplierAll()
                    .Where(x => x.IsActive)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Code.ToLower().Contains(query.ToLower())
                        || x.Name.ToLower().Contains(query.ToLower())
                        || x.Phone.ToLower().Contains(query.ToLower())
                        || x.Address.ToLower().Contains(query.ToLower()))
                    .WhereIf(groupId > 0, x => x.SupplierGroupId == groupId)
                    .Select(s => new
                    {
                        isSelected = false,
                        Group = s.Setup_SupplierGroup.Name,
                        Id = s.SupplierId,
                        s.Code,
                        s.Name,
                        s.Address,
                        PhoneNo = s.Phone,
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

        public CommonSetupSupplierFinancialInfo SelectSupplierFinancialInfoByCompanyIdAndSupplierId(long id, long companyId)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                ISelectSetupSupplier iSelectSetupSupplier = new DSelectSetupSupplier(companyId);
                var supplierItem = iSelectSetupSupplier.SelectSupplierAll()
                    .Where(x => x.SupplierId == id)
                    .Select(s => new CommonSetupSupplierFinancialInfo
                    {
                        Code = s.Code,
                        Name = s.Name,
                        OpeningDate = s.OpeningDate,
                        AccountsId = s.AccountsId,
                        AccountsName = s.AccountsId == null ? string.Empty : s.Setup_Accounts.Name,
                        SelectedCurrency = s.SelectedCurrency,
                        ExchangeRate = currencyInfo.BaseCurrency == s.SelectedCurrency ? 1 : (currencyInfo.Currency1 == s.SelectedCurrency ? s.Currency1Rate : s.Currency2Rate),
                        OpeningBalance = currencyInfo.BaseCurrency == s.SelectedCurrency ? s.OpeningBalance : (currencyInfo.Currency1 == s.SelectedCurrency ? s.OpeningBalance1 : s.OpeningBalance2),
                    })
                    .FirstOrDefault();

                if (supplierItem != null)
                {
                    return supplierItem;
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

        public CommonSetupSupplier SelectSupplierByCompanyIdAndSupplierIdForEdit(long id, long companyId)
        {
            try
            {
                ISelectSetupSupplier iSelectSetupSupplier = new DSelectSetupSupplier(companyId);
                var supplierItem = iSelectSetupSupplier.SelectSupplierAll()
                    .Where(x => x.SupplierId == id)
                    .Select(s => new CommonSetupSupplier
                    {
                        Code = s.Code,
                        SupplierGroupId = s.SupplierGroupId,
                        Name = s.Name,
                        Address = s.Address,
                        Phone = s.Phone,
                        Fax = s.Fax,
                        Email = s.Email,
                        URL = s.URL,
                        ContactPerson = s.ContactPerson,
                        ContactPersonMobile = s.ContactPersonMobile,
                        ProfessionId = s.ProfessionId,
                        Designation = s.Designation,
                        BankId = s.BankId,
                        BankAccountName = s.BankAccountName,
                        BankAccountNumber = s.BankAccountNumber,
                        IsActive = s.IsActive
                    })
                    .FirstOrDefault();

                if (supplierItem != null)
                {
                    return supplierItem;
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

        public object SelectSupplierShortInfo(long supplierId, string currency, long companyId)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);
                decimal closingBalance = GetSupplierClosingBalance.GetSupplierWiseClosingBalance(currency, supplierId, companyId, DateTime.Now.Date.AddDays(1));

                ISelectSetupSupplier iSelectSetupCustomer = new DSelectSetupSupplier(companyId);
                var supplierInfo = iSelectSetupCustomer.SelectSupplierAll()
                    .Where(x => x.SupplierId == supplierId)
                    .Select(s => new
                    {
                        GroupName = s.Setup_SupplierGroup.Name,
                        ContactNo = s.Phone,
                        s.Address,
                        ClosingBalance = closingBalance
                    })
                    .FirstOrDefault();

                if (supplierInfo != null)
                {
                    return supplierInfo;
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
    }
}