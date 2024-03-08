using System;

namespace Inventory360DataModel.Task
{
    public class CommonComplainReceiveDetail_Charge
    {
        public System.Guid ReceiveDetailChargeId { get; set; }
        public System.Guid ReceiveId { get; set; }
        public long ChargeEventId { get; set; }
        public long ChargeId { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal Charge1Amount { get; set; }
        public decimal Charge2Amount { get; set; }
    }
}