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
    
    public partial class Setup_Designation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_Designation()
        {
            this.Setup_Employee = new HashSet<Setup_Employee>();
        }
    
        public long DesignationId { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_Employee> Setup_Employee { get; set; }
    }
}
