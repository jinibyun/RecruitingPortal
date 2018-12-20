using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class TypeOfWorkViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortCutName { get; set; }

        [ScriptIgnore]
        public ICollection<CompanyTypeOfWork> CompanyTypeOfWorks { get; set; }
        [ScriptIgnore]
        public ICollection<JobSeekerTypeOfWork> JobSeekerTypeOfWorks { get; set; }
        [ScriptIgnore]
        public ICollection<JobPostingTypeOfWork> JobPostingTypeOfWorks { get; set; }
    }
}