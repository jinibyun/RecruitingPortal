using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class JobApplyViewModel
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public int JobSeekerId { get; set; }
        public int JobApplyStatusId { get; set; }        
        public int? ViewCount { get; set; }        
        public DateTime AppliedDate { get; set; }
        public DateTime PostingDate { get; set; }
        public string JobPostingTitle { get; set; }
        public int TotalAppliedNumber { get; set; }
        public bool PostingIsActive { get; set; }

        public JobApplyStatu JobApplyStatu { get; set; }

        public JobPosting JobPosting { get; set; }

        public JobSeeker JobSeeker { get; set; }

        public ICollection<JobHire> JobHires { get; set; }

        // added
        public string JobPostingJobId { get; set; }
        public string typeOfServiceName { get; set; }
        public string cityName { get; set; }
        public int jobApplyCount { get; set; }
    }
}