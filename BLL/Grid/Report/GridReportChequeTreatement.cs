using Inventory360DataModel;
using Inventory360DataModel.Task;
using Inventory360DataModel.Temp;
using BLL.Common;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.DataAccess.Select.Temp;
using DAL.DataAccess.StoredProcedures;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using DAL.Interface.Select.Temp;
using DAL.Interface.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;


namespace BLL.Grid.Report
{
    public class GridReportChequeTreatement
    {
        CommonList commonList = new CommonList();
        public object GenerateChequeTreatementStatusWiseChequeDetailReport(long companyId, string currency,string ReportName, string dataPositionOptionValue, string chequeType, long locationId, long bankId, long customerOrSupplierId, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, string chequeOrTreatementBankOptionValue, string chequeCollectionOrPaymentDateOptionValue)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                var pagedData = new CommonRecordInformation<dynamic>();
                ISelectTaskChequeInfo iSelectTaskChequeInfo = new DSelectTaskChequeInfo(companyId);
                if (chequeType != "issuedCheque")
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .Where(x => x.CollectionDetailId != null)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate <= dateTo)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "CollectionDate"), x => x.Task_CollectionDetail.Task_Collection.CollectionDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "CollectionDate"), x => x.Task_CollectionDetail.Task_Collection.CollectionDate <= dateTo)
                        .WhereIf(!string.IsNullOrEmpty(chequeStatusCode), x => x.Status == chequeStatusCode || (x.Status == null && chequeStatusCode == "N"))
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue != "Treatement Bank"), x => x.BankId == bankId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue == "Treatement Bank"), x => x.Task_ChequeTreatment.OrderByDescending(y => y.StatusDate).FirstOrDefault().TreatmentBankId == bankId)
                        .WhereIf((customerOrSupplierId != 0), x => x.Task_CollectionDetail.Task_Collection.Setup_Customer.CustomerId == customerOrSupplierId)
                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            CompanyName = s.Setup_Company.Name,
                            CompanyAddress = s.Setup_Company.Address,
                            Phone = s.Setup_Company.PhoneNo,
                            Fax = s.Setup_Company.FaxNo,
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank.Name,
                            Amount = s.Amount,
                            //Amount = currencyInfo.BaseCurrency == currency ? s.Amount : (currencyInfo.Currency1 == currency ? s.Amount1 : s.Amount2),
                            Status = s.Status == null ? "N" : s.Status,
                            SendBankName = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.Name,
                            SendBankId = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault() == null ? 0 : s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.BankId,
                            BankCompareWith = chequeOrTreatementBankOptionValue,
                            CustomerOrSupplierName = s.Task_CollectionDetail.Task_Collection.Setup_Customer.Name,
                            CollectionOrPaymentNo = s.Task_CollectionDetail.Task_Collection.CollectionNo,
                            CollectionOrPaymentDate = s.Task_CollectionDetail.Task_Collection.CollectionDate,
                            VoucherNo = s.Task_Voucher.VoucherType == "Credit" ? s.Task_Voucher.VoucherNo : "",
                            VoucherId = s.Task_Voucher.VoucherType == "Credit" ? s.Task_Voucher.VoucherId : Guid.Empty,
                            CompanyId = s.CompanyId,
                            LocationId = s.LocationId
                        }).OrderBy(o => o.ChequeDate).ThenBy(t => t.ChequeNo).AsQueryable();

                    //List<CommonTaskChequeTreatmentDTO> _CommonTaskChequeTreatmentList = new List<CommonTaskChequeTreatmentDTO>();
                    //var ChequeStatusList = new ManagerDefault().SelectChequeStatus();
                    //foreach(var item in ChequeStatusList)
                    //{
                    //    List<CommonTaskChequeTreatmentDTO> _CommonTaskChequeTreatmentDTO = new List<CommonTaskChequeTreatmentDTO>();
                    //    if (item.Value == "N")
                    //        _CommonTaskChequeTreatmentDTO = ChequeInfoLists.Where(x=>x.Status == item.Value).ToList();

                    //}

                    var data = new
                    {
                        ReportName = commonList.SelectReportName().Where(x => x.Value == ReportName).Select(s => s.Item).FirstOrDefault(),
                        CompanyName = ChequeInfoLists.FirstOrDefault().CompanyName,
                        CompanyAddress = ChequeInfoLists.FirstOrDefault().CompanyAddress,
                        Phone = ChequeInfoLists.FirstOrDefault().Phone,
                        Fax = ChequeInfoLists.FirstOrDefault().Fax,
                        DateFrom = dateFrom,
                        DateTo = dateTo,
                        ChequeType = chequeType,
                        statusList = ChequeInfoLists.Select(x => new TempStatusList { Status = x.Status }).Distinct().ToList(),
                        ChequeInfoList = ChequeInfoLists
                    };
                    return data;
                }
                else
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .Where(x => x.PaymentDetailId != null)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate <= dateTo)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "PaymentDate"), x => x.Task_PaymentDetail.Task_Payment.PaymentDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "PaymentDate"), x => x.Task_PaymentDetail.Task_Payment.PaymentDate <= dateTo)
                        .WhereIf(!string.IsNullOrEmpty(chequeStatusCode), x => x.Status == chequeStatusCode)
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue != "Treatement Bank"), x => x.BankId == bankId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue == "Treatement Bank"), x => x.Task_ChequeTreatment.OrderByDescending(y => y.StatusDate).FirstOrDefault().TreatmentBankId == bankId)
                        .WhereIf((customerOrSupplierId != 0), x => x.Task_PaymentDetail.Task_Payment.Setup_Supplier.SupplierId == customerOrSupplierId)

                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            CompanyName = s.Setup_Company.Name,
                            CompanyAddress = s.Setup_Company.Address,
                            Phone = s.Setup_Company.PhoneNo,
                            Fax = s.Setup_Company.FaxNo,
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank == null ? "" : s.Setup_Bank.Name,
                            Amount = s.Amount,
                            //Amount = currencyInfo.BaseCurrency == currency ? s.Amount : (currencyInfo.Currency1 == currency ? s.Amount1 : s.Amount2),
                            Status = s.Status == null ? "N" : s.Status,
                            SendBankName = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.Name,
                            SendBankId = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault() == null ? 0 : s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.BankId,
                            BankCompareWith = chequeOrTreatementBankOptionValue,
                            CustomerOrSupplierName = s.Task_PaymentDetail.Task_Payment.Setup_Supplier.Name,
                            CollectionOrPaymentNo = s.Task_PaymentDetail.Task_Payment.PaymentNo,
                            CollectionOrPaymentDate = s.Task_PaymentDetail.Task_Payment.PaymentDate,
                            VoucherNo = s.Task_Voucher.VoucherType == "Debit" ? s.Task_Voucher.VoucherNo : "",
                            VoucherId = s.Task_Voucher.VoucherType == "Debit" ? s.Task_Voucher.VoucherId : Guid.Empty,
                            CompanyId = s.CompanyId,
                            LocationId = s.LocationId
                        }).OrderBy(o => o.ChequeDate).ThenBy(t => t.ChequeNo).AsQueryable();

                    var data = new
                    {
                        ReportName = commonList.SelectReportName().Where(x => x.Value == ReportName).Select(s => s.Item).FirstOrDefault(),
                        CompanyName = ChequeInfoLists.FirstOrDefault().CompanyName,
                        CompanyAddress = ChequeInfoLists.FirstOrDefault().CompanyAddress,
                        Phone = ChequeInfoLists.FirstOrDefault().Phone,
                        Fax = ChequeInfoLists.FirstOrDefault().Fax,
                        DateFrom = dateFrom,
                        DateTo = dateTo,
                        ChequeType = chequeType,
                        statusList = ChequeInfoLists.Select(x => new TempStatusList { Status = x.Status }).Distinct().ToList(),
                        ChequeInfoList = ChequeInfoLists
                    };
                    return data;
                }             

                {
                    throw new Exception("No record found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GenerateChequeTreatementStatusWiseChequeDetailReportChequeInHand(long companyId, string currency,string ReportName,string dataPositionOptionValue, string chequeType, long locationId, long bankId, long customerOrSupplierId, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, string chequeOrTreatementBankOptionValue, string chequeCollectionOrPaymentDateOptionValue)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                var pagedData = new CommonRecordInformation<dynamic>();
                ISelectTaskChequeInfo iSelectTaskChequeInfo = new DSelectTaskChequeInfo(companyId);
                if (ReportName == "ChequeInHand")
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .Where(x => x.CollectionDetailId != null)
                        .Where(x =>x.Status == null || x.Status == "D")
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate <= dateTo)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "CollectionDate"), x => x.Task_CollectionDetail.Task_Collection.CollectionDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "CollectionDate"), x => x.Task_CollectionDetail.Task_Collection.CollectionDate <= dateTo)                        
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue != "Treatement Bank"), x => x.BankId == bankId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue == "Treatement Bank"), x => x.Task_ChequeTreatment.OrderByDescending(y => y.StatusDate).FirstOrDefault().TreatmentBankId == bankId)
                        .WhereIf((customerOrSupplierId != 0), x => x.Task_CollectionDetail.Task_Collection.Setup_Customer.CustomerId == customerOrSupplierId)
                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            CompanyName = s.Setup_Company.Name,
                            CompanyAddress = s.Setup_Company.Address,
                            Phone = s.Setup_Company.PhoneNo,
                            Fax = s.Setup_Company.FaxNo,
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank.Name,
                            Amount = s.Amount,
                            //Amount = currencyInfo.BaseCurrency == currency ? s.Amount : (currencyInfo.Currency1 == currency ? s.Amount1 : s.Amount2),
                            Status = s.Status == null ? "N" : s.Status,
                            SendBankName = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.Name,
                            SendBankId = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault() == null ? 0 : s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.BankId,
                            BankCompareWith = chequeOrTreatementBankOptionValue,
                            CustomerOrSupplierName = s.Task_CollectionDetail.Task_Collection.Setup_Customer.Name,
                            CollectionOrPaymentNo = s.Task_CollectionDetail.Task_Collection.CollectionNo,
                            CollectionOrPaymentDate = s.Task_CollectionDetail.Task_Collection.CollectionDate,
                            VoucherNo = s.Task_Voucher.VoucherType == "Credit" ? s.Task_Voucher.VoucherNo : "",
                            VoucherId = s.Task_Voucher.VoucherType == "Credit" ? s.Task_Voucher.VoucherId : Guid.Empty,
                            CompanyId = s.CompanyId,
                            LocationId = s.LocationId
                        }).OrderBy(o => o.ChequeDate).ThenBy(t => t.ChequeNo).AsQueryable();

                    var data = new
                    {
                        ReportName = commonList.SelectReportName().Where(x => x.Value == ReportName).Select(s => s.Item).FirstOrDefault(),
                        CompanyName = ChequeInfoLists.FirstOrDefault().CompanyName,
                        CompanyAddress = ChequeInfoLists.FirstOrDefault().CompanyAddress,
                        Phone = ChequeInfoLists.FirstOrDefault().Phone,
                        Fax = ChequeInfoLists.FirstOrDefault().Fax,
                        DateFrom = dateFrom,
                        DateTo = dateTo,
                        ChequeType = chequeType,
                        statusList = ChequeInfoLists.Select(x => new TempStatusList { Status = x.Status }).Distinct().ToList(),
                        ChequeInfoList = ChequeInfoLists
                    };
                    return data;
                }
                else if (ReportName == "AdvanceChequeIssued")
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .Where(x => x.PaymentDetailId != null)
                        .Where(x => x.Status == null || x.Status == "D")
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate <= dateTo)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "PaymentDate"), x => x.Task_PaymentDetail.Task_Payment.PaymentDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "PaymentDate"), x => x.Task_PaymentDetail.Task_Payment.PaymentDate <= dateTo)
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue != "Treatement Bank"), x => x.BankId == bankId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue == "Treatement Bank"), x => x.Task_ChequeTreatment.OrderByDescending(y => y.StatusDate).FirstOrDefault().TreatmentBankId == bankId)
                        .WhereIf((customerOrSupplierId != 0), x => x.Task_PaymentDetail.Task_Payment.Setup_Supplier.SupplierId == customerOrSupplierId)
                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            CompanyName = s.Setup_Company.Name,
                            CompanyAddress = s.Setup_Company.Address,
                            Phone = s.Setup_Company.PhoneNo,
                            Fax = s.Setup_Company.FaxNo,
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank.Name,
                            Amount = s.Amount,
                            //Amount = currencyInfo.BaseCurrency == currency ? s.Amount : (currencyInfo.Currency1 == currency ? s.Amount1 : s.Amount2),
                            Status = s.Status == null ? "N" : s.Status,
                            SendBankName = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.Name,
                            SendBankId = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault() == null ? 0 : s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.BankId,
                            BankCompareWith = chequeOrTreatementBankOptionValue,
                            CustomerOrSupplierName = s.Task_PaymentDetail.Task_Payment.Setup_Supplier.Name,
                            CollectionOrPaymentNo = s.Task_PaymentDetail.Task_Payment.PaymentNo,
                            CollectionOrPaymentDate = s.Task_PaymentDetail.Task_Payment.PaymentDate,
                            VoucherNo = s.Task_Voucher.VoucherType == "Debit" ? s.Task_Voucher.VoucherNo : "",
                            VoucherId = s.Task_Voucher.VoucherType == "Debit" ? s.Task_Voucher.VoucherId : Guid.Empty,
                            CompanyId = s.CompanyId,
                            LocationId = s.LocationId
                        }).OrderBy(o => o.ChequeDate).ThenBy(t => t.ChequeNo).AsQueryable();

                    var data = new
                    {
                        ReportName = commonList.SelectReportName().Where(x => x.Value == ReportName).Select(s => s.Item).FirstOrDefault(),
                        CompanyName = ChequeInfoLists.FirstOrDefault().CompanyName,
                        CompanyAddress = ChequeInfoLists.FirstOrDefault().CompanyAddress,
                        Phone = ChequeInfoLists.FirstOrDefault().Phone,
                        Fax = ChequeInfoLists.FirstOrDefault().Fax,
                        DateFrom = dateFrom,
                        DateTo = dateTo,
                        ChequeType = chequeType,
                        statusList = ChequeInfoLists.Select(x => new TempStatusList { Status = x.Status }).Distinct().ToList(),
                        ChequeInfoList = ChequeInfoLists
                    };
                    return data;
                }
                {
                    throw new Exception("No record found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GenerateChequeTreatementStatusWiseChequeDetailReportChequeHistory(long companyId, string currency, string reportName, string dataPositionOptionValue, string chequeType, long locationId, long bankId, long customerOrSupplierId, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, string chequeOrTreatementBankOptionValue, string chequeCollectionOrPaymentDateOptionValue)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                var pagedData = new CommonRecordInformation<dynamic>();
                ISelectTaskChequeInfo iSelectTaskChequeInfo = new DSelectTaskChequeInfo(companyId);
                if (chequeType != "issuedCheque")
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .Where(x => x.CollectionDetailId != null)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate <= dateTo)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "CollectionDate"), x => x.Task_CollectionDetail.Task_Collection.CollectionDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "CollectionDate"), x => x.Task_CollectionDetail.Task_Collection.CollectionDate <= dateTo)
                        .WhereIf(!string.IsNullOrEmpty(chequeStatusCode), x => x.Status == chequeStatusCode || (x.Status == null && chequeStatusCode == "N"))
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue != "Treatement Bank"), x => x.BankId == bankId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue == "Treatement Bank"), x => x.Task_ChequeTreatment.OrderByDescending(y => y.StatusDate).FirstOrDefault().TreatmentBankId == bankId)
                        .WhereIf((customerOrSupplierId != 0), x => x.Task_CollectionDetail.Task_Collection.Setup_Customer.CustomerId == customerOrSupplierId)
                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            CompanyName = s.Setup_Company.Name,
                            CompanyAddress = s.Setup_Company.Address,
                            Phone = s.Setup_Company.PhoneNo,
                            Fax = s.Setup_Company.FaxNo,
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank.Name,
                            Amount = s.Amount,
                            Status = s.Status == null ? "N" : s.Status,
                            BankCompareWith = chequeOrTreatementBankOptionValue,
                            CustomerOrSupplierName = s.Task_CollectionDetail.Task_Collection.Setup_Customer.Name,
                            CollectionOrPaymentNo = s.Task_CollectionDetail.Task_Collection.CollectionNo,
                            CollectionOrPaymentDate = s.Task_CollectionDetail.Task_Collection.CollectionDate,
                            ChequeTreatment = s.Task_ChequeTreatment.Select(sd => new ChequeTreatment
                            {
                                ChequeInfoId = sd.ChequeInfoId,
                                Status = sd.Status,
                                StatusDate = sd.StatusDate ?? DateTime.Now ,
                                BankName = sd.Setup_Bank.Name
                                }).OrderBy(x => x.StatusDate).ToList()
                            }).OrderBy(o => o.ChequeDate).ThenBy(t => t.ChequeNo).AsQueryable();

                    var data = new
                    {
                        ReportName = commonList.SelectReportName().Where(x => x.Value == reportName).Select(s => s.Item).FirstOrDefault(),
                        CompanyName = ChequeInfoLists.FirstOrDefault().CompanyName,
                        CompanyAddress = ChequeInfoLists.FirstOrDefault().CompanyAddress,
                        Phone = ChequeInfoLists.FirstOrDefault().Phone,
                        Fax = ChequeInfoLists.FirstOrDefault().Fax,
                        DateFrom = dateFrom,
                        DateTo = dateTo,
                        ChequeType = chequeType,
                        CustomerOrSupplierNameList = ChequeInfoLists.Select(x =>x.CustomerOrSupplierName ).Distinct().ToList(),
                        ChequeInfoList = ChequeInfoLists.ToList()
                    };
                    return data;

                    //return ChequeInfoLists;
                }
                else
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .Where(x => x.PaymentDetailId != null)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "ChequeDate"), x => x.ChequeDate <= dateTo)
                        .WhereIf((dateFrom.HasValue && chequeCollectionOrPaymentDateOptionValue == "PaymentDate"), x => x.Task_PaymentDetail.Task_Payment.PaymentDate >= dateFrom)
                        .WhereIf((dateTo.HasValue && chequeCollectionOrPaymentDateOptionValue == "PaymentDate"), x => x.Task_PaymentDetail.Task_Payment.PaymentDate <= dateTo)
                        .WhereIf(!string.IsNullOrEmpty(chequeStatusCode), x => x.Status == chequeStatusCode)
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue != "Treatement Bank"), x => x.BankId == bankId)
                        .WhereIf((bankId != 0 && chequeOrTreatementBankOptionValue == "Treatement Bank"), x => x.Task_ChequeTreatment.OrderByDescending(y => y.StatusDate).FirstOrDefault().TreatmentBankId == bankId)
                        .WhereIf((customerOrSupplierId != 0), x => x.Task_PaymentDetail.Task_Payment.Setup_Supplier.SupplierId == customerOrSupplierId)

                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            CompanyName = s.Setup_Company.Name,
                            CompanyAddress = s.Setup_Company.Address,
                            Phone = s.Setup_Company.PhoneNo,
                            Fax = s.Setup_Company.FaxNo,
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank == null ? "" : s.Setup_Bank.Name,
                            Amount = s.Amount,                  
                            Status = s.Status == null ? "N" : s.Status,
                            BankCompareWith = chequeOrTreatementBankOptionValue,
                            CustomerOrSupplierName = s.Task_PaymentDetail.Task_Payment.Setup_Supplier.Name,
                            CollectionOrPaymentNo = s.Task_PaymentDetail.Task_Payment.PaymentNo,
                            CollectionOrPaymentDate = s.Task_PaymentDetail.Task_Payment.PaymentDate
                            ,
                            ChequeTreatment = s.Task_ChequeTreatment.Select(sd => new ChequeTreatment
                            {
                                ChequeInfoId = sd.ChequeInfoId,
                                Status = sd.Status,                               
                                StatusDate = sd.StatusDate ?? DateTime.Now,
                                BankName = sd.Setup_Bank.Name
                            }).OrderBy(x => x.StatusDate).ToList()
                        }).OrderBy(o => o.ChequeDate).ThenBy(t => t.ChequeNo).AsQueryable();

                    var data = new
                    {
                        ReportName = commonList.SelectReportName().Where(x => x.Value == reportName).Select(s => s.Item).FirstOrDefault(),
                        CompanyName = ChequeInfoLists.FirstOrDefault().CompanyName,
                        CompanyAddress = ChequeInfoLists.FirstOrDefault().CompanyAddress,
                        Phone = ChequeInfoLists.FirstOrDefault().Phone,
                        Fax = ChequeInfoLists.FirstOrDefault().Fax,
                        DateFrom = dateFrom,
                        DateTo = dateTo,
                        ChequeType = chequeType,
                        CustomerOrSupplierNameList = ChequeInfoLists.Select(x =>x.CustomerOrSupplierName).Distinct().ToList(),
                        ChequeInfoList = ChequeInfoLists.ToList()
                    };
                    return data;
                    //return ChequeInfoLists;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public object GenerateChequeTreatementStatusWiseChequeDetailReportCustomerSupplierwiseChequePerformance(long companyId, long entryBy, string currency,string ReportName, string dataPositionOptionValue, string chequeType, long locationId, long bankId, long customerOrSupplierId, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, string chequeOrTreatementBankOptionValue, string chequeCollectionOrPaymentDateOptionValue)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                var pagedData = new CommonRecordInformation<dynamic>();


                IExecuteCustomerOrSupplierWiseChequePerformance customerOrSupplierWiseChequePerformance = new DExecuteCustomerOrSupplierWiseChequePerformance( companyId,entryBy,  currency,  dataPositionOptionValue,  chequeType,  locationId,  bankId,  customerOrSupplierId, dateFrom,  dateTo,  chequeStatusCode,  chequeOrTreatementBankOptionValue,  chequeCollectionOrPaymentDateOptionValue);
                customerOrSupplierWiseChequePerformance.ExecuteCustomerOrSupplierWiseChequePerformance();

                ISelectTempCustomerOrSupplierWiseChequePerformance iSelectChequePerformanceInfo = new DSelectTempCustomerOrSupplierWiseChequePerformance(companyId, entryBy);
                var data = iSelectChequePerformanceInfo.SelectTempCustomerOrSupplierWiseChequePerformance().ToList();

                return new {
                    ReportName = ReportName,
                    ChequeType = chequeType,
                    DateFrom = dateFrom,
                    DateTo = dateTo,
                    LocationName = data.FirstOrDefault().LocationName,
                    CompanyName = data.FirstOrDefault().CompanyName,
                    CompanyAddress = data.FirstOrDefault().CompanyAddress,
                    CompanyContact = data.FirstOrDefault().CompanyContact,
                    CommonCustomerOrSupplierWiseChequePerformanceDetail = data.ToList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}