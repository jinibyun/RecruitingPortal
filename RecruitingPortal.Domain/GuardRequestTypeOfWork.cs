//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RecruitingPortal.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class GuardRequestTypeOfWork
    {
        public int GuardRequestId { get; set; }
        public int TypeOfWorkId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual GuardRequest GuardRequest { get; set; }
        public virtual TypeOfWork TypeOfWork { get; set; }
    }
}
