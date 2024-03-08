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
    
    public partial class Configuration_EventWiseCharge
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Configuration_EventWiseCharge()
        {
            this.Task_ComplainReceive_Charge = new HashSet<Task_ComplainReceive_Charge>();
            this.Task_CustomerDelivery_Charge = new HashSet<Task_CustomerDelivery_Charge>();
            this.Task_DeliveryChallan_Charge = new HashSet<Task_DeliveryChallan_Charge>();
            this.Task_ReplacementReceive_Charge = new HashSet<Task_ReplacementReceive_Charge>();
            this.Task_SalesInvoice_Charge = new HashSet<Task_SalesInvoice_Charge>();
            this.Task_SalesOrder_Charge = new HashSet<Task_SalesOrder_Charge>();
            this.Task_SalesQuotation_Charge = new HashSet<Task_SalesQuotation_Charge>();
            this.Temp_SalesInvoice_Charge = new HashSet<Temp_SalesInvoice_Charge>();
        }
    
        public long ChargeEventId { get; set; }
        public string EventName { get; set; }
        public long ChargeId { get; set; }
        public Nullable<long> AccountsId { get; set; }
        public long CompanyId { get; set; }
        public long EntryBy { get; set; }
        public System.DateTime EntryDate { get; set; }
    
        public virtual Setup_Charge Setup_Charge { get; set; }
        public virtual Setup_Company Setup_Company { get; set; }
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_Accounts Setup_Accounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ComplainReceive_Charge> Task_ComplainReceive_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_CustomerDelivery_Charge> Task_CustomerDelivery_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_DeliveryChallan_Charge> Task_DeliveryChallan_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_ReplacementReceive_Charge> Task_ReplacementReceive_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesInvoice_Charge> Task_SalesInvoice_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesOrder_Charge> Task_SalesOrder_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesQuotation_Charge> Task_SalesQuotation_Charge { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_SalesInvoice_Charge> Temp_SalesInvoice_Charge { get; set; }
    }
}
