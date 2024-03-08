using Inventory360DataModel;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskSalesInvoice
    {
        public object SelectSalesInvoiceShortInfoByInvoiceId(Guid id, long companyId)
        {
            try
            {
                ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);

                return iSelectTaskSalesInvoice.SelectSalesInvoiceAll()
                    .Where(x => x.InvoiceId == id)
                    .Select(s => new
                    {
                        InvoiceId = s.InvoiceId,
                        InvoiceNo = s.InvoiceNo,
                        InvoiceDate = s.InvoiceDate,
                        InvoiceAmount = s.InvoiceAmount,
                        InvoiceDiscount = s.InvoiceDiscount,
                        CustomerId = s.CustomerId,
                        CustomerName = s.Setup_Customer.Code + " # " + s.Setup_Customer.PhoneNo + " # " + s.Setup_Customer.Name,
                        CustomerPhoneNo = s.Setup_Customer.PhoneNo,
                        CustomerAddress = s.Setup_Customer.Address,
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectSalesInvoiceInfoByProduct(long productId, string ProductSerial, long companyId)
        {
            try
            {
                ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);

                return iSelectTaskSalesInvoice.SelectSalesInvoiceDetail()
                    .Where(x => x.ProductId == productId && x.Task_SalesInvoiceDetailSerial.Any(y => y.Serial == ProductSerial))
                    .Select(s => new
                    {
                        InvoiceId = s.InvoiceId,
                        InvoiceNo = s.Task_SalesInvoice.InvoiceNo,
                        InvoiceDate = s.Task_SalesInvoice.InvoiceDate,
                        InvoiceAmount = s.Task_SalesInvoice.InvoiceAmount,
                        InvoiceDiscount = s.Task_SalesInvoice.InvoiceDiscount,
                        CustomerId = s.Task_SalesInvoice.CustomerId,
                        CustomerName = s.Task_SalesInvoice.Setup_Customer.Code + " # " + s.Task_SalesInvoice.Setup_Customer.PhoneNo + " # " + s.Task_SalesInvoice.Setup_Customer.Name,
                        CustomerPhoneNo = s.Task_SalesInvoice.Setup_Customer.PhoneNo,
                        CustomerAddress = s.Task_SalesInvoice.Setup_Customer.Address,
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectSalesInvoiceWarrantyInfoByProduct(Guid InvoiceId, long productId, long companyId)
        {
            try
            {
                if (InvoiceId == Guid.Empty)
                {
                    ISelectSetupProduct iSelectSetupProduct = new DSelectSetupProduct(companyId);
                    var data = iSelectSetupProduct.SelectProductAll()
                        .Where(x => x.ProductId == productId)
                        .Select(s => new
                        {
                            ProductId = s.ProductId,
                            IsWarrantyAvailable = s.IsWarrantyAvailable,
                            IsServiceWarranty = s.IsServiceWarranty,
                            IsLifeTimeWarranty = s.IsLifeTimeWarranty,
                            WarrantyDays = s.WarrantyDays,
                            AdditionalWarrantyDays = s.AdditionalWarrantyDays,
                            ServiceWarrantyDays = s.ServiceWarrantyDays,
                        }).FirstOrDefault();

                    var result = new
                    {
                        WarrantyDays = data.IsLifeTimeWarranty == true ? "Life Time Warranty" : (data.WarrantyDays + data.AdditionalWarrantyDays).ToString(),
                        WarrantyDaysLeft = data.IsLifeTimeWarranty == true ? "0" : "",
                        ServiceWarrantyDays = data.IsLifeTimeWarranty == true ? "Life Time Warranty" : data.ServiceWarrantyDays.ToString() + " "+"Days",
                        ServiceWarrantyDaysAvailable = data.IsLifeTimeWarranty == true ? "Life Time Warranty" : "",
                    };
                    return result;
                }
                else
                {
                    ISelectTaskSalesInvoice iSelectTaskSalesInvoice = new DSelectTaskSalesInvoice(companyId);
                    var data = iSelectTaskSalesInvoice.SelectSalesInvoiceDetail()
                        .Where(x => x.InvoiceId == InvoiceId && x.ProductId == productId)
                        .Select(s => new
                        {
                            s.InvoiceId,
                            s.ProductId,
                            s.Task_SalesInvoice.InvoiceDate,
                            s.Setup_Product.IsWarrantyAvailable,
                            s.Setup_Product.IsServiceWarranty,
                            s.Setup_Product.IsLifeTimeWarranty,
                            s.Setup_Product.WarrantyDays,
                            s.Setup_Product.AdditionalWarrantyDays,
                            s.Setup_Product.ServiceWarrantyDays,
                        }).FirstOrDefault();

                    var result = new
                    {
                        ProductId = data.ProductId,
                        InvoiceDate = data.InvoiceDate,
                        WarrantyDays = data.IsLifeTimeWarranty == true ? "Life Time Warranty" : (data.WarrantyDays + data.AdditionalWarrantyDays).ToString(),
                        WarrantyDaysLeft = productWarrantyLeftCalculation(data.WarrantyDays, data.AdditionalWarrantyDays, data.InvoiceDate, data.IsWarrantyAvailable, data.IsLifeTimeWarranty),
                        ServiceWarrantyDays = data.IsLifeTimeWarranty == true ? "Life Time Warranty" : data.ServiceWarrantyDays.ToString(),
                        ServiceWarrantyDaysAvailable = productWarrantyAvailableCalculation(data.ServiceWarrantyDays, data.InvoiceDate, data.IsServiceWarranty, data.IsLifeTimeWarranty)
                    };
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string productWarrantyLeftCalculation(decimal WarrantyDays, decimal AdditionalWarrantyDays, DateTime InvoiceDate, bool IsWarrantyAvailable, bool IsLifeTimeWarranty)
        {
            string result = "";
            if (IsLifeTimeWarranty)
            {
                result = "";
            }
            else if (IsWarrantyAvailable)
            {
                //decimal WarrantyLeftDays = (DateTime.Now.Date.Day - InvoiceDate.Day) > (WarrantyDays + AdditionalWarrantyDays) ? (DateTime.Now.Date.Day - InvoiceDate.Day) : 0;
                decimal WarrantyLeftDays = (WarrantyDays + AdditionalWarrantyDays) - (decimal)(DateTime.Now.Date - InvoiceDate).Days;
                if (WarrantyLeftDays < 0)
                {
                    result = Math.Abs(WarrantyLeftDays).ToString() + " Days Over";
                }
                else
                {
                    result = Math.Abs(WarrantyLeftDays).ToString() + " Days";
                }
            }
            return result;
        }
        private string productWarrantyAvailableCalculation(decimal ServiceWarrantyDays, DateTime InvoiceDate, bool IsServiceWarranty, bool IsLifeTimeWarranty)
        {
            string result = "0";
            if (IsLifeTimeWarranty)
            {
                result = "Life Time";
            }
            else if (IsServiceWarranty)
            {
                //result = (DateTime.Now.Date.Day - InvoiceDate.Day) < ServiceWarrantyDays ? (DateTime.Now.Date.Day - InvoiceDate.Day).ToString() : "0";
                decimal ServiceWarranty = ServiceWarrantyDays - (decimal)(DateTime.Now.Date - InvoiceDate).Days;
                if (ServiceWarranty < 0)
                {
                    result = Math.Abs(ServiceWarranty).ToString() + " Days Over";
                }
                else
                {
                    result = Math.Abs(ServiceWarranty).ToString() + " Days";
                }
            }
            return result;
        }
    }
}