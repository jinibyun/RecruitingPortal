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
    
    public partial class JobHire
    {
        public int Id { get; set; }
        public int JobApplyId { get; set; }
        public System.DateTime HiredDate { get; set; }
        public string HiredByAspNetUsersId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual JobApply JobApply { get; set; }
    }
}