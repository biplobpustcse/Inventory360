using System;

namespace Inventory360DataModel.Task
{
    public class CommonComplainReceive_Charge
    {
        public Guid ReceiveChargeId { get; set; }
        public Guid ReceiveId { get; set; }
        public long ChargeEventId { get; set; }
        public long ChargeId { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal Charge1Amount { get; set; }
        public decimal Charge2Amount { get; set; }
    }

    public class CommonComplainReceiveChargeFromDelivery
    {
        public bool isSelected { get; set; }
        public long ChargeId { get; set; }
        public string Name { get; set; }
        public decimal ChargeAmount { get; set; }
    }
}