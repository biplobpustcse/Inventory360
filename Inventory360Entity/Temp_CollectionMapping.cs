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
    
    public partial class Temp_CollectionMapping
    {
        public System.Guid MappingId { get; set; }
        public System.Guid CollectionId { get; set; }
        public System.Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
    
        public virtual Temp_Collection Temp_Collection { get; set; }
        public virtual Temp_SalesInvoice Temp_SalesInvoice { get; set; }
    }
}
