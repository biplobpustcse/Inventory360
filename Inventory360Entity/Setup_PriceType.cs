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
    
    public partial class Setup_PriceType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_PriceType()
        {
            this.Configuration_OperationalEventDetail = new HashSet<Configuration_OperationalEventDetail>();
            this.Setup_Price = new HashSet<Setup_Price>();
        }
    
        public long PriceTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsDetailPrice { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public string RangeType { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal LowerLimit1 { get; set; }
        public decimal LowerLimit2 { get; set; }
        public decimal UpperLimit { get; set; }
        public decimal UpperLimit1 { get; set; }
        public decimal UpperLimit2 { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration_OperationalEventDetail> Configuration_OperationalEventDetail { get; set; }
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_Price> Setup_Price { get; set; }
    }
}
