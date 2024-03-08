using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownComplainReceive
    {
        public object SelectAllComplainReceiveNoByCompanyIdForDropdown(string query, long CustomerId, string dateFrom, string dateTo, long companyId, long locationId)
        {
            try
            {
                DateTime? fromDate = MyConversion.ConvertDateStringToDate(dateFrom);
                DateTime? toDate = MyConversion.ConvertDateStringToDate(dateTo);

                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);

                return iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ReceiveNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(CustomerId != 0, x => x.CustomerId == CustomerId)
                    .WhereIf(fromDate != null, x => x.ReceiveDate >= fromDate)
                    .WhereIf(toDate != null, x => x.ReceiveDate <= toDate)
                    .Where(x => x.LocationId == locationId && x.Approved.Equals("A"))
                    .OrderBy(o => o.ReceiveNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.ReceiveNo.ToString(),
                        Value = s.ReceiveId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductNameByComplainReceive(long companyId, string query, Guid complainReceiveId)
        {
            try
            {
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);

                return iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .WhereIf(complainReceiveId != Guid.Empty, x => x.ReceiveId == complainReceiveId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Setup_Product.Name.ToLower().Contains(query.ToLower())
                     || x.Setup_Product.Code.ToLower().Contains(query.ToLower())
                    )
                    .Where(x=>x.Task_ComplainReceive.Approved.Equals("A"))
                    .OrderBy(o => o.Setup_Product.Name)
                    .Select(s => new
                    {
                        Item = "[" + s.Setup_Product.Code + "] " + s.Setup_Product.Name.ToString(),
                        Value = s.ProductId.ToString(),
                        isSerialProduct = s.Setup_Product.SerialAvailable,
                        s.Setup_Product.Code,
                        complainReceiveId = s.ReceiveId,
                        s.Cost,
                        s.Cost1,
                        s.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectRMAProductNameByComplainReceive(long companyId, string query, Guid complainReceiveId)
        {
            try
            {
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);
                return iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.Setup_Product.Name.ToLower().Contains(query.ToLower())
                        || x.Setup_Product.Code.ToLower().Contains(query.ToLower()))
                    .WhereIf(complainReceiveId != Guid.Empty, x => x.Task_ComplainReceive.ReceiveId == complainReceiveId)
                    .Select(s => new
                    {
                        Item = "[" + s.Setup_Product.Code + "] " + s.Setup_Product.Name.ToString(),
                        Value = s.ProductId.ToString(),
                        isSerialProduct = s.Setup_Product.SerialAvailable,
                        s.Setup_Product.Code,
                        complainReceiveId,
                        s.Cost,
                        s.Cost1,
                        s.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductBySerialFromComplainReceive(long companyId, long LocationId, string serial, long ProductId, Guid complainReceiveId)
        {
            try
            {
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);
                return iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .WhereIf(complainReceiveId != Guid.Empty, x => x.Task_ComplainReceive.ReceiveId == complainReceiveId)
                    .WhereIf(ProductId != 0, x => x.ProductId == ProductId)
                    .WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower())
                    || x.AdditionalSerial.ToLower().Contains(serial.ToLower())
                    )
                    .Where(x => x.Task_ComplainReceive.Approved.Equals("A"))
                    .OrderBy(o => o.Serial)
                    .Select(s => new
                    {
                        Item = s.Serial.ToString(),
                        Value = s.Serial.ToString(),
                        ProductName = "[" + s.Setup_Product.Code + "] " + s.Setup_Product.Name.ToString(),
                        ProductId = s.ProductId.ToString(),
                        s.Setup_Product.Code,
                        complainReceiveId = s.ReceiveId,
                        s.Cost,
                        s.Cost1,
                        s.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectRMAProductBySerialFromComplainReceive(long companyId, long LocationId, string serial, long ProductId, Guid complainReceiveId)
        {
            try
            {
                ISelectTaskComplainReceiveDetail iSelectTaskComplainReceiveDetail = new DSelectTaskComplainReceiveDetail(companyId);

                return iSelectTaskComplainReceiveDetail.SelectTaskComplainReceiveDetailAll()
                    .WhereIf(complainReceiveId != Guid.Empty, x => x.ReceiveId == complainReceiveId)
                    .WhereIf(!string.IsNullOrEmpty(serial), x => x.Serial.ToLower().Contains(serial.ToLower()))
                    .Where(x => x.Task_ComplainReceive.Approved.Equals("A"))
                    .OrderBy(o => o.Serial)
                    .Select(s => new
                    {
                        Item = s.Serial.ToString(),
                        Value = s.Serial.ToString(),
                        ProductName = "[" + s.Setup_Product.Code + "] " + s.Setup_Product.Name.ToString(),
                        ProductId = s.ProductId.ToString(),
                        s.Setup_Product.Code,
                        complainReceiveId = s.ReceiveId,
                        s.Cost,
                        s.Cost1,
                        s.Cost2
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectComplainReceiveNo(long companyId, long locationId,string query, string ApprovalStatus)
        {
            try
            {
                ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);

                return iSelectTaskComplainReceive.SelectComplainReceiveAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ReceiveNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(!string.IsNullOrEmpty(ApprovalStatus),x=> x.Approved == ApprovalStatus)
                    .OrderBy(o => o.ReceiveNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.ReceiveNo.ToString(),
                        Value = s.ReceiveId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectComplainReceiveNoByCompanyIdForDropdown(long companyId, long locationId, string query)
        {
            try {
                string ApprovalStatus = "";
                return SelectComplainReceiveNo(companyId, locationId, query, ApprovalStatus);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}