using Inventory360DataModel;
using BLL.Common;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskCollection
    {
        private CommonRecordInformation<dynamic> SelectCollectionLists(string query, string currency, string collectionStatus, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                // get company currency
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskCollection iSelectTaskCollection = new DSelectTaskCollection(companyId);
                var collectionLists = iSelectTaskCollection.SelectCollectionAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.CollectionNo.ToLower().Contains(query.ToLower())
                        || x.Setup_Customer.Code.ToLower().Contains(query.ToLower())
                        || x.Setup_Customer.Name.ToLower().Contains(query.ToLower())
                        || x.Setup_Customer.PhoneNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(collectionStatus), x => x.Approved.Equals(collectionStatus))
                    .Select(s => new
                    {
                        s.CollectionId,
                        s.CollectionNo,
                        s.CollectionDate,
                        CollectedAmount = currencyInfo.BaseCurrency == currency ? s.CollectedAmount : (currencyInfo.Currency1 == currency ? s.CollectedAmount1 : s.CollectedAmount2),
                        CustomerCode = s.Setup_Customer.Code,
                        CustomerName = s.Setup_Customer.Name,
                        ContactNo = s.Setup_Customer.PhoneNo,
                        Approved = s.Approved,
                        CompanyId = s.CompanyId,
                        LocationId = s.LocationId
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = collectionLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = collectionLists
                    .OrderByDescending(o => o.CollectionDate)
                    .ThenByDescending(t => t.CollectionNo)
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

        public object SelectAllCollectionLists(string query, string currency, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectCollectionLists(query, currency, string.Empty, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectCollectionMappingDataByCustomerIdAndCollectionAgainst(string collectionAgainst, string currency, long customerId, long companyId)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                if (collectionAgainst.Equals(CommonEnum.SalesCollectionAgainst.Inv.ToString()))
                {
                    ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);
                    return iSelectTaskSalesInvoice.SelectSalesInvoiceAll()
                        .Where(x => x.CustomerId == customerId
                            && x.Approved.Equals("A")
                            && !x.IsSettledByCollection)
                        .Select(s => new
                        {
                            isSelected = false,
                            Id = s.InvoiceId,
                            No = s.InvoiceNo,
                            Date = s.InvoiceDate,
                            Amount = currencyInfo.BaseCurrency == currency ? (s.InvoiceAmount - s.InvoiceDiscount) : (currencyInfo.Currency1 == currency ? (s.Invoice1Amount - s.Invoice1Discount) : (s.Invoice2Amount - s.Invoice2Discount)),
                            CollectedAmount = currencyInfo.BaseCurrency == currency ? s.CollectedAmount : (currencyInfo.Currency1 == currency ? s.Collected1Amount : s.Collected2Amount),
                            GivenAmount = 0
                        })
                        .OrderBy(o => o.No)
                        .ToList();
                }
                else if (collectionAgainst.Equals(CommonEnum.SalesCollectionAgainst.SO.ToString()))
                {
                    ISelectTaskSalesOrder iSelectTaskSalesOrder = new DSelectTaskSalesOrder(companyId);
                    return iSelectTaskSalesOrder.SelectSalesOrderAll()
                        .Where(x => x.CustomerId == customerId
                            && (x.OrderAmount - x.OrderDiscount - x.CollectedAmount) > 0
                            && x.Approved.Equals("A"))
                        .Select(s => new
                        {
                            isSelected = false,
                            Id = s.SalesOrderId,
                            No = s.SalesOrderNo,
                            Date = s.OrderDate,
                            Amount = currencyInfo.BaseCurrency == currency ? (s.OrderAmount - s.OrderDiscount) : (currencyInfo.Currency1 == currency ? (s.Order1Amount - s.Order1Discount) : (s.Order2Amount - s.Order2Discount)),
                            CollectedAmount = currencyInfo.BaseCurrency == currency ? s.CollectedAmount : (currencyInfo.Currency1 == currency ? s.Collected1Amount : s.Collected2Amount),
                            GivenAmount = 0
                        })
                        .OrderBy(o => o.No)
                        .ToList();
                }
                else if (collectionAgainst.Equals(CommonEnum.SalesCollectionAgainst.Pre.ToString()))
                {
                    ISelectSetupCustomer iSelectSetupCustomer = new DSelectSetupCustomer(companyId);
                    return iSelectSetupCustomer.SelectCustomerAll()
                        .Where(x => x.CustomerId == customerId
                            && (x.OpeningBalance - x.CollectedAmount) > 0)
                        .Select(s => new
                        {
                            isSelected = false,
                            Id = customerId,
                            No = "Opening",
                            Date = s.OpeningDate,
                            Amount = currencyInfo.BaseCurrency == currency ? s.OpeningBalance : (currencyInfo.Currency1 == currency ? s.OpeningBalance1 : s.OpeningBalance2),
                            CollectedAmount = currencyInfo.BaseCurrency == currency ? s.CollectedAmount : (currencyInfo.Currency1 == currency ? s.Collected1Amount : s.Collected2Amount),
                            GivenAmount = 0
                        })
                        .OrderBy(o => o.No)
                        .ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectUnApprovedCollectionLists(string query, string currency, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectCollectionLists(query, currency, "N", companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}