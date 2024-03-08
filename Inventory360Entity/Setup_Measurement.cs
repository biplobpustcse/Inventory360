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
    
    public partial class Setup_Measurement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_Measurement()
        {
            this.Setup_ProductDimension = new HashSet<Setup_ProductDimension>();
        }
    
        public long MeasurementId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal LengthValue { get; set; }
        public decimal WidthValue { get; set; }
        public decimal HeightValue { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setup_ProductDimension> Setup_ProductDimension { get; set; }
    }
}
