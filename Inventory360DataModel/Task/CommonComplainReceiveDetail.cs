using System;
using System.Collections.Generic;

namespace Inventory360DataModel.Task
{
    public class CommonComplainReceiveDetail
    {
        public Guid ReceiveDetailId { get; set; }
        public Guid ReceiveId { get; set; }
        public Nullable<Guid> InvoiceId { get; set; }
        public long ProductId { get; set; }
        public Nullable<long> ProductDimensionId { get; set; }
        public long UnitTypeId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }
        public bool IsWarrantyAvailable { get; set; }
        public bool IsServiceWarranty { get; set; }
        public bool IsOnlyService { get; set; }
        public decimal Cost { get; set; }
        public decimal Cost1 { get; set; }
        public decimal Cost2 { get; set; }
        public string Remarks { get; set; }
        public List<CommonComplainReceiveDetail_Problem> ComplainReceiveDetail_Problem { get; set; }       
        public List<CommonComplainReceiveDetail_SpareProduct> ComplainReceiveDetail_SpareProduct { get; set; }

    }
}