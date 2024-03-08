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
    
    public partial class Setup_Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_Project()
        {
            this.Task_PostedVoucher = new HashSet<Task_PostedVoucher>();
            this.Task_VoucherDetail = new HashSet<Task_VoucherDetail>();
        }
    
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string IsActive { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PostedVoucher> Task_PostedVoucher { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_VoucherDetail> Task_VoucherDetail { get; set; }
    }
}