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
    
    public partial class Setup_AccountsControl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_AccountsControl()
        {
            this.Setup_AccountsCashBankIdentification = new HashSet<Setup_AccountsCashBankIdentification>();
            this.Setup_AccountsSubsidiary = new HashSet<Setup_AccountsSubsidiary>();
        }
    
        public long AccountsControlId { get; set; }
        public long AccountsSubGroupId { get; set; }
        public string Name { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_AccountsCashBankIdentification> Setup_AccountsCashBankIdentification { get; set; }
        public virtual Setup_AccountsSubGroup Setup_AccountsSubGroup { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_AccountsSubsidiary> Setup_AccountsSubsidiary { get; set; }
    }
}