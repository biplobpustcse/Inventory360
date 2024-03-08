using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonReplacementClaim : CommonHeader
    {
        public string ClaimNo { get; set; }
        public DateTime ClaimDate { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierName { get; set; }
        public string SupplierAddress { get; set; }        
        public string RequestedBy { get; set; }      
        public string Location { get; set; }
        public string Remarks { get; set; }
        public List<CommonReplacementClaimDetail> ReplacementClaimDetail { get; set; }
        
    }

    public class CommonReplacementClaimDetail
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Serial { get; set; }
        public string Remarks { get; set; }
        public string ReceiveNo { get; set; }
        public DateTime ReceiveDate { get; set; }        
        public List<CommonReplacementClaimDetail_Problem> ReplacementClaimDetail_Problem { get; set; }        
    }
    public class CommonReplacementClaimDetail_Problem
    {
        public System.Guid ClaimDetailId { get; set; }
        public string ProblemName { get; set; }
        public string ProblemNote { get; set; }
    }    
}