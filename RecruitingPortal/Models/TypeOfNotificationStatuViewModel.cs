using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class TypeOfNotificationStatuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortCutName { get; set; }

        public ICollection<NotificationQueueViewModel> NotificationQueues { get; set; }
    }
}