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
    
    public partial class Setup_ConvertionRatio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_ConvertionRatio()
        {
            this.Setup_ConvertionRatioDetail = new HashSet<Setup_ConvertionRatioDetail>();
            this.Task_Convertion = new HashSet<Task_Convertion>();
        }
    
        public System.Guid ConvertionRatioId { get; set; }
        public string RatioNo { get; set; }
        public string RatioTitle { get; set; }
        public System.DateTime RatioDate { get; set; }
        public string Description { get; set; }
        public string Approved { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string CancelReason { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Security_User Security_User1 { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_ConvertionRatioDetail> Setup_ConvertionRatioDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_Convertion> Task_Convertion { get; set; }
    }
}
