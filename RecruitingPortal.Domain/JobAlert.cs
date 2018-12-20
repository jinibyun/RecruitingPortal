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
    
    public partial class JobAlert
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobAlert()
        {
            this.NotificationQueues = new HashSet<NotificationQueue>();
        }
    
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int JobAlertFrequencyId { get; set; }
        public string Keyword { get; set; }
        public Nullable<int> WithinKilometer { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        public virtual JobAlertFrequency JobAlertFrequency { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotificationQueue> NotificationQueues { get; set; }
    }
}
