using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskDeliveryChallan
    {
        public Guid ChallanId { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public long CustomerId { get; set; }
        public long? BuyerId { get; set; }
        public Guid SalesOrderId { get; set; }
        public long DeliveryFromId { get; set; }
        public string DeliveryPlace { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public long? TransportId { get; set; }
        public long? TransportTypeId { get; set; }
        public string VehicalNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public List<CommonTaskDeliveryChallanDetail> DeliveryChallanDetailLists { get; set; }
    }
}