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
    
    public partial class Task_TransferReceiveNos
    {
        public System.Guid Id { get; set; }
        public string ReceiveNo { get; set; }
        public long Year { get; set; }
        public long CompanyId { get; set; }
    
        public virtual Setup_Company Setup_Company { get; set; }
    }
}
