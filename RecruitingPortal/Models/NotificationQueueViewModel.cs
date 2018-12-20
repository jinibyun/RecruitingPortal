using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class NotificationQueueViewModel
    {
        public int Id { get; set; }
        public int NotificationStatusTypeId { get; set; }
        public int NotificationTypeId { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string EmailCC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? IsBodyHTML { get; set; }
        public byte NumberOfAttempt { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? MemberId { get; set; }
        public string AttachmentFile { get; set; }
        public int NotificationFrequencyTypeId { get; set; }
        public int? JobPostingId { get; set; }
        public int? JobAlertId { get; set; }
        public int? GuardRequestId { get; set; }

        public TypeOfNotificationStatuViewModel TypeOfNotificationStatu { get; set; }
        public NotificationTypeViewModel NotificationType { get; set; }        
        public GuardRequestViewModel GuardRequest { get; set; }
        public JobAlertViewModel JobAlert { get; set; }
        public JobPostingViewModel JobPosting { get; set; }
        public AspNetUsersViewModel AspNetUsers { get; set; }
    }
}