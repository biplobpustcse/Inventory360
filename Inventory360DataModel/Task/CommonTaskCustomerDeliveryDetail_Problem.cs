using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCustomerDeliveryDetail_Problem
    {
        public Guid DeliveryDetailProblemId { get; set; }
        public Guid DeliveryDetailId { get; set; }
        public long ProblemId { get; set; }
        public string Note { get; set; }
    }
}