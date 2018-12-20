using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class ReportJobPostData
    {
        public List<AllJobPostViewModel> AllJobPostData { get; set; }
    }

    public class AllJobPostViewModel
    {
        public int Id { get; set; }
        public string JobId { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? GuardRequestId { get; set; }
        public string ServiceTypeName { get; set; }
    }

}