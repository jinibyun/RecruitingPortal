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
    
    public partial class JobPostingFile
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public string JobFilePath1 { get; set; }
        public string JobFilePath2 { get; set; }
        public string JobFilePath3 { get; set; }
    
        public virtual JobPosting JobPosting { get; set; }
    }
}