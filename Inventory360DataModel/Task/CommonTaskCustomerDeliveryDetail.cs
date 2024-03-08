using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonTaskCustomerDeliveryDetail
    {
        public System.Guid DeliveryDetailId { get; set; }
        public System.Guid DeliveryId { get; set; }
        public System.Guid ComplainReceiveId { get; set; }
        public long PreviousProductId { get; set; }
        public Nullable<long> PreviousProductDimensionId { get; set; }
        public long PreviousUnitTypeId { get; set; }
        public string PreviousSerial { get; set; }
        public long NewProductId { get; set; }
        public Nullable<long> NewProductDimensionId { get; set; }
        public long NewUnitTypeId { get; set; }
        public string NewSerial { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public bool IsAdjustmentRequired { get; set; }
        public string AdjustmentType { get; set; }
        public decimal AdjustedAmount { get; set; }
        public decimal AdjustedAmount1 { get; set; }
        public decimal AdjustedAmount2 { get; set; }
        public long DeliveryType { get; set; }
        public decimal TotalSpareAmount { get; set; }
        public decimal TotalSpareAmount1 { get; set; }
        public decimal TotalSpareAmount2 { get; set; }
        public decimal TotalSpareDiscount { get; set; }
        public decimal TotalSpareDiscount1 { get; set; }
        public decimal TotalSpareDiscount2 { get; set; }
        public List<CommonTaskCustomerDeliveryDetail_Problem> customerDeliveryDetail_Problem { get; set; }       
        public List<CommonTaskCustomerDeliveryDetail_SpareProduct> customerDeliveryDetail_SpareProduct { get; set; }

    }
}