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
    
    public partial class JobSeekerContactLog
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string ContactLog { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string AspNetUsersId { get; set; }
    
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}
