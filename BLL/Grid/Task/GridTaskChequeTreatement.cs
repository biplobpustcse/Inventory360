using Inventory360DataModel;
using Inventory360DataModel.Task;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskChequeTreatement
    {
        private CommonRecordInformation<dynamic> SelectAllChequeInfoLists(string query,string chequeTypValue, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, long locationId, long ownBankId,long CustomerOrSupplierId, string currency, string collectionStatus, long companyId, int pageIndex, int pageSize)
        {
            try
            {

                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskChequeInfo iSelectTaskChequeInfo = new DSelectTaskChequeInfo(companyId);
                var pagedData = new CommonRecordInformation<dynamic>();               
                if (chequeTypValue != "issuedCheque")
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .WhereIf(dateFrom.HasValue, x => x.ChequeDate >= dateFrom)
                        .WhereIf(dateTo.HasValue, x => x.ChequeDate <= dateTo)
                        .WhereIf(!string.IsNullOrEmpty(chequeStatusCode), x => x.Status == chequeStatusCode || (x.Status == null && chequeStatusCode =="N"))
                        .WhereIf((locationId != 0), x => x.LocationId == locationId)
                        .WhereIf((ownBankId != 0), x => x.BankId == ownBankId)
                        .WhereIf((CustomerOrSupplierId != 0), x => x.Task_CollectionDetail.Task_Collection.Setup_Customer.CustomerId == CustomerOrSupplierId)
                        .WhereIf(!string.IsNullOrEmpty(query),
                            x => x.ChequeNo.ToLower().Contains(query.ToLower())
                            || x.Status.ToLower().Contains(query.ToLower())
                            || x.Setup_Bank.Name.ToLower().Contains(query.ToLower())
                        )

                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank.Name,
                            Amount = s.Amount,
                            //Amount = currencyInfo.BaseCurrency == currency ? s.Amount : (currencyInfo.Currency1 == currency ? s.Amount1 : s.Amount2),
                            Status = s.Status == null ? "N" : s.Status,
                            SendBankName = s.Task_ChequeTreatment.OrderByDescending(x=>x.StatusDate).FirstOrDefault().Setup_Bank.Name,
                            SendBankId = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.BankId,
                            CustomerOrSupplierName = s.Task_CollectionDetail.Task_Collection.Setup_Customer.Name,
                            CollectionOrPaymentNo = s.Task_CollectionDetail.Task_Collection.CollectionNo,
                            VoucherNo = s.Task_Voucher.VoucherType == "Credit" ? s.Task_Voucher.VoucherNo : "",
                            VoucherId = s.Task_Voucher.VoucherType == "Credit" ? s.Task_Voucher.VoucherId : Guid.Empty ,
                            CompanyId = s.CompanyId,
                            LocationId = s.LocationId
                        });

                    pagedData.TotalNumberOfRecords = ChequeInfoLists.Count();
                    pagedData.Data = ChequeInfoLists
                    .OrderByDescending(o => o.ChequeDate)
                    .ThenByDescending(t => t.ChequeNo)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                }
                else
                {
                    var ChequeInfoLists = iSelectTaskChequeInfo.SelectChequeInfoAll()
                        .WhereIf(dateFrom.HasValue, x => x.ChequeDate >= dateFrom)
                        .WhereIf(dateTo.HasValue, x => x.ChequeDate <= dateTo)
                        .WhereIf(!string.IsNullOrEmpty(chequeStatusCode), x => x.Status == chequeStatusCode)
                        .WhereIf((locationId != 0 && locationId != null), x => x.LocationId == locationId)
                        .WhereIf((ownBankId != 0 && ownBankId != null), x => x.BankId == ownBankId)
                        .WhereIf((CustomerOrSupplierId != 0), x => x.Task_PaymentDetail.Task_Payment.Setup_Supplier.SupplierId == CustomerOrSupplierId)
                        .WhereIf(!string.IsNullOrEmpty(query),
                            x => x.ChequeNo.ToLower().Contains(query.ToLower())
                            || x.Status.ToLower().Contains(query.ToLower())
                            || x.Setup_Bank.Name.ToLower().Contains(query.ToLower())
                        )

                        .Select(s => new CommonTaskChequeTreatmentDTO
                        {
                            isSelected = false,
                            ChequeInfoId = s.ChequeInfoId,
                            ChequeNo = s.ChequeNo,
                            ChequeDate = s.ChequeDate,
                            BankName = s.Setup_Bank == null? "": s.Setup_Bank.Name,
                            Amount = s.Amount,
                            //Amount = currencyInfo.BaseCurrency == currency ? s.Amount : (currencyInfo.Currency1 == currency ? s.Amount1 : s.Amount2),
                            Status = s.Status == null ? "N" : s.Status,
                            SendBankName = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.Name,
                            SendBankId = s.Task_ChequeTreatment.OrderByDescending(x => x.StatusDate).FirstOrDefault().Setup_Bank.BankId,
                            CustomerOrSupplierName = s.Task_PaymentDetail.Task_Payment.Setup_Supplier.Name,
                            CollectionOrPaymentNo = s.Task_PaymentDetail.Task_Payment.PaymentNo,
                            VoucherNo = s.Task_Voucher.VoucherType == "Debit"? s.Task_Voucher.VoucherNo : "",
                            VoucherId = s.Task_Voucher.VoucherType == "Debit" ? s.Task_Voucher.VoucherId : Guid.Empty,
                            CompanyId = s.CompanyId,
                            LocationId = s.LocationId
                        });

                    pagedData.TotalNumberOfRecords = ChequeInfoLists.Count();
                    pagedData.Data = ChequeInfoLists
                    .OrderByDescending(o => o.ChequeDate)
                    .ThenByDescending(t => t.ChequeNo)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                }
                
               
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                

                return pagedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectAllChequeInfoLists(string query,string chequeTypValue, DateTime? dateFrom, DateTime? dateTo, string chequeStatusCode, long locationId, long ownBankId,long CustomerOrSupplierId, string currency, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectAllChequeInfoLists(query, chequeTypValue, dateFrom, dateTo, chequeStatusCode, locationId, ownBankId, CustomerOrSupplierId, currency, string.Empty, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}