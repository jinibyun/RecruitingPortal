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
    
    public partial class CompanyLang
    {
        public int CompanyId { get; set; }
        public int LangId { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Lang Lang { get; set; }
    }
}
