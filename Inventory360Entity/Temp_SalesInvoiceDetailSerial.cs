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
    
    public partial class Temp_SalesInvoiceDetailSerial
    {
        public System.Guid InvoiceDetailSerialId { get; set; }
        public System.Guid InvoiceDetailId { get; set; }
        public string Serial { get; set; }
        public string AdditionalSerial { get; set; }
    
        public virtual Temp_SalesInvoiceDetail Temp_SalesInvoiceDetail { get; set; }
    }
}
