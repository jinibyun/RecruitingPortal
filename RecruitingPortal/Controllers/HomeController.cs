using RecruitingPortal.BLL;
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
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (LoggedInUser == null)
                return RedirectToAction("Login", "Account");

            switch (LoggedInUser.Role)
            {
                case EnumMemberRole.JobSeeker:
                    return RedirectToAction("Index", "JobPosting");
                case EnumMemberRole.Company:
                    return RedirectToAction("Dashboard", "Company");
                case EnumMemberRole.Staff:
                    return RedirectToAction("Dashboard", "ServiceTeam");
            }
            return HttpNotFound();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}