using AutoMapper;
using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;
using RecruitingPortal.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;


namespace RecruitingPortal.Controllers
{
    [Authorize]
    public class JobApplyController : BaseController
    {
        public JobApplyController(PortalService<JobSeeker> jobSeeker, PortalService<JobApply> jobApply,
                                 PortalService<JobPosting> jobPosting, PortalService<NotificationQueue> notificationQueue)
        {
            jobPostingService = jobPosting;
            jobSeekerService = jobSeeker;
            jobApplyService = jobApply;
            notificationQueueService = notificationQueue;
        }

        [Authorize]
        public ViewResult Index(string category, int page = 1)
        {
            return View();
        }

        // Id is job posting id
        public ActionResult AppliedList(int? Id = null)
        {
            try
            {
                if (!Id.HasValue)
                {
                    ModelState.AddModelError("", "Error: jobPostingViewModel is null");
                    throw new Exception();
                }

                var entity = jobApplyService.Get(x => x.JobPostingId == Id).ToList<JobApply>();
                var viewModel = Mapper.Map<List<JobApply>, List<JobApplyViewModel>>(entity);
                /*
                JobPostingViewModel viewModel = new JobPostingViewModel();

                // note: where method implements eager loading
                JobPosting entity = jobPostingService.Get(x => x.Id == (int)Id).FirstOrDefault<JobPosting>();

                viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(entity);
                var commonLibrary = new CommonLibrary();
                viewModel.JobStatus = entity.GuardRequestId.HasValue ? commonLibrary.GetJobStatus(guardRequestService, (int)entity.GuardRequestId) : EnumJobStatus.Posted;
                */
                return View(viewModel);

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult Apply(JobApplyViewModel viewModel, int Id, bool IsRedirectToAction = true)
        {
            try
            {
                // check if job seeker has profile information or not
                var session = WebUtil.GetSession<LoggedInUserViewModel>("LoggedInUserViewModel");
                bool isProfileExist = jobSeekerService.Get(x => x.AspNetUsersId == session.Id).Any();
                if (!isProfileExist)
                {
                    // ref: http://stackoverflow.com/questions/22657335/how-to-show-alert-message-in-mvc-4-controller
                    // return Content("<script language='javascript' type='text/javascript'>alert('You will have to create your profile in order to apply for the job.');window.location.href = '" + ConfigurationManager.AppSettings["locationLink"] + "JobSeeker/CreateOrEdit';</script>");
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet); 
                }

                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<JobApplyViewModel, JobApply>(viewModel);

                    int jobSeekerId = jobSeekerService.Get(x => x.AspNetUsersId == session.Id).FirstOrDefault<JobSeeker>().Id;
                    entity.JobPostingId = Id;
                    entity.JobSeekerId = jobSeekerId;
                    entity.JobApplyStatusId = 1;
                    entity.ViewCount = 1;
                    entity.AppliedDate = DateTime.Now;

                    var newAdded = jobApplyService.Add(entity);

                    if (IsRedirectToAction)
                        return RedirectToAction("Index", "JobPosting");

                    TempData["message"] = "You have successfully applied for the job";

                    // NotificationQueue
                    try
                    {
                        var newJobApply = jobApplyService.Get(x => x.Id == newAdded.Id).FirstOrDefault<JobApply>();
                        var subject = string.Format("Job application has been submitted.");
                        string content = "You have successfully applied for the job. The description is as follows: <br />";
                        content += "<ul style='text-align:left'>";
                        content += "<li>Job Apply Id: " + newJobApply.Id.ToString() + "</li>";
                        content += "<li>Job Post Id: " + newJobApply.JobPosting.JobId.ToString() + "</li>";
                        content += "<li>Position: " + newJobApply.JobPosting.TypeOfPosition.Name + "</li>";
                        content += "<li>Service Type: " + newJobApply.JobPosting.TypeOfService.Name + "</li>";
                        content += "<li>Description: " + newJobApply.JobPosting.Description + "</li>";
                        content += "<li>City: " + newJobApply.JobPosting.city.City1 + "</li>";
                        content += "<li>Central Office: " + newJobApply.JobPosting.BranchAddress.Name + "</li>";
                        content += "<li>Apply Date: " + String.Format("{0: MM/dd/yyyy hh:mm:ss}", newJobApply.AppliedDate) + "</li>";
                        content += "</ul>";
                        content += "<p>Please click <a href='{0}{1}'>here</a> to see applied job</p>";

                        var body = WebUtil.PrepareMailBodyWith(
                                                        "BodyTitle", subject,
                                                        "BodyContent", string.Format(content, ConfigurationManager.AppSettings["mainHost"], Url.Action("Index", "JobApply"))
                                                        );
                        
                        // Only Job Seeker applied
                        var emailTo = new List<string>();
                        emailTo.Add(User.Identity.Name);
                        emailTo.Add(newJobApply.JobPosting.AspNetUser.Email); // job poster email (HR admin)

                        AddNotificationQueue(subject, body, emailTo, session.Id, null, null, newJobApply.JobPosting.Id, EnumNotificationType.JobApplied);
                        TempData["message"] += " and job application confirmation email has been sent";
                    }
                    catch (Exception ex)
                    {
                        log.Error(this, ex);
                    }

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                // ref: http://stackoverflow.com/questions/21682581/return-error-message-with-actionresult
                ModelState.AddModelError("", "Error in applying job");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        public JsonResult GetJobApply(DTParameters param)
        {
            // ref: https://www.echosteg.com/jquery-datatables-asp.net-mvc5-server-side
            try
            {
                if (LoggedInUser == null) RedirectToAction("Index", "Home");
                JobApplyViewModel viewModel = new JobApplyViewModel();

                var viewModels = new List<JobApplyViewModel>();

                var list = jobApplyService.Get(x => x.JobSeeker.AspNetUsersId == LoggedInUser.Id)
                                           .OrderByDescending(x => x.AppliedDate) as IEnumerable<JobApply>;


                if (list != null && list.Count() > 0)
                {
                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<JobApply, JobApplyViewModel>(member);
                        viewModels.Add(viewModel);
                    }
                }

                List<JobApplyViewModel> data = new DataTabelResultSet().GetJobApply(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels);
                int count = new DataTabelResultSet().Count<JobApplyViewModel>(param.Search.Value, viewModels.ToList<JobApplyViewModel>());


                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                data = data.Select(x =>
                {
                    return new JobApplyViewModel()
                    {
                        Id = x.JobPostingId,
                        JobPostingTitle = x.JobPostingTitle,
                        AppliedDate = x.AppliedDate,
                        typeOfServiceName = x.JobPosting.TypeOfService != null ? x.JobPosting.TypeOfService.Name : "",
                        cityName = x.JobPosting.city != null ? x.JobPosting.city.City1 : "",
                        jobApplyCount = x.JobPosting.JobApplies != null ? x.JobPosting.JobApplies.Count : 0,                        

                    };
                }).ToList<JobApplyViewModel>();

                DTResult<JobApplyViewModel> result = new DTResult<JobApplyViewModel>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                var jsonData = Json(result, JsonRequestBehavior.AllowGet);
                return jsonData;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}