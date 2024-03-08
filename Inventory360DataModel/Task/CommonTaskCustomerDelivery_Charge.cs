using System;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCustomerDelivery_Charge
    {
        public System.Guid DeliveryChargeId { get; set; }
        public System.Guid DeliveryId { get; set; }
        public long ChargeId { get; set; }
        public long ChargeEventId { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal Charge1Amount { get; set; }
        public decimal Charge2Amount { get; set; }
    }
}