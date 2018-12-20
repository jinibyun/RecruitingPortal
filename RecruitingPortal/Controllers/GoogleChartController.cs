using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    [Authorize]
    public class GoogleChartController : BaseController
    {
        public GoogleChartController(PortalService<JobApply> jobApply, IBusinessLayer service) : base(service)
        {
            jobApplyService = jobApply;
        }
        // GET: GoogleChart
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AppliedCandidate(string partialPage, string partialPageDiv, string fromDate, string toDate)
        {
            var result = _service.GetAppliedCandidate<AppliedCandidateViewModel>(fromDate, toDate);

            // convert to google chart format to be rendered properly
            var model = new ChartViewModel()
            {
                Title = "",
                Subtitle = "",
                DataTable = GooglTable.ConstructDataTable(result)
            };

            ViewData["partialPageDiv"] = partialPageDiv;
            return PartialView(string.Format("~/Views/Shared/{0}", partialPage), model);
        }
        
        public PartialViewResult JobStatusStatistics(string partialPage, string partialPageDiv, string fromDate, string toDate)
        {
            var result = _service.GetJobStatusStatistics<JobStatusStatisticsViewModel>(fromDate, toDate);                        

            // convert to google chart format to be rendered properly
            var model = new ChartViewModel()
            {
                Title = "",
                Subtitle = "",
                DataTable = GooglTable.ConstructDataTable(result)
            };

            ViewData["partialPageDiv"] = partialPageDiv;
            return PartialView(string.Format("~/Views/Shared/{0}", partialPage), model);
        }

        public PartialViewResult JobPostStatistics(string partialPage, string partialPageDiv, string fromDate, string toDate)
        {
            var result = _service.GetJobPostStatistics<JobPostStatisticsViewModel>(fromDate, toDate);

            // convert to google chart format to be rendered properly
            var model = new ChartViewModel()
            {
                Title = "",
                Subtitle = "",
                DataTable = GooglTable.ConstructDataTable(result)
            };

            ViewData["partialPageDiv"] = partialPageDiv;
            return PartialView(string.Format("~/Views/Shared/{0}", partialPage), model);
        }

        public PartialViewResult RequestGuardStatistics(string partialPage, string partialPageDiv, string fromDate, string toDate)
        {
            var result = _service.GetRequestGuardStatistics<RequestGuardStatisticsViewModel>(LoggedInUser.Id, fromDate, toDate);

            // convert to google chart format to be rendered properly
            var model = new ChartViewModel()
            {
                Title = "",
                Subtitle = "",
                DataTable = GooglTable.ConstructDataTable(result)
            };

            ViewData["partialPageDiv"] = partialPageDiv;
            return PartialView(string.Format("~/Views/Shared/{0}", partialPage), model);
        }
    }
}