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
    
    public partial class Temp_AccountsLedgerOrProvisionalLedger
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Temp_AccountsLedgerOrProvisionalLedger()
        {
            this.Temp_AccountsLedgerDetail = new HashSet<Temp_AccountsLedgerDetail>();
        }
    
        public System.Guid TempId { get; set; }
        public string LedgerOrProvisional { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherDescription { get; set; }
        public Nullable<decimal> DrAmount { get; set; }
        public Nullable<decimal> CrAmount { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public Nullable<long> EntryBy { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_AccountsLedgerDetail> Temp_AccountsLedgerDetail { get; set; }
    }
}
