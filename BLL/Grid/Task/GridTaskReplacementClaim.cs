using Inventory360DataModel;
using DAL.DataAccess.Select.Task;
using DAL.Interface.Select.Task;
using System;
using System.Linq;

namespace BLL.Grid.Task
{
    public class GridTaskReplacementClaim
    {
        private CommonRecordInformation<dynamic> SelectReplacementClaim(string query, string ReplacementClaimStatus, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                pageSize = pageSize > 100 ? 100 : pageSize;
                int skip = pageSize * (pageIndex - 1);

                ISelectTaskReplacementClaim iSelectTaskReplacementClaim = new DSelectTaskReplacementClaim(companyId);
                var transferOrderLists = iSelectTaskReplacementClaim.SelectTaskReplacementClaimAll()
                    .Where(x => x.LocationId == locationId)
                    .WhereIf(!string.IsNullOrEmpty(query), x => x.ClaimNo.ToLower().Contains(query.ToLower())
                    || x.Setup_Supplier.Name.ToLower().Contains(query.ToLower())
                    || x.Setup_Supplier.Code.ToLower().Contains(query.ToLower())
                    || x.Setup_Supplier.Phone.ToLower().Contains(query.ToLower())
                    )
                    .WhereIf(!string.IsNullOrEmpty(ReplacementClaimStatus), x => x.Approved == "N" || x.Approved == null)
                    .Select(s => new
                    {
                        s.ClaimId,
                        s.ClaimNo,
                        s.ClaimDate,
                        SupplierName = s.Setup_Supplier.Name,
                        SupplierCode = s.Setup_Supplier.Code,
                        SupplierPhoneNo = s.Setup_Supplier.Phone,
                        s.Approved
                    });

                var pagedData = new CommonRecordInformation<dynamic>();
                pagedData.TotalNumberOfRecords = transferOrderLists.Count();
                pagedData.Start = CommonUtility.StartingIndexOfDataGrid((pagedData.TotalNumberOfRecords == 0 ? 0 : pageIndex), pageSize);
                pagedData.End = CommonUtility.EndingIndexOfDataGrid(pagedData.Start, pageSize, pagedData.TotalNumberOfRecords);
                pagedData.LastPageNo = CommonUtility.LastPageNo(pageSize, pagedData.TotalNumberOfRecords);
                pagedData.Data = transferOrderLists
                    .OrderByDescending(o => o.ClaimDate)
                    .ThenByDescending(t => t.ClaimNo)
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
        public object SelectReplacementClaimLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectReplacementClaim(query, string.Empty, locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object SelectUnApprovedReplacementClaimLists(string query, long locationId, long companyId, int pageIndex, int pageSize)
        {
            try
            {
                return SelectReplacementClaim(query, "N", locationId, companyId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectReplacementClaimShortInfoById(Guid claimId, long companyId)
        {
            try
            {
                ISelectTaskReplacementClaim iSelectTaskReplacementClaim = new DSelectTaskReplacementClaim(companyId);

                return iSelectTaskReplacementClaim.SelectTaskReplacementClaimAll()
                    .Where(x => x.ClaimId == claimId)
                    .Select(s => new
                    {
                        s.ClaimId,
                        s.ClaimNo,
                        s.ClaimDate,
                        s.SupplierId,
                        SupplierName = s.Setup_Supplier.Code + " # " + s.Setup_Supplier.Phone + " # " + s.Setup_Supplier.Name,
                        SupplierPhoneNo = s.Setup_Supplier.Phone,
                        SupplierAddress = s.Setup_Supplier.Address
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectReplacementClaimInfoByProduct(long productId, string ProductSerial, long companyId, Guid ReplacementClaimId)
        {
            try
            {
                ISelectTaskReplacementClaimDetail iSelectTaskReplacementClaimDetail = new DSelectTaskReplacementClaimDetail(companyId);

                return iSelectTaskReplacementClaimDetail.SelectReplacementClaimDetailAll()
                    .Where(x => x.ProductId == productId && x.Serial == ProductSerial)
                    .WhereIf(ReplacementClaimId != Guid.Empty, x => x.Task_ReplacementClaim.ClaimId == ReplacementClaimId)
                    .Select(s => new
                    {
                        s.ClaimId,
                        s.ClaimDetailId,
                        s.Task_ReplacementClaim.ClaimNo,
                        s.Task_ReplacementClaim.ClaimDate,
                        s.Task_ReplacementClaim.SupplierId,
                        SupplierName = s.Task_ReplacementClaim.Setup_Supplier.Code + " # " + s.Task_ReplacementClaim.Setup_Supplier.Phone + " # " + s.Task_ReplacementClaim.Setup_Supplier.Name,
                        SupplierPhoneNo = s.Task_ReplacementClaim.Setup_Supplier.Phone,
                        SupplierAddress = s.Task_ReplacementClaim.Setup_Supplier.Address
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object SelectAllReplacementClaimDetail_Problem(long companyId, Guid ClaimDetailId)
        {
            try
            {
                ISelectTaskReplacementClaimDetail_Problem iSelectTaskReplacementClaimDetail_Problem = new DSelectTaskReplacementClaimDetail_Problem(companyId);
                var receivedProblemLists = iSelectTaskReplacementClaimDetail_Problem.SelectReplacementClaimDetail_ProblemAll()
                    .Where(x => x.ClaimDetailId == ClaimDetailId)
                    .Select(s => new
                    {
                        isSelected = true,
                        ProblemId = s.ProblemId,
                        Name = s.Setup_Problem.Name,
                        Note = s.Note
                    })
                    .ToList();

                return receivedProblemLists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}