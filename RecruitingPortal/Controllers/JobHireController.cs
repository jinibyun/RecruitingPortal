using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class JobHireController : BaseController
    {
        public JobHireController(PortalService<JobPosting> jobPosting, PortalService<JobSeeker> jobSeeker,
                                 PortalService<JobHire> jobHire, PortalService<GuardRequest> guardRequest, PortalService<NotificationQueue> notificationQueue)
        {
            jobPostingService = jobPosting;
            jobSeekerService = jobSeeker;
            jobHireService = jobHire;
            guardRequestService = guardRequest;
            notificationQueueService = notificationQueue;
        }


        public JsonResult ChangeToHire(int jobPostingId, int jobApplyId, string applicantName, string applicantEmail)
        {
            try
            {
                var jobPosting = jobPostingService.Get(x => x.Id == jobPostingId).FirstOrDefault<JobPosting>();
                if (jobPosting != null)
                {
                    // 1. update JobPosting.IsHired
                    jobPosting.IsHired = true;
                    jobPostingService.Change(jobPosting);

                    // 2. insert JobApply
                    jobHireService.Add(new JobHire
                    {
                        JobApplyId = jobApplyId,
                        HiredDate = DateTime.Now,
                        HiredByAspNetUsersId = LoggedInUser.Id
                    });

                    // NotificationQueue
                    try
                    {
                        var subject = string.Format("Applicant <b>{0}</b> has been hired with {1} ", applicantName, jobPosting.JobId);

                        string content = "Job hiring confirmation <br />";
                        content += "<ul style='text-align:left'>";
                        content += "<li>Job Post Id: " + jobPosting.JobId + "</li>";
                        content += "<li>Request Id: " + (jobPosting.GuardRequestId.HasValue ? jobPosting.GuardRequestId.ToString() : "NONE") + "</li>";
                        content += "<li>Position: " + jobPosting.TypeOfPosition.Name + "</li>";
                        content += "<li>Service Type: " + jobPosting.TypeOfService.Name + "</li>";
                        content += "<li>Job Title: " + jobPosting.Title + "</li>";
                        content += "<li>Description: " + jobPosting.Description + "</li>";
                        content += "<li>Province: " + jobPosting.city.Region.Region1 + "</li>";
                        content += "<li>City: " + jobPosting.city.City1 + "</li>";
                        content += "<li>Central Office: " + jobPosting.BranchAddress.Name + "</li>";
                        content += "<li>Post Date: " + String.Format("{0: MM/dd/yyyy hh:mm:ss}", jobPosting.CreateDate) + "</li>";
                        content += "</ul>";
                        content += string.Format("<a href='{0}{1}'>View Job Posting Page</a>", ConfigurationManager.AppSettings["mainHost"], Url.Action("CompanyPostedJob", "JobPosting"));

                        var body = WebUtil.PrepareMailBodyWith("BodyTitle", subject
                                                                , "BodyContent", content
                                                              );

                        // ServiceTeam(Guard Requester), HR Admin and job applicant
                        var emailTo = new List<string>();
                        emailTo.Add(User.Identity.Name);
                        emailTo.Add(applicantEmail);

                        // NOTE: job posting may not have guard request
                        if (jobPosting.GuardRequestId.HasValue)
                        {
                            var guardRequesterEmail = guardRequestService.Get(x => x.Id == (int)jobPosting.GuardRequestId).FirstOrDefault<GuardRequest>().AspNetUser.Email;
                            emailTo.Add(guardRequesterEmail);
                        }

                        AddNotificationQueue(subject, body, emailTo, jobPosting.AspNetUsersId, jobPosting.GuardRequestId, null, jobPosting.Id, EnumNotificationType.JobDeleted);
                        // TempData["message"] += " Job has been successfully hired and the confirmation email has been sent";
                    }
                    catch (Exception ex)
                    {
                        log.Error(this, ex);
                    }

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

    }
}