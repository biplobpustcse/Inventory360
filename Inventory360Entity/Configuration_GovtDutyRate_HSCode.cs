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
    
    public partial class Configuration_GovtDutyRate_HSCode
    {
        public System.Guid RateId { get; set; }
        public long OperationalEventId { get; set; }
        public long HSCodeId { get; set; }
        public long GovtDutyId { get; set; }
        public string SelectedCurrency { get; set; }
        public decimal Currency1Rate { get; set; }
        public decimal Currency2Rate { get; set; }
        public string RateType { get; set; }
        public decimal RateValue { get; set; }
        public decimal RateValue1 { get; set; }
        public decimal RateValue2 { get; set; }
        public string ExemptedType { get; set; }
        public decimal ExemptedValue { get; set; }
        public decimal ExemptedValue1 { get; set; }
        public decimal ExemptedValue2 { get; set; }
        public string Remarks { get; set; }
        public long EntryBy { get; set; }
    
        public virtual Configuration_OperationalEvent Configuration_OperationalEvent { get; set; }
        public virtual Security_User Security_User { get; set; }
        public virtual Setup_GovtDuty Setup_GovtDuty { get; set; }
        public virtual Setup_HSCode Setup_HSCode { get; set; }
    }
}
