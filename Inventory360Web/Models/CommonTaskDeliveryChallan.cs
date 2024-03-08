using System;
using System.Collections.Generic;

namespace Inventory360Web.Models
{
    public class CommonTaskDeliveryChallan : CommonHeader
    {
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerName { get; set; }
        public string CustContPerson { get; set; }
        public string CustContPersonNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string DeliveryFrom { get; set; }
        public string WareHouse { get; set; }
        public string DeliveryPlace { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public string TransportName { get; set; }
        public string TransportType { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
        public List<CommonTaskDeliveryChallanDetail> DeliveryChallanDetailLists { get; set; }
    }

    public class CommonTaskDeliveryChallanDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDimension { get; set; }
        public string UnitType { get; set; }
        public decimal Quantity { get; set; }
        public bool SerialAvailable { get; set; }
        public List<CommonTaskProductSerial> SerialLists { get; set; }
    }
}