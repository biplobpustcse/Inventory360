//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory360Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task_SalesQuotation_DeliveryInfo
    {
        public System.Guid DeliveryInfoId { get; set; }
        public System.Guid QuotationId { get; set; }
        public string DeliveryPlace { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonNo { get; set; }
        public Nullable<long> TransportId { get; set; }
        public Nullable<long> TransportTypeId { get; set; }
        public string VehicleNo { get; set; }
        public string DriverName { get; set; }
        public string DriverContactNo { get; set; }
    
        public virtual Setup_Transport Setup_Transport { get; set; }
        public virtual Setup_TransportType Setup_TransportType { get; set; }
        public virtual Task_SalesQuotation Task_SalesQuotation { get; set; }
    }
}
