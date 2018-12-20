using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class TypeOfJobNoticeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<JobSeekerViewModel> JobSeekers { get; set; }
        public ICollection<NotificationQueueViewModel> NotificationQueues { get; set; }
    }
}