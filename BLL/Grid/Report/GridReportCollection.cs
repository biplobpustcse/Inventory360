using Inventory360DataModel;
using BLL.Common;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Report
{
    public class GridReportCollection
    {
        public object GenerateCollectionReport(long companyId, string currency, string collectionNo, Guid collectionId)
        {
            try
            {
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                ISelectTaskCollection iSelectTaskCollection = new DSelectTaskCollection(companyId);

                var collectionItem = iSelectTaskCollection.SelectCollectionAll()
                    .WhereIf(collectionId != Guid.Empty, x => x.CollectionId == collectionId)
                    .WhereIf(!string.IsNullOrEmpty(collectionNo), x => x.CollectionNo == collectionNo)
                    .Select(s => new
                    {
                        s.CollectionNo,
                        s.CollectionDate,
                        CustomerCode = s.Setup_Customer.Code,
                        CustomerPhone = s.Setup_Customer.PhoneNo,
                        CustomerName = s.Setup_Customer.Name,
                        CustomerAddress = s.Setup_Customer.Address,
                        CollectedAmount = currencyInfo.BaseCurrency == currency ? s.CollectedAmount : (currencyInfo.Currency1 == currency ? s.CollectedAmount1 : s.CollectedAmount2),
                        Approved = s.Approved,
                        ApprovedBy = s.ApprovedBy == null ? "" : s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        CancelReason = s.CancelReason,
                        CollectedBy = s.Security_User.FullName + " [" + s.Security_User.UserName + "]",
                        s.MRNo,
                        s.Remarks,
                        CompanyName = s.Setup_Company.Name,
                        CompanyAddress = s.Setup_Company.Address,
                        Phone = s.Setup_Company.PhoneNo,
                        Fax = s.Setup_Company.FaxNo,
                        EntryByName = s.Security_User2.FullName + " [" + s.Security_User2.UserName + "]",
                        CollectionDetailLists = s.Task_CollectionDetail.Select(sd => new
                        {
                            PaymentMode = sd.Configuration_PaymentMode.Name,
                            Amount = currencyInfo.BaseCurrency == currency ? sd.Amount : (currencyInfo.Currency1 == currency ? sd.Amount1 : sd.Amount2),
                            ChequeInfo = sd.Task_ChequeInfo.Select(ci => new
                            {
                                BankName = ci.Setup_Bank.Name,
                                ci.ChequeNo,
                                ci.ChequeDate,
                                ChequeAmount = currencyInfo.BaseCurrency == currency ? ci.Amount : (currencyInfo.Currency1 == currency ? ci.Amount1 : ci.Amount2)
                            })
                            .ToList()
                        }).OrderBy(o => o.PaymentMode).ToList()
                    })
                    .FirstOrDefault();

                if (collectionItem != null)
                {
                    return collectionItem;
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

        public object GenerateTransferRequisitionReport(long companyId, string transferRequisitionNo, Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}