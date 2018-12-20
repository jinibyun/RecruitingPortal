using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class TypeOfPositionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<GuardRequestViewModel> GuardRequests { get; set; }
    }
}