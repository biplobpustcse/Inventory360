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
    
    public partial class Task_ComplainReceiveDetail_Problem
    {
        public System.Guid ReceiveDetailProblemId { get; set; }
        public System.Guid ReceiveDetailId { get; set; }
        public long ProblemId { get; set; }
        public string Note { get; set; }
    
        public virtual Setup_Problem Setup_Problem { get; set; }
        public virtual Task_ComplainReceiveDetail Task_ComplainReceiveDetail { get; set; }
    }
}
