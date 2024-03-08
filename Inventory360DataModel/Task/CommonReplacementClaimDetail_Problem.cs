using System;

namespace Inventory360DataModel.Task
{
    public class CommonReplacementClaimDetail_Problem
    {
        public Guid ClaimDetailProblemId { get; set; }
        public Guid ClaimDetailId { get; set; }
        public long ProblemId { get; set; }
        public string Note { get; set; }
    }
}