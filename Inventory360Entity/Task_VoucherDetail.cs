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
    
    public partial class Task_VoucherDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task_VoucherDetail()
        {
            this.Task_PostedVoucher = new HashSet<Task_PostedVoucher>();
        }
    
        public System.Guid VoucherDetailId { get; set; }
        public System.Guid VoucherId { get; set; }
        public long AccountsId { get; set; }
        public long ProjectId { get; set; }
        public string Particulars { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency1Debit { get; set; }
        public decimal Currency1Credit { get; set; }
        public decimal Currency2Rate { get; set; }
        public decimal Currency2Debit { get; set; }
        public decimal Currency2Credit { get; set; }
    
        public virtual Setup_Accounts Setup_Accounts { get; set; }
        public virtual Setup_Project Setup_Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PostedVoucher> Task_PostedVoucher { get; set; }
        public virtual Task_Voucher Task_Voucher { get; set; }
    }
}
