using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class JobPostingFileViewModel
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public string JobFilePath1 { get; set; }
        public string JobFilePath2 { get; set; }
        public string JobFilePath3 { get; set; }

        public JobPosting JobPosting { get; set; }
    }
}