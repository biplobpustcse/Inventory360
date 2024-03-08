using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskReplacementReceive_Charge
    {
        public Guid ReceiveChargeId { get; set; }
        public Guid ReceiveId { get; set; }
        public long ChargeId { get; set; }
        public long ChargeEventId { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal Charge1Amount { get; set; }
        public decimal Charge2Amount { get; set; }
    }
}