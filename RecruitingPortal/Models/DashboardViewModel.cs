using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class ServiceTeamRequestViewModel
    {
        public string Name { get; set; }
        public int RequestCount { get; set; }
    }

    // google chart data view model
    public class AppliedCandidateViewModel
    {
        public string ServiceName { get; set; }
        public int AppliedCount { get; set; }
    }

    public class JobStatusStatisticsViewModel
    {
        public string StatusName { get; set; }
        public int StatusCount { get; set; }
    }

    public class JobPostStatisticsViewModel
    {
        public string ServiceName { get; set; }
        public int PostedCount { get; set; }
    }

    public class RequestGuardStatisticsViewModel
    {
        public string ServiceName { get; set; }
        public int RequestedCount { get; set; }
        public int PostedCount { get; set; }
    }
}