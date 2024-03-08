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
    
    public partial class Task_PaymentDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task_PaymentDetail()
        {
            this.Task_ChequeInfo = new HashSet<Task_ChequeInfo>();
        }
    
        public System.Guid PaymentDetailId { get; set; }
        public System.Guid PaymentId { get; set; }
        public long PaymentModeId { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public Nullable<System.Guid> VoucherId { get; set; }
    
        public virtual Configuration_PaymentMode Configuration_PaymentMode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ChequeInfo> Task_ChequeInfo { get; set; }
        public virtual Task_Payment Task_Payment { get; set; }
        public virtual Task_Voucher Task_Voucher { get; set; }
    }
}