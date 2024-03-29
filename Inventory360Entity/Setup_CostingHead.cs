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
    
    public partial class Setup_CostingHead
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_CostingHead()
        {
            this.Task_ImportCostingDetail = new HashSet<Task_ImportCostingDetail>();
        }
    
        public long CostingHeadId { get; set; }
        public long CostingControlId { get; set; }
        public string Name { get; set; }
        public Nullable<long> AccountsId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
        public Nullable<long> EditedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Security_User Security_User1 { get; set; }
        public virtual Setup_Accounts Setup_Accounts { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        public virtual Setup_CostingControl Setup_CostingControl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ImportCostingDetail> Task_ImportCostingDetail { get; set; }
    }
}
