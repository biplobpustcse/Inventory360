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
    
    public partial class Task_ReplacementClaim
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task_ReplacementClaim()
        {
            this.Task_ReplacementClaimDetail = new HashSet<Task_ReplacementClaimDetail>();
            this.Task_ReplacementReceiveDetail = new HashSet<Task_ReplacementReceiveDetail>();
        }
    
        public System.Guid ClaimId { get; set; }
        public string ClaimNo { get; set; }
        public System.DateTime ClaimDate { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public Nullable<long> RequestedBy { get; set; }
        public long SupplierId { get; set; }
        public string Remarks { get; set; }
        public bool IsSettledByReplacementReceive { get; set; }
        public string Approved { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string CancelReason { get; set; }
        public Nullable<System.Guid> VoucherId { get; set; }
        public long LocationId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Security_User Security_User1 { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        public virtual Setup_Employee Setup_Employee { get; set; }
        public virtual Setup_Location Setup_Location { get; set; }
        public virtual Setup_Supplier Setup_Supplier { get; set; }
        public virtual Task_Voucher Task_Voucher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReplacementClaimDetail> Task_ReplacementClaimDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReplacementReceiveDetail> Task_ReplacementReceiveDetail { get; set; }
    }
}
