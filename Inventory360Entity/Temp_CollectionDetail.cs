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
    
    public partial class Temp_CollectionDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Temp_CollectionDetail()
        {
            this.Temp_ChequeInfo = new HashSet<Temp_ChequeInfo>();
        }
    
        public System.Guid CollectionDetailId { get; set; }
        public System.Guid CollectionId { get; set; }
        public long PaymentModeId { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
    
        public virtual Configuration_PaymentMode Configuration_PaymentMode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_ChequeInfo> Temp_ChequeInfo { get; set; }
        public virtual Temp_Collection Temp_Collection { get; set; }
    }
}
