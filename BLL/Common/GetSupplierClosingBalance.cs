using DAL.DataAccess.DatabaseFunctions;
using DAL.DataAccess.Select.Setup;
using DAL.Interface.DatabaseFunctions;
using DAL.Interface.Select.Setup;
using System;
using System.Linq;

namespace BLL.Common
{
    public class GetSupplierClosingBalance
    {
        public static decimal GetSupplierWiseClosingBalance(string currency, long supplierId, long companyId, DateTime closingDate)
        {
            try
            {
                IExecuteDBFnCurrencyLevel iExecuteDBFnCurrencyLevel = new DExecuteDBFnCurrencyLevel(currency, companyId);
                int currencyLevel = iExecuteDBFnCurrencyLevel.ExecuteDBFnCurrentyLevel();

                ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                long customerId = iSelectSetupCustomer.SelectCustomerAll()
                    .Where(x => x.SupplierId == supplierId)
                    .Select(s => s.CustomerId)
                    .DefaultIfEmpty(0)
                    .FirstOrDefault();

                // if found customerId then this supplier is combined by customer
                if (customerId > 0)
                {
                    IExecuteDBFnPartyLedgerBalance iExecuteDBFnPartyLedgerBalance = new DExecuteDBFnPartyLedgerBalance(currencyLevel, customerId, supplierId, closingDate, companyId);
                    return iExecuteDBFnPartyLedgerBalance.ExecuteDBFnPartyLedgerBalance();
                }
                else
                {
                    IExecuteDBFnSupplierLedgerBalance iExecuteDBFnSupplierLedgerBalance = new DExecuteDBFnSupplierLedgerBalance(currencyLevel, supplierId, closingDate, companyId);
                    return iExecuteDBFnSupplierLedgerBalance.ExecuteDBFnSupplierLedgerBalance();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}