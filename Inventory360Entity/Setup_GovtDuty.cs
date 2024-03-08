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
    
    public partial class Setup_GovtDuty
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Setup_GovtDuty()
        {
            this.Configuration_GovtDutyRate_HSCode = new HashSet<Configuration_GovtDutyRate_HSCode>();
            this.Configuration_GovtDutyRate_Location = new HashSet<Configuration_GovtDutyRate_Location>();
            this.Task_DeliveryChallanDetail_GovtDuty = new HashSet<Task_DeliveryChallanDetail_GovtDuty>();
            this.Task_SalesInvoiceDetail_GovtDuty = new HashSet<Task_SalesInvoiceDetail_GovtDuty>();
            this.Task_SalesOrderDetail_GovtDuty = new HashSet<Task_SalesOrderDetail_GovtDuty>();
            this.Task_SalesQuotationDetail_GovtDuty = new HashSet<Task_SalesQuotationDetail_GovtDuty>();
            this.Temp_SalesInvoiceDetail_GovtDuty = new HashSet<Temp_SalesInvoiceDetail_GovtDuty>();
        }
    
        public long GovtDutyId { get; set; }
        public string DutyName { get; set; }
        public string Description { get; set; }
        public short DutyOrder { get; set; }
        public decimal DefaultValue { get; set; }
        public Nullable<long> AccountsId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration_GovtDutyRate_HSCode> Configuration_GovtDutyRate_HSCode { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Configuration_GovtDutyRate_Location> Configuration_GovtDutyRate_Location { get; set; }
        public virtual Setup_Accounts Setup_Accounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_DeliveryChallanDetail_GovtDuty> Task_DeliveryChallanDetail_GovtDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesInvoiceDetail_GovtDuty> Task_SalesInvoiceDetail_GovtDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesOrderDetail_GovtDuty> Task_SalesOrderDetail_GovtDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task_SalesQuotationDetail_GovtDuty> Task_SalesQuotationDetail_GovtDuty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Temp_SalesInvoiceDetail_GovtDuty> Temp_SalesInvoiceDetail_GovtDuty { get; set; }
    }
}