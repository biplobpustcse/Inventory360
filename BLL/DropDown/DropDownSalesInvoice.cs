using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownSalesInvoice
    {
        public object SelectAllInvoiceNoByCompanyIdForDropdown(string query, long CustomerId, DateTime? dateFrom, DateTime? dateTo, long companyId, long locationId)
        {
            try
            {
                ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);

                return iSelectTaskSalesInvoice.SelectSalesInvoiceAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.InvoiceNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(CustomerId != 0, x => x.CustomerId == CustomerId)
                    .WhereIf(dateFrom.HasValue, x => x.InvoiceDate >= dateFrom)
                    .WhereIf(dateTo.HasValue, x => x.InvoiceDate <= dateTo)
                    .Where(x => x.LocationId == locationId && x.Approved.Equals("A"))
                    .OrderBy(o => o.InvoiceNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.InvoiceNo.ToString(),
                        Value = s.InvoiceId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductNameByInvoice(long companyId, string query, Guid invoiceId)
        {
            try
            {
                ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);

                return iSelectTaskSalesInvoice.SelectSalesInvoiceDetail()
                    .WhereIf(invoiceId != Guid.Empty, x => x.InvoiceId == invoiceId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Setup_Product.Name.ToLower().Contains(query.ToLower())
                     || x.Setup_Product.Code.ToLower().Contains(query.ToLower())
                    )
                    .OrderBy(o => o.Setup_Product.Name)
                    .Select(s => new
                    {
                        Item = "[" + s.Setup_Product.Code + "] " + s.Setup_Product.Name.ToString(),
                        Value = s.ProductId.ToString(),
                        isSerialProduct = s.Setup_Product.SerialAvailable,
                        Code = s.Setup_Product.Code,
                        Cost = s.Cost,
                        Cost1 = s.Cost1,
                        Cost2 = s.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductBySerialFromInvoice(long companyId,long LocationId, string serial,bool isReceiveAgainstPreviousSales, long ProductId, Guid InvoiceId)
        {
            try
            {
                if (isReceiveAgainstPreviousSales)
                {
                    ISelectStockCurrentStockSerial iSelectStockCurrentStockSerial = new DSelectStockCurrentStockSerial(companyId);

                    return iSelectStockCurrentStockSerial.SelectCurrentStockSerialAll()
                        .WhereIf(ProductId !=0, x => x.Stock_CurrentStock.ProductId == ProductId)
                        .Where(x=>x.Stock_CurrentStock.LocationId == LocationId && x.Stock_CurrentStock.CompanyId == companyId
                        )
                        .WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower()))
                        .OrderBy(o => o.Serial)
                        .Take(10)
                        .Select(s => new
                        {
                            Item = s.Serial,
                            Value = s.Serial,
                            ProductName = "[" + s.Stock_CurrentStock.Setup_Product.Code + "] " + s.Stock_CurrentStock.Setup_Product.Name.ToString(),
                            ProductId = s.Stock_CurrentStock.ProductId.ToString(),
                            s.Stock_CurrentStock.Setup_Product.Code,
                            s.Stock_CurrentStock.Cost,
                            s.Stock_CurrentStock.Cost1,
                            s.Stock_CurrentStock.Cost2
                        })
                        .ToList();
                }
                else
                {
                    ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);
                    return iSelectTaskSalesInvoice.SelectSalesInvoiceDetailSerial()
                        .WhereIf(InvoiceId != Guid.Empty, x => x.Task_SalesInvoiceDetail.InvoiceId == InvoiceId)
                        .WhereIf(ProductId != 0, x => x.Task_SalesInvoiceDetail.ProductId == ProductId)
                        .WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower())
                        || x.AdditionalSerial.ToLower().Contains(serial.ToLower())
                        )
                        .OrderBy(o => o.Serial)
                        .Select(s => new
                        {
                            Item = s.Serial.ToString(),
                            Value = s.Serial.ToString(),
                            ProductName = "[" + s.Task_SalesInvoiceDetail.Setup_Product.Code + "] " + s.Task_SalesInvoiceDetail.Setup_Product.Name.ToString(),
                            ProductId = s.Task_SalesInvoiceDetail.ProductId.ToString(),
                            s.Task_SalesInvoiceDetail.Setup_Product.Code,
                            s.Task_SalesInvoiceDetail.Cost,
                            s.Task_SalesInvoiceDetail.Cost1,
                            s.Task_SalesInvoiceDetail.Cost2
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}