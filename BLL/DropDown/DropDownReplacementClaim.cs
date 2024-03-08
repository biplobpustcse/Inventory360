using Inventory360DataModel;
using DAL.DataAccess.Select.Stock;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Stock;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.DropDown
{
    public class DropDownReplacementClaim
    {
        public object SelectAllReplacementClaimNoByCompanyIdForDropdown(string query, long SupplierId, DateTime? dateFrom, DateTime? dateTo, long companyId, long locationId)
        {
            try
            {
                ISelectTaskReplacementClaim ISelectTaskReplacementClaim = new DSelectTaskReplacementClaim(companyId);

                return ISelectTaskReplacementClaim.SelectTaskReplacementClaimAll()
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ClaimNo.ToLower().Contains(query.ToLower()))
                    .WhereIf(SupplierId != 0, x => x.SupplierId == SupplierId)
                    .WhereIf(dateFrom.HasValue, x => x.ClaimDate >= dateFrom)
                    .WhereIf(dateTo.HasValue, x => x.ClaimDate <= dateTo)
                    .Where(x => x.LocationId == locationId && x.Approved.Equals("A"))
                    .OrderBy(o => o.ClaimNo)
                    .Select(s => new CommonResultList
                    {
                        Item = s.ClaimNo.ToString(),
                        Value = s.ClaimId.ToString()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectProductNameByReplacementClaim(long companyId, string query, Guid ReplacementClaimId)
        {
            try
            {
                ISelectTaskReplacementClaimDetail iSelectTaskReplacementClaimDetail = new DSelectTaskReplacementClaimDetail(companyId);

                return iSelectTaskReplacementClaimDetail.SelectReplacementClaimDetailAll()
                    .WhereIf(ReplacementClaimId != Guid.Empty, x => x.ClaimId == ReplacementClaimId)
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
                        s.ClaimId
                        //s.Cost,
                        //s.Cost1,
                        //s.Cost2
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
            ISelectTaskComplainReceive iSelectTaskComplainReceive = new DSelectTaskComplainReceive(companyId);
            try
            {
                var data = iSelectTaskComplainReceive.SelectRMAProductNameByComplainReceive(companyId, query, complainReceiveId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectProductBySerialFromReplacementClaim(long companyId, long LocationId, string serial, long ProductId, Guid ReplacementClaimId)
        {
            try
            {
                ISelectTaskReplacementClaimDetail iSelectTaskReplacementClaimDetail = new DSelectTaskReplacementClaimDetail(companyId);
                return iSelectTaskReplacementClaimDetail.SelectReplacementClaimDetailAll()
                    .WhereIf(ReplacementClaimId != Guid.Empty, x => x.Task_ReplacementClaim.ClaimId == ReplacementClaimId)
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
                        s.ClaimId
                        //s.Cost,
                        //s.Cost1,
                        //s.Cost2
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
            ISelectStockRMAStock iSelectStockRMAStock = new DSelectStockRMAStock(companyId);
            try
            {
                var data = iSelectStockRMAStock.SelectRMAProductBySerialFromComplainReceive(companyId, LocationId, serial, ProductId, complainReceiveId);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}