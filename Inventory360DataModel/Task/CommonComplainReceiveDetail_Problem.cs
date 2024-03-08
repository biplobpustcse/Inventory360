using System;

namespace Inventory360DataModel.Task
{
    public class CommonComplainReceiveDetail_Problem
    {
        public Guid ReceiveDetailProblemId { get; set; }
        public Guid ReceiveDetailId { get; set; }
        public long ProblemId { get; set; }
        public string Note { get; set; }
    }

    public class CommonComplainReceiveDetailProblemFromDelivery
    {
        public bool isSelected { get; set; }
        public long ProblemId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
    }
}