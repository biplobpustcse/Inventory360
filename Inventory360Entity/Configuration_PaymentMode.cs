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
    
    public partial class Configuration_PaymentMode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Configuration_PaymentMode()
        {
            this.Configuration_OperationalEventDetail = new HashSet<Configuration_OperationalEventDetail>();
            this.Configuration_OperationalEventDetail1 = new HashSet<Configuration_OperationalEventDetail>();
            this.Task_CollectionDetail = new HashSet<Task_CollectionDetail>();
            this.Task_PaymentDetail = new HashSet<Task_PaymentDetail>();
            this.Task_PurchaseOrder = new HashSet<Task_PurchaseOrder>();
            this.Task_SalesOrder = new HashSet<Task_SalesOrder>();
            this.Task_SalesQuotation = new HashSet<Task_SalesQuotation>();
            this.Temp_CollectionDetail = new HashSet<Temp_CollectionDetail>();
        }
    
        public long PaymentModeId { get; set; }
        public string Name { get; set; }
        public bool NeedDetail { get; set; }
        public bool AutoHonor { get; set; }
        public bool IsCashType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration_OperationalEventDetail> Configuration_OperationalEventDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration_OperationalEventDetail> Configuration_OperationalEventDetail1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_CollectionDetail> Task_CollectionDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PaymentDetail> Task_PaymentDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_PurchaseOrder> Task_PurchaseOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesOrder> Task_SalesOrder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesQuotation> Task_SalesQuotation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_CollectionDetail> Temp_CollectionDetail { get; set; }
    }
}
