using Inventory360DataModel;
using Inventory360DataModel.Task;
using BLL.Common;
using DAL.DataAccess.Select.Setup;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Setup;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskComplainReceive
    {
        private CommonRecordInformation<dynamic> SelectComplainReceive(string query, string ComplainReceiveStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);
                var complainReceiveLists = iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ReceiveNo.ToLower().Contains(query.ToLower())
                    || x.Setup_Customer.Name.ToLower().Contains(query.ToLower())
                    || x.Setup_Customer.Code.ToLower().Contains(query.ToLower())
                    || x.Setup_Customer.PhoneNo.ToLower().Contains(query.ToLower())
                    )
                    .WhereIf(!string.IsNullOrEmpty(ComplainReceiveStatus), x => x.Approved == "N" || x.Approved == null)
                    .Select(s => new
                    {
                        s.ReceiveId,
                        s.ReceiveNo,
                        s.ReceiveDate,
                        CustomerName = s.Setup_Customer.Name,
                        CustomerCode = s.Setup_Customer.Code,
                        CustomerPhoneNo = s.Setup_Customer.PhoneNo,
                        s.TotalChargeAmount,
                        s.Approved
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = complainReceiveLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = complainReceiveLists
                    .OrderByDescending(o => o.ReceiveDate)
                    .ThenByDescending(t => t.ReceiveNo)
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
        public object SelectComplainReceiveLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectComplainReceive(query, string.Empty, locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectUnApprovedComplainReceiveLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectComplainReceive(query, "N", locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectComplainReceiveInfoByProduct(long productId, string ProductSerial, long companyId, Guid ComplainReceiveId)
        {
            try
            {
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);

                return iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .Where(x => x.ProductId == productId && x.Serial == ProductSerial)
                    .WhereIf(ComplainReceiveId != Guid.Empty,x=>x.Task_ComplainReceive.ReceiveId == ComplainReceiveId)
                    .Select(s => new
                    {
                        s.ReceiveId,
                        s.ReceiveDetailId,
                        s.Task_ComplainReceive.ReceiveNo,
                        s.Task_ComplainReceive.ReceiveDate,
                        s.Task_ComplainReceive.TotalChargeAmount,
                        //s.Task_ComplainReceive.TotalSparePartsAmount,
                        //s.Task_ComplainReceive.TotalSparePartsDiscount,
                        s.Task_ComplainReceive.CustomerId,
                        CustomerName = s.Task_ComplainReceive.Setup_Customer.Code + " # " + s.Task_ComplainReceive.Setup_Customer.PhoneNo + " # " + s.Task_ComplainReceive.Setup_Customer.Name,
                        CustomerPhoneNo = s.Task_ComplainReceive.Setup_Customer.PhoneNo,
                        CustomerAddress = s.Task_ComplainReceive.Setup_Customer.Address
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetAllProblemWithComplainReceivedForCustomerDelivery(long companyId, Guid receiveDetailId)
        {
            try
            {
                ISelectSetupProblem iSelectSetupProblem = new DSelectSetupProblem(companyId);
                var problemLists = iSelectSetupProblem.SelectProblemAll()
                    .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                        && (x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.ComplainReceive.ToString()) || x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.CustomerDelivery.ToString())))
                    .Select(s => new CommonComplainReceiveDetailProblemFromDelivery
                    {
                        isSelected = false,
                        ProblemId = s.ProblemId,
                        Name = s.Name,
                        Note = ""
                    })
                    .OrderBy(o => o.Name)
                    .ToList();

                if (receiveDetailId != Guid.Empty)
                {
                    ISelectTaskComplainReceiveDetailProblem iSelectTaskComplainReceiveDetailProblem = new DSelectTaskComplainReceiveDetailProblem(companyId);
                    var receivedProblemLists = iSelectTaskComplainReceiveDetailProblem.SelectComplainReceiveDetailProblemAll()
                        .Where(x => x.ReceiveDetailId == receiveDetailId)
                        .Select(s => new CommonComplainReceiveDetailProblemFromDelivery
                        {
                            isSelected = false,
                            ProblemId = s.ProblemId,
                            Name = s.Setup_Problem.Name,
                            Note = s.Note
                        })
                        .ToList();

                    foreach (var item in problemLists)
                    {
                        var itemFound = receivedProblemLists.Where(x => x.ProblemId == item.ProblemId).FirstOrDefault();
                        if (itemFound != null)
                        {
                            item.isSelected = true;
                            item.Note = itemFound.Note;
                        }
                    }
                }

                return problemLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetAllProblemWithComplainReceivedForReplacementClaim(long companyId, Guid receiveDetailId)
        {
            try
            {
                ISelectSetupProblem iSelectSetupProblem = new DSelectSetupProblem(companyId);
                var problemLists = iSelectSetupProblem.SelectProblemAll()
                    .Where(x => x.Configuration_OperationalEvent.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString())
                        && (x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.ComplainReceive.ToString()) || x.Configuration_OperationalEvent.SubEventName.Equals(CommonEnum.OperationalSubEvent.ReplacementClaim.ToString())))
                    .Select(s => new CommonComplainReceiveDetailProblemFromDelivery
                    {
                        isSelected = false,
                        ProblemId = s.ProblemId,
                        Name = s.Name,
                        Note = ""
                    })
                    .OrderBy(o => o.Name)
                    .ToList();

                if (receiveDetailId != Guid.Empty)
                {
                    ISelectTaskComplainReceiveDetailProblem iSelectTaskComplainReceiveDetailProblem = new DSelectTaskComplainReceiveDetailProblem(companyId);
                    var receivedProblemLists = iSelectTaskComplainReceiveDetailProblem.SelectComplainReceiveDetailProblemAll()
                        .Where(x => x.ReceiveDetailId == receiveDetailId)
                        .Select(s => new CommonComplainReceiveDetailProblemFromDelivery
                        {
                            isSelected = false,
                            ProblemId = s.ProblemId,
                            Name = s.Setup_Problem.Name,
                            Note = s.Note
                        })
                        .ToList();

                    foreach (var item in problemLists)
                    {
                        var itemFound = receivedProblemLists.Where(x => x.ProblemId == item.ProblemId).FirstOrDefault();
                        if (itemFound != null)
                        {
                            item.isSelected = true;
                            item.Note = itemFound.Note;
                        }
                    }
                }

                return problemLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetAllChargeWithComplainReceived(long companyId, Guid complainReceiveId, string currency)
        {
            try
            {
                // get company currency
                var currencyInfo = GetCompanyCurrencyInfo.CompanyCurrencyInfo(companyId);

                ISelectSetupEventWiseCharge iSelectSetupEventWiseCharge = new DSelectSetupEventWiseCharge(companyId);
                var chargeLists= iSelectSetupEventWiseCharge.SelectEventWiseChargeAll()
                    .Where(x=>x.EventName.Equals(CommonEnum.OperationalEvent.RMA.ToString()))
                        .Select(s => new CommonComplainReceiveChargeFromDelivery
                        {
                            isSelected = false,
                            ChargeId=s.ChargeId,
                            Name=s.Setup_Charge.Name,
                            ChargeAmount = 0
                        })
                        .ToList();



                if (complainReceiveId != Guid.Empty)
                {
                    ISelectTaskComplainReceiveCharge iSelectTaskComplainReceiveCharge = new DSelectTaskComplainReceiveCharge(companyId);
                    var receiveChargeLists= iSelectTaskComplainReceiveCharge.SelectTaskComplainReceiveChargeAll()
                        .Where(x=>x.ReceiveId==complainReceiveId)
                        .Select(s => new CommonComplainReceiveChargeFromDelivery
                        {
                            isSelected = false,
                            ChargeId=s.Configuration_EventWiseCharge.ChargeId,
                            Name=s.Configuration_EventWiseCharge.Setup_Charge.Name,
                            ChargeAmount = currencyInfo.BaseCurrency == currency ? s.ChargeAmount : (currencyInfo.Currency1 == currency ? s.Charge1Amount : s.Charge2Amount)
                        })
                        .ToList();

                    foreach (var item in chargeLists)
                    {
                        var itemFound = receiveChargeLists.Where(x => x.ChargeId == item.ChargeId).FirstOrDefault();
                        if (itemFound != null)
                        {
                            item.isSelected = true;
                            item.ChargeAmount = itemFound.ChargeAmount;
                        }
                    }
                }

                return chargeLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetSpareProductByParentRcvdIdAndProduct(long companyId, Guid ComplainReceiveId, long ProductId, string serial)
        {
            try
            {
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);

                return iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetail_SpareProductAll()
                    .Where(x => x.Task_ComplainReceiveDetail.ReceiveId == ComplainReceiveId && x.Task_ComplainReceiveDetail.ProductId == ProductId && x.Task_ComplainReceiveDetail.Serial == serial)
                    .Select(s => new
                    {
                        s.ProductId,
                        ProductName = s.Setup_Product.Name,
                        s.UnitTypeId,
                        UnitTypeName = s.Setup_UnitType.Name,
                        s.Quantity,
                        s.Price,
                        s.Discount,
                        s.ProductDimensionId,
                        isServiceProduct = s.Setup_Product.ProductType == "Service" ? true : false,
                        Remarks = ""
                    });

                //return resultList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectComplainReceiveShortInfoById(Guid id, long companyId)
        {
            try
            {
                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);

                return iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .Where(x => x.ReceiveId == id)
                    .Select(s => new
                    {
                        s.ReceiveId,
                        s.ReceiveNo,
                        s.ReceiveDate,
                        s.TotalChargeAmount,
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
    }
}