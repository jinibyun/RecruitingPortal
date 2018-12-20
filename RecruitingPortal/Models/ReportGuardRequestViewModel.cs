using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class ReportGuardRequestData
    {
        public List<AllGuardRequestViewModel> AllGuardRequestData { get; set; }
    }

    public class AllGuardRequestViewModel
    {
        public int GuardRequestId { get; set; }
        public string JobId { get; set; }
        public string Title { get; set; }
        public string Requestor { get; set; }
        public string Responder { get; set; }
        public string RequestServiceTypeName { get; set; }
        public string RequestCity { get; set; }
        public string RequestServiceLocation { get; set; }
        public string RequestPostalCode { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? JobPostDate { get; set; }
        public DateTime? CreateDate { get; set; }
    }

}