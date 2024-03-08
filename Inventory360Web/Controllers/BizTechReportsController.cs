using Inventory360Web.Models;
using Newtonsoft.Json;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Inventory360Web.Controllers
{
    public class Inventory360ReportsController : BaseController
    {
        // GET: Inventory360Reports
        public ActionResult Collection()
        {
            return View();
        }
        public ActionResult TransferRequisition()
        {
            return View();
        }
        public ActionResult ChequeTreatmentRpt()
        {
            return View();
        }
        public ActionResult TransferOrderRpt()
        {
            return View();
        }
        public ActionResult Convertion()
        {
            return View();
        }


        public async Task<ActionResult> PrintIndCollection(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0009?collectionId=" + id + "&collectionNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonTaskCollection collectionInfo = JsonConvert.DeserializeObject<CommonTaskCollection>(Response);
                    collectionInfo.AmountInWord = AmountInWord(currency, Math.Round(collectionInfo.CollectedAmount, 2));
                    collectionInfo.CurrencyType = currency;
                    collectionInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_IndCollection", collectionInfo)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_IndCollection");
        }

        public async Task<ActionResult> PrintIndTransferRequisition(string token, string id = "", string no = "", string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0409?TransferRequisitionId=" + id + "&TransferRequisitionNo=" + no);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    ReportTaskItemRequisitionFinalization requisitionFinalize = JsonConvert.DeserializeObject<ReportTaskItemRequisitionFinalization>(Response);
                    requisitionFinalize.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == requisitionFinalize.Currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_IndTransferRequisition", requisitionFinalize)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_IndTransferRequisition");
        }
        public async Task<ActionResult> PrintIndTransferOrder(string id = "",string no="", string user = "",string token="")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0410?TransferOrderId=" + id+ "&TransferOrderNo=" + no);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    ReportTaskTransferOrder transferOrder = JsonConvert.DeserializeObject<ReportTaskTransferOrder>(Response);

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_IndTransferOrder", transferOrder)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_IndTransferOrder");
        }
        public async Task<ActionResult> PrintChequeTreatement(string token,string ReportName ,string dataPositionOptionValue, string chequeType,long LocationId,long bankId,long customerOrSupplierId,DateTime? dateFrom,DateTime? dateTo,string chequeStatusCode,string chequeOrTreatementBankOptionValue,string chequeCollectionOrPaymentDateOptionValue, string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = new HttpResponseMessage();
                Res = await client.GetAsync("api/R0200?ReportName=" + ReportName + "&dataPositionOptionValue=" + dataPositionOptionValue + "&chequeType=" + chequeType + "&LocationId=" + LocationId + "&bankId=" + bankId + "&customerOrSupplierId=" + customerOrSupplierId + "&dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&chequeStatusCode=" + chequeStatusCode + "&chequeOrTreatementBankOptionValue=" + chequeOrTreatementBankOptionValue + "&chequeCollectionOrPaymentDateOptionValue=" + chequeCollectionOrPaymentDateOptionValue);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    if(ReportName == "CustomerSupplierwiseChequePerformance")
                    {
                        CommonCustomerOrSupplierWiseChequePerformance ChequePerformanceInfo = JsonConvert.DeserializeObject<CommonCustomerOrSupplierWiseChequePerformance>(Response);

                        return new Rotativa.ViewAsPdf("_chequeTreatementStatusCustomerWiseChequePerformance", ChequePerformanceInfo)
                        {
                            CustomSwitches = PageHeader(user),
                            PageSize = Size.A3
                            //PageSize = Size.A4
                        };
                    }
                    CommonTaskChequeTreatement chequeTreatementInfo = JsonConvert.DeserializeObject<CommonTaskChequeTreatement>(Response);
            
                    //will take ActionMethod and generate the pdf
                    if (ReportName == "ChequeHistory")
                    {
                        return new Rotativa.ViewAsPdf("_chequeTreatementChequeHistory", chequeTreatementInfo)
                        {
                            CustomSwitches = PageHeader(user),
                            PageSize = Size.A3
                            //PageSize = Size.A4
                        };
                    }
                    else
                    {
                        return new Rotativa.ViewAsPdf("_chequeTreatementStatusWiseChequeDetail", chequeTreatementInfo)
                        {
                            CustomSwitches = PageHeader(user),
                            PageSize = Size.A3
                            //PageSize = Size.A4
                        };
                    }
                }
            }

           // return View("_reportBlankView");
            return new Rotativa.ViewAsPdf("_reportBlankView")
            {
                CustomSwitches = PageHeader(user),
                PageSize = Size.A3
            };
        }
        public async Task<ActionResult> PrintComplainReceive(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0201?ReceiveId=" + id + "&ReceiveNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonComplainReceive commonComplainReceiveInfo = JsonConvert.DeserializeObject<CommonComplainReceive>(Response);
                    commonComplainReceiveInfo.AmountInWord = AmountInWord(currency, Math.Round(commonComplainReceiveInfo.TotalChargeAmount, 2));
                    commonComplainReceiveInfo.CurrencyType = currency;
                    commonComplainReceiveInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_ComplainReceive", commonComplainReceiveInfo)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_ComplainReceive");
        }
        public async Task<ActionResult> PrintCustomerDelivery(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0202?DeliveryId=" + id + "&DeliveryNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonCustomerDelivery CommonCustomerDeliveryInfo = JsonConvert.DeserializeObject<CommonCustomerDelivery>(Response);
                    CommonCustomerDeliveryInfo.AmountInWord = AmountInWord(currency, Math.Round(CommonCustomerDeliveryInfo.TotalChargeAmount, 2));
                    CommonCustomerDeliveryInfo.CurrencyType = currency;
                    CommonCustomerDeliveryInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_CustomerDelivery", CommonCustomerDeliveryInfo)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_CustomerDelivery");
        }
        public async Task<ActionResult> PrintReplacementClaim(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0203?ClaimId=" + id + "&ClaimNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonReplacementClaim CommonReplacementClaimInfo = JsonConvert.DeserializeObject<CommonReplacementClaim>(Response);
                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_ReplacementClaim", CommonReplacementClaimInfo)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_ReplacementClaim");
        }
        public async Task<ActionResult> ReplacementReceive(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0204?ReceiveId=" + id + "&ReceiveNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonReplacementReceive commonReplacementReceive = JsonConvert.DeserializeObject<CommonReplacementReceive>(Response);
                    commonReplacementReceive.AmountInWord = AmountInWord(currency, Math.Round(commonReplacementReceive.TotalAmount, 2));
                    commonReplacementReceive.CurrencyType = currency;
                    commonReplacementReceive.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_ReplacementReceive", commonReplacementReceive)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_ReplacementReceive");
        }

        public ActionResult SalesAnalysis()
        {
            return View();
        }

        public async Task<ActionResult> PrintSalesAnalysis(string token, string reportName, long? locationId, string dateFrom, string dateTo, long salesPersonId, string salesMode, long? customerGroupId, long customerId, long? groupId, long? subGroupId, long? categoryId, long? brandId, string model, long productId, string currencyId, string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0029?locationId=" + (locationId == null ? 0 : locationId)
                    + "&dateFrom=" + dateFrom
                    + "&dateTo=" + dateTo
                    + "&salesPersonId=" + salesPersonId
                    + "&salesMode=" + salesMode
                    + "&customerGroupId=" + (customerGroupId == null ? 0 : customerGroupId)
                    + "&customerId=" + customerId
                    + "&groupId=" + (groupId == null ? 0 : groupId)
                    + "&subGroupId=" + (subGroupId == null ? 0 : subGroupId)
                    + "&categoryId=" + (categoryId == null ? 0 : categoryId)
                    + "&brandId=" + (brandId == null ? 0 : brandId)
                    + "&model=" + model
                    + "&productId=" + productId
                    + "&currencyId=" + currencyId);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonSalesAnalysisReport salesAnalysisReportInfo = JsonConvert.DeserializeObject<CommonSalesAnalysisReport>(Response);
                    salesAnalysisReportInfo.ReportName = reportName.Replace("_", " ");
                    if (string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
                    {
                        salesAnalysisReportInfo.DateRange = "All";
                    }
                    else
                    {
                        salesAnalysisReportInfo.DateRange = string.IsNullOrEmpty(dateFrom) ? string.Empty : ("From " + MyConversion.ConvertDateStringToFormattedDateString(dateFrom) + " ");
                        salesAnalysisReportInfo.DateRange += string.IsNullOrEmpty(dateTo) ? string.Empty : ("To " + MyConversion.ConvertDateStringToFormattedDateString(dateTo));
                    }
                    salesAnalysisReportInfo.CurrencyType = currencyId;
                    salesAnalysisReportInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currencyId).Select(s => s.CultureInfoCode).FirstOrDefault();

                    var reportFileName = reportName.Equals("Date_Wise_Sales") ? "_DateWiseSalesAnalysis"
                        : reportName.Equals("Date_Wise_Sales_With_Collection") ? "_DateWiseSalesWithCollectionAnalysis"
                        : reportName.Equals("Location_Wise_Sales_Summary") ? "_LocationWiseSalesSummaryAnalysis"
                        : reportName.Equals("Product_Wise_Sales_Summary") ? "_ProductWiseSalesSummaryAnalysis"
                        : reportName.Equals("Customer_Vs_Product_Sales_Summary") ? "_CustomerVsProductSalesSummaryAnalysis"
                        : reportName.Equals("SalesPerson_Wise_Sales_Summary") ? "_SalesPersonWiseSalesSummaryAnalysis"
                        : reportName.Equals("SalesPerson_Vs_Customer_Summary") ? "_SalesPersonVsCustomerSummaryAnalysis"
                        : reportName.Equals("SalesPerson_Vs_Customer_Detail") ? "_SalesPersonVsCustomerSalesDetailAnalysis"
                        : string.Empty;

                    var pageOrientation = reportName.Equals("Date_Wise_Sales") || reportName.Equals("SalesPerson_Vs_Customer_Detail") ? Orientation.Landscape
                        : Orientation.Portrait;

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf(reportFileName, salesAnalysisReportInfo)
                    {
                        CustomSwitches = PageHeaderFooter(user, string.Empty),
                        PageSize = Size.A4,
                        PageOrientation = pageOrientation
                    };
                }
            }

            return View("_IndStockReport");
        }
        public ActionResult ComplainReceiveAnalysis()
        {
            return View();
        }
        public async Task<ActionResult> PrintComplainReceiveAnalysis(string token, string reportName, long? locationId, string dateFrom, string dateTo, string complainReceiveId, long? customerGroupId, long customerId, long? groupId, long? subGroupId, long? categoryId, long? brandId, string model, long productId, string currencyId, string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0205?locationId=" + (locationId == null ? 0 : locationId)
                    + "&dateFrom=" + dateFrom
                    + "&dateTo=" + dateTo
                    + "&complainReceiveId=" + complainReceiveId
                    + "&customerGroupId=" + (customerGroupId == null ? 0 : customerGroupId)
                    + "&customerId=" + customerId
                    + "&groupId=" + (groupId == null ? 0 : groupId)
                    + "&subGroupId=" + (subGroupId == null ? 0 : subGroupId)
                    + "&categoryId=" + (categoryId == null ? 0 : categoryId)
                    + "&brandId=" + (brandId == null ? 0 : brandId)
                    + "&model=" + model
                    + "&productId=" + productId
                    + "&currencyId=" + currencyId);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonComplainReceiveAnalysisReport complainReceiveAnalysisReportInfo = JsonConvert.DeserializeObject<CommonComplainReceiveAnalysisReport>(Response);
                    complainReceiveAnalysisReportInfo.ReportName = reportName.Replace("_", " ");
                    if (string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
                    {
                        complainReceiveAnalysisReportInfo.DateRange = "All";
                    }
                    else
                    {
                        complainReceiveAnalysisReportInfo.DateRange = string.IsNullOrEmpty(dateFrom) ? string.Empty : ("From " + MyConversion.ConvertDateStringToFormattedDateString(dateFrom) + " ");
                        complainReceiveAnalysisReportInfo.DateRange += string.IsNullOrEmpty(dateTo) ? string.Empty : ("To " + MyConversion.ConvertDateStringToFormattedDateString(dateTo));
                    }
                    complainReceiveAnalysisReportInfo.CurrencyType = currencyId;
                    complainReceiveAnalysisReportInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currencyId).Select(s => s.CultureInfoCode).FirstOrDefault();

                    var reportFileName = reportName.Equals("Date_Wise_Complain_Receive") ? "_DateWiseComplainReceive"
                        : reportName.Equals("Complain_Receive_Status") ? "_ComplainReceiveStatus"
                        : reportName.Equals("Pending_Delivery_(RMA)") ? "_ComplainReceiveDeliveryPending"              
                        : reportName.Equals("Customer_Wise_Complain_Receive") ? "_CustomerWiseComplainReceive"
                        : string.Empty;

                    var pageOrientation = Orientation.Landscape;

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf(reportFileName, complainReceiveAnalysisReportInfo)
                    {
                        CustomSwitches = PageHeaderFooter(user, string.Empty),
                        PageSize = Size.A4,
                        PageOrientation = pageOrientation
                    };
                }
            }

            return View("_reportBlankView");
        }
        public ActionResult CustomerDeliveryAnalysis()
        {
            return View();
        }
        public async Task<ActionResult> PrintCustomerDeliveryAnalysis(string token, string reportName, long? locationId, string dateFrom, string dateTo, string customerDeliveryId, long? customerGroupId, long customerId, long? groupId, long? subGroupId, long? categoryId, long? brandId, string model, long productId, string currencyId, string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0206?locationId=" + (locationId == null ? 0 : locationId)
                    + "&dateFrom=" + dateFrom
                    + "&dateTo=" + dateTo
                    + "&customerDeliveryId=" + customerDeliveryId
                    + "&customerGroupId=" + (customerGroupId == null ? 0 : customerGroupId)
                    + "&customerId=" + customerId
                    + "&groupId=" + (groupId == null ? 0 : groupId)
                    + "&subGroupId=" + (subGroupId == null ? 0 : subGroupId)
                    + "&categoryId=" + (categoryId == null ? 0 : categoryId)
                    + "&brandId=" + (brandId == null ? 0 : brandId)
                    + "&model=" + model
                    + "&productId=" + productId
                    + "&currencyId=" + currencyId);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonCustomerDeliveryAnalysisReport customerDeliveryAnalysisReportInfo = JsonConvert.DeserializeObject<CommonCustomerDeliveryAnalysisReport>(Response);
                    customerDeliveryAnalysisReportInfo.ReportName = reportName.Replace("_", " ");
                    if (string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
                    {
                        customerDeliveryAnalysisReportInfo.DateRange = "All";
                    }
                    else
                    {
                        customerDeliveryAnalysisReportInfo.DateRange = string.IsNullOrEmpty(dateFrom) ? string.Empty : ("From " + MyConversion.ConvertDateStringToFormattedDateString(dateFrom) + " ");
                        customerDeliveryAnalysisReportInfo.DateRange += string.IsNullOrEmpty(dateTo) ? string.Empty : ("To " + MyConversion.ConvertDateStringToFormattedDateString(dateTo));
                    }
                    customerDeliveryAnalysisReportInfo.CurrencyType = currencyId;
                    customerDeliveryAnalysisReportInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currencyId).Select(s => s.CultureInfoCode).FirstOrDefault();

                    var reportFileName = reportName.Equals("Date_Wise_Delivery_(RMA)_Summary") ? "_DateWiseCustomerDeliverySummary"
                        : reportName.Equals("Date_Wise_Delivery_(RMA)_Detail") ? "_DateWiseCustomerDeliveryDetail"
                        : reportName.Equals("Customer_Wise_Delivery_(RMA)_Detail") ? "_CustomerWiseDeliveryDetail"
                        : string.Empty;

                    var pageOrientation = reportName.Equals("Date_Wise_Delivery_(RMA)Detail") || reportName.Equals("Customer_Wise_Delivery_(RMA)Detail") ? Orientation.Landscape
                       : Orientation.Portrait;

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf(reportFileName, customerDeliveryAnalysisReportInfo)
                    {
                        CustomSwitches = PageHeaderFooter(user, string.Empty),
                        PageSize = Size.A4,
                        PageOrientation = pageOrientation
                    };
                }
            }

            return View("_reportBlankView");
        }
        public ActionResult ReplacementClaimAnalysis()
        {
            return View();
        }
        public async Task<ActionResult> PrintReplacementClaimAnalysis(string token, string reportName, long? locationId, string dateFrom, string dateTo, long? supplierGroupId, long supplierId, long? groupId, long? subGroupId, long? categoryId, long? brandId, string model, long productId, string currencyId, string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0207?locationId=" + (locationId == null ? 0 : locationId)
                    + "&dateFrom=" + dateFrom
                    + "&dateTo=" + dateTo
                    + "&supplierGroupId=" + (supplierGroupId == null ? 0 : supplierGroupId)
                    + "&supplierId=" + supplierId
                    + "&groupId=" + (groupId == null ? 0 : groupId)
                    + "&subGroupId=" + (subGroupId == null ? 0 : subGroupId)
                    + "&categoryId=" + (categoryId == null ? 0 : categoryId)
                    + "&brandId=" + (brandId == null ? 0 : brandId)
                    + "&model=" + model
                    + "&productId=" + productId
                    + "&currencyId=" + currencyId);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonReplacementClaimAnalysisReport replacementClaimAnalysisReportInfo = JsonConvert.DeserializeObject<CommonReplacementClaimAnalysisReport>(Response);
                    replacementClaimAnalysisReportInfo.ReportName = reportName.Replace("_", " ");
                    if (string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
                    {
                        replacementClaimAnalysisReportInfo.DateRange = "All";
                    }
                    else
                    {
                        replacementClaimAnalysisReportInfo.DateRange = string.IsNullOrEmpty(dateFrom) ? string.Empty : ("From " + MyConversion.ConvertDateStringToFormattedDateString(dateFrom) + " ");
                        replacementClaimAnalysisReportInfo.DateRange += string.IsNullOrEmpty(dateTo) ? string.Empty : ("To " + MyConversion.ConvertDateStringToFormattedDateString(dateTo));
                    }
                    replacementClaimAnalysisReportInfo.CurrencyType = currencyId;
                    replacementClaimAnalysisReportInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currencyId).Select(s => s.CultureInfoCode).FirstOrDefault();

                    var reportFileName = reportName.Equals("Date_Wise_Replacement_Claim") ? "_DateWiseReplacementClaim"
                        : reportName.Equals("Supplier_Wise_Replacement_Claim_Detail") ? "_SupplierWiseReplacementClaimDetail"
                        : reportName.Equals("Product_Receivable_From_Vendor") ? "_ReplacementProductReceivableFromVendor"
                        : string.Empty;

                    var pageOrientation = Orientation.Landscape;

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf(reportFileName, replacementClaimAnalysisReportInfo)
                    {
                        CustomSwitches = PageHeaderFooter(user, string.Empty),
                        PageSize = Size.A4,
                        PageOrientation = pageOrientation
                    };
                }
            }

            return View("_reportBlankView");
        }
        public ActionResult ReplacementReceiveAnalysis()
        {
            return View();
        }
        public async Task<ActionResult> PrintReplacementReceiveAnalysis(string token, string reportName, long? locationId, string dateFrom, string dateTo, string replacementReceiveId, long? supplierGroupId, long supplierId, long? groupId, long? subGroupId, long? categoryId, long? brandId, string model, long productId, string currencyId, string user = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0208?locationId=" + (locationId == null ? 0 : locationId)
                    + "&dateFrom=" + dateFrom
                    + "&dateTo=" + dateTo
                    + "&replacementReceiveId=" + replacementReceiveId
                    + "&supplierGroupId=" + (supplierGroupId == null ? 0 : supplierGroupId)
                    + "&supplierId=" + supplierId
                    + "&groupId=" + (groupId == null ? 0 : groupId)
                    + "&subGroupId=" + (subGroupId == null ? 0 : subGroupId)
                    + "&categoryId=" + (categoryId == null ? 0 : categoryId)
                    + "&brandId=" + (brandId == null ? 0 : brandId)
                    + "&model=" + model
                    + "&productId=" + productId
                    + "&currencyId=" + currencyId);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonReplacementReceiveAnalysisReport replacementReceiveAnalysisReport = JsonConvert.DeserializeObject<CommonReplacementReceiveAnalysisReport>(Response);
                    replacementReceiveAnalysisReport.ReportName = reportName.Replace("_", " ");
                    if (string.IsNullOrEmpty(dateFrom) && string.IsNullOrEmpty(dateTo))
                    {
                        replacementReceiveAnalysisReport.DateRange = "All";
                    }
                    else
                    {
                        replacementReceiveAnalysisReport.DateRange = string.IsNullOrEmpty(dateFrom) ? string.Empty : ("From " + MyConversion.ConvertDateStringToFormattedDateString(dateFrom) + " ");
                        replacementReceiveAnalysisReport.DateRange += string.IsNullOrEmpty(dateTo) ? string.Empty : ("To " + MyConversion.ConvertDateStringToFormattedDateString(dateTo));
                    }
                    replacementReceiveAnalysisReport.CurrencyType = currencyId;
                    replacementReceiveAnalysisReport.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currencyId).Select(s => s.CultureInfoCode).FirstOrDefault();

                    var reportFileName = reportName.Equals("Date_Wise_Replacement_Receive_Summary") ? "_Date_Wise_Replacement_Receive_Summary"
                        : reportName.Equals("Date_Wise_Replacement_Receive_Detail") ? "_Date_Wise_Replacement_Receive_Detail"
                        : reportName.Equals("Supplier_Wise_Replacement_Receive_Detail") ? "_Supplier_Wise_Replacement_Receive_Detail"
                        : reportName.Equals("Product_Wise_Replacement_Receive_Summary") ? "_Product_Wise_Replacement_Receive_Summary"
                        : string.Empty;

                    var pageOrientation = reportName.Equals("Date_Wise_Replacement_Receive_Detail") || reportName.Equals("Supplier_Wise_Replacement_Receive_Detail") ? Orientation.Landscape
                       : Orientation.Portrait;                    

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf(reportFileName, replacementReceiveAnalysisReport)
                    {
                        CustomSwitches = PageHeaderFooter(user, string.Empty),
                        PageSize = Size.A4,
                        PageOrientation = pageOrientation
                    };
                }
            }

            return View("_reportBlankView");
        }        
        public async Task<ActionResult> PrintConvertion(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0209?ConvertionId=" + id + "&ConvertionNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonConvertion commonConvertionInfo = JsonConvert.DeserializeObject<CommonConvertion>(Response);                    
                    commonConvertionInfo.CurrencyType = currency;
                    commonConvertionInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_Convertion", commonConvertionInfo)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_reportBlankView");
        }
        public async Task<ActionResult> PrintConvertionRatio(string token, string id = "", string no = "", string user = "", string currency = "")
        {
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiAddress"].ToString());

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Define token value as header
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //Sending request to find web api REST service resource using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/R0210?ConvertionRatioId=" + id + "&RatioNo=" + no + "&currency=" + currency);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the list  
                    CommonConvertionRatio commonConvertionRatioInfo = JsonConvert.DeserializeObject<CommonConvertionRatio>(Response);
                    commonConvertionRatioInfo.CurrencyType = currency;
                    commonConvertionRatioInfo.CurrencyCultureInfo = CurrencyFormat.CountryWiseCultureInfo().Where(x => x.CurrencyCode == currency).Select(s => s.CultureInfoCode).FirstOrDefault();

                    //will take ActionMethod and generate the pdf
                    return new Rotativa.ViewAsPdf("_ConvertionRatio", commonConvertionRatioInfo)
                    {
                        CustomSwitches = PageHeader(user),
                        PageSize = Size.A4
                    };
                }
            }

            return View("_reportBlankView");
        }

    }
}