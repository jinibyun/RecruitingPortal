using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class TypeOfServiceViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortCutName { get; set; }

        [ScriptIgnore]
        public ICollection<JobPostingViewModel> JobPostings { get; set; }
        [ScriptIgnore]
        public ICollection<JobSeekerViewModel> JobSeekers { get; set; }
        [ScriptIgnore]
        public ICollection<GuardRequestViewModel> GuardRequests { get; set; }

        // added
        public IEnumerable<System.Web.Mvc.SelectListItem> ItemsInDropDown { get; set; }
    }
}