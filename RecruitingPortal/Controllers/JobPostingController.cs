using AutoMapper;
using Newtonsoft.Json;
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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    [Authorize]
    public class JobPostingController : BaseController
    {
        private string startJobId = DateTime.Now.ToString("yy") + "1000";
        // TODO: consider this http://stackoverflow.com/questions/14511811/massive-controller-constructor-argument-list-when-using-di-in-mvc
        public JobPostingController(PortalService<JobSeeker> jobSeeker, PortalService<TypeOfWork> typeOfWork,
                                    PortalService<RecruitingPortal.Domain.TypeOfService> typeOfService,
                                    PortalService<TypeOfSecurityExperience> typeOfSecurityExperience,
                                    PortalService<TypeOfJobNotice> typeOfJobNotice,
                                    PortalService<JobSeekerTypeOfWork> jobSeekerTypeOfWork,
                                    PortalService<JobSeekerSecurityExperience> jobSeekerSecurityExperience,
                                    PortalService<JobSeekerLang> jobSeekerLang,
                                    PortalService<Region> region, PortalService<city> city, PortalService<Company> company,
                                    PortalService<JobPosting> jobPosting, PortalService<JobPostingTypeOfWork> jobPostingTypeOfWork,
                                    PortalService<JobPostingFile> jobPostingFile, PortalService<TypeOfPosition> typeOfPosition,
                                    PortalService<BranchAddress> branchAddress, PortalService<GuardRequest> guardRequest,
                                    PortalService<NotificationQueue> notificationQueue)
        {
            jobSeekerService = jobSeeker;
            typeOfWorkServiceService = typeOfWork;
            typeOfServiceService = typeOfService;
            typeOfSecurityExperienceService = typeOfSecurityExperience;
            typeOfJobNoticeService = typeOfJobNotice;
            jobSeekerTypeOfWorkService = jobSeekerTypeOfWork;
            jobSeekerSecurityExperienceService = jobSeekerSecurityExperience;
            jobSeekerLangService = jobSeekerLang;
            cityService = city;
            regionService = region;
            companyService = company;
            jobPostingService = jobPosting;
            jobPostingTypeOfWorkService = jobPostingTypeOfWork;
            jobPostingFileService = jobPostingFile;
            typeOfPositionService = typeOfPosition;
            branchAddressService = branchAddress;
            guardRequestService = guardRequest;
            notificationQueueService = notificationQueue;

        }

        public ViewResult CompanyPostedJob()
        {
            return View();
        }

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult CreateOrEdit(int? Id = null, int? RequestId = null, int? TypeOfServiceId = null, int? TypeOfPositionId = null)
        {
            JobPostingViewModel viewModel = new JobPostingViewModel();
            viewModel.GuardRequestId = RequestId;
            ViewData["PageMode"] = PageMode.CREATE;
            JobPosting entity = null;
            GuardRequest entityRequest = null;
            if (Id.HasValue)
            {
                entity = jobPostingService.Get(x => x.Id == (int)Id).FirstOrDefault<JobPosting>();

                viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(entity);

                // note: related object needs manual mapping
                viewModel.city = entity.city;
                ViewData["PageMode"] = PageMode.EDIT;
            }
            else
            {
                if (viewModel.GuardRequestId.HasValue)
                    entityRequest = guardRequestService.Get(x => x.Id == (int)RequestId).FirstOrDefault<GuardRequest>();
            }


            // ref: http://stackoverflow.com/questions/24770756/how-to-set-the-selected-value-in-enumdropdownlistfor
            // type of work
            List<CheckBoxes> checkBoxListTypeOfWork = new List<CheckBoxes>();
            var typeOfWorkServices = typeOfWorkServiceService.GetAll();

            foreach (var member in typeOfWorkServices)
            {
                checkBoxListTypeOfWork.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
            }

            if (entityRequest != null)
            {
                foreach (var jobtype in entityRequest.GuardRequestTypeOfWorks)
                {
                    checkBoxListTypeOfWork.Where(x => x.Id == jobtype.TypeOfWorkId).SingleOrDefault<CheckBoxes>().Checked = true;
                }
            }
            viewModel.CheckBoxListTypeOfWork = checkBoxListTypeOfWork;

            // type of service
            List<SelectListItem> typeOfServices = new List<SelectListItem>();
            typeOfServices.Add(new SelectListItem { Text = "Select Service Type", Value = "" });

            var typeServices = typeOfServiceService.GetAll();
            foreach (var member in typeServices)
            {
                typeOfServices.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }
            if (entity != null)
            {
                typeOfServices.Where(x => x.Value == entity.TypeOfServiceId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
            }
            else
            {
                if (TypeOfServiceId != null)
                {
                    typeOfServices.Where(x => x.Value == TypeOfServiceId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
                }
            }

            viewModel.TypeOfServices = typeOfServices;

            // type of position
            List<SelectListItem> typeOfPositions = new List<SelectListItem>();
            typeOfPositions.Add(new SelectListItem { Text = "Select Position", Value = "" });

            var positions = typeOfPositionService.GetAll();
            foreach (var member in positions)
            {
                typeOfPositions.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }
            if (entity != null)
            {
                typeOfPositions.Where(x => x.Value == entity.TypeOfPositionId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
            }
            viewModel.TypeOfPositions = typeOfPositions;


            // central office
            List<SelectListItem> branches = new List<SelectListItem>();
            branches.Add(new SelectListItem { Text = "Select Office", Value = "" });
            var branchOffices = branchAddressService.GetAll();
            foreach (var member in branchOffices)
            {
                branches.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }
            if (entity != null)
            {
                branches.Where(x => x.Value == entity.BranchAddressId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
            }
            viewModel.BranchOffices = branches;


            if (entity != null)
            {
                viewModel.JobPostingFiles = entity.JobPostingFiles;
            }

            SetLocation<JobPostingViewModel>(ref viewModel);

            return View(viewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateOrEdit(JobPostingViewModel viewModel,
                                         FormCollection collection,
                                         HttpPostedFileBase fileAttached1 = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<JobPostingViewModel, JobPosting>(viewModel);

                    var pageMode = collection["PageMode"] != null ? (PageMode)Enum.Parse(typeof(PageMode), collection["PageMode"].ToString()) : PageMode.CREATE;
                    int? RequestId = null;
                    if (!string.IsNullOrEmpty(collection["GuardRequestId"]))
                    {
                        RequestId = int.Parse(collection["GuardRequestId"].ToString());
                    }


                    UploadFiles = new Dictionary<EnumFilePostType, HttpPostedFileBase>();

                    if (fileAttached1 != null)
                        UploadFiles.Add(EnumFilePostType.JobPosting, fileAttached1);

                    entity.TypeOfServiceId = viewModel.TypeOfServiceId;
                    entity.CityId = viewModel.CityId;
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateDate = DateTime.Now;
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.IsExpired = false;
                    entity.IsHired = false;
                    entity.GuardRequestId = RequestId;
                    entity.AspNetUsersId = LoggedInUser.Id;

                    if (pageMode == PageMode.EDIT) // edit mode
                    {
                        SetChildTable(viewModel, entity, pageMode, UploadFiles);
                        entity.JobId = collection["JobId"].ToString();
                        jobPostingService.Change(entity);
                        TempData["message"] = "A job has been successfully updated";

                    }
                    else // create mode
                    {
                        // get job id
                        var jobPostingList = jobPostingService.GetAll().ToList();
                        var serviceType = typeOfServiceService.GetSingle(x => x.Id == (int)entity.TypeOfServiceId);
                        var jobPostingWithService = jobPostingList.Where(x => x.JobId.StartsWith(serviceType.ShortCutName) && x.JobId.Substring(2, 2) == DateTime.Now.ToString("yy")).ToList<JobPosting>();
                        if (jobPostingWithService.Count > 0)
                        {
                            var maxJobPostingJobId = jobPostingWithService.OrderByDescending(x => x.JobId).First();
                            var maxJobIdInteger = Regex.Match(maxJobPostingJobId.JobId, @"\d+").Value; // retrieve only integer value from job id
                            entity.JobId = string.Format("{0}{1}", serviceType.ShortCutName, int.Parse(maxJobIdInteger) + 1);
                        }
                        else
                        {
                            entity.JobId = string.Format("{0}{1}", serviceType.ShortCutName, startJobId);
                        }

                        var newAdded = jobPostingService.Add(entity);
                        SetChildTable(viewModel, newAdded, PageMode.CREATE, UploadFiles);
                        TempData["message"] = string.Format("A job has been successfully posted. The new job id is {0}", entity.JobId);

                        // NotificationQueue
                        try
                        {
                            var newAddedJobPost = jobPostingService.Get(x => x.Id == newAdded.Id).FirstOrDefault<JobPosting>();
                            var subject = "New job has been posted";

                            string content = "You have successfully posted job. The description is as follows: <br />";
                            content += "<ul style='text-align:left'>";
                            content += "<li>Job Post Id: " + newAddedJobPost.JobId + "</li>";
                            if (newAddedJobPost.GuardRequestId.HasValue)
                                content += "<li>Request Id: " + newAddedJobPost.GuardRequestId.ToString() + "</li>";
                            else
                                content += "<li>Request Id: The job posted without service team request</li>";

                            content += "<li>Position: " + newAddedJobPost.TypeOfPosition.Name + "</li>";
                            content += "<li>Service Type: " + newAddedJobPost.TypeOfService.Name + "</li>";
                            content += "<li>Job Title: " + newAddedJobPost.Title + "</li>";
                            content += "<li>Description: " + newAddedJobPost.Description + "</li>";
                            content += "<li>Province: " + newAddedJobPost.city.Region.Region1 + "</li>";
                            content += "<li>City: " + newAddedJobPost.city.City1 + "</li>";
                            content += "<li>Central Office: " + newAddedJobPost.BranchAddress.Name + "</li>";
                            content += "<li>Post Date: " + String.Format("{0: MM/dd/yyyy hh:mm:ss}", entity.CreateDate) + "</li>";
                            content += "</ul>";
                            content += string.Format("<a href='{0}{1}'>View Job Posting Page</a>", ConfigurationManager.AppSettings["mainHost"], Url.Action("CompanyPostedJob", "JobPosting"));

                            var body = WebUtil.PrepareMailBodyWith("BodyTitle", subject
                                                                    , "BodyContent", content
                                                                  );

                            // ServiceTeam(Guard Requester) and HR
                            var emailTo = new List<string>();
                            if (newAddedJobPost.GuardRequestId.HasValue)
                            {
                                var guardRequesterEmail = guardRequestService.Get(x => x.Id == (int)newAddedJobPost.GuardRequestId).FirstOrDefault<GuardRequest>().AspNetUser.Email;
                                emailTo.Add(guardRequesterEmail);
                            }
                            emailTo.Add(User.Identity.Name);

                            AddNotificationQueue(subject, body, emailTo, entity.AspNetUsersId, entity.GuardRequestId, null, newAddedJobPost.Id, EnumNotificationType.JobPosted);
                            TempData["message"] += " and the confirmation email has been sent";
                        }
                        catch (Exception ex)
                        {
                            log.Error(this, ex);
                        }
                    }

                    return RedirectToAction("CompanyPostedJob", "JobPosting", new { area = "" });
                }
                else
                {
                    // note: after validation (not posting) then if it post, all dropdownvalue gets null. That's why I have bind them again
                    // ref: http://stackoverflow.com/questions/2028709/asp-net-mvc-view-dropdownlist-not-being-binded-to-model-after-submit-post

                    // education level
                    List<SelectListItem> educationLevels = new List<SelectListItem>();
                    educationLevels.Add(new SelectListItem { Text = "Select Education Level", Value = "" });
                    foreach (EnumEducationLevel member in Enum.GetValues(typeof(EnumEducationLevel)))
                    {
                        educationLevels.Add(new SelectListItem { Text = member.ToString(), Value = ((int)member).ToString() });
                    }

                    viewModel.EducationLevels = educationLevels;

                    // type of service
                    List<SelectListItem> typeOfServices = new List<SelectListItem>();
                    typeOfServices.Add(new SelectListItem { Text = "Select Service Type", Value = "" });

                    var typeServices = typeOfServiceService.GetAll();
                    foreach (var member in typeServices)
                    {
                        typeOfServices.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
                    }

                    viewModel.TypeOfServices = typeOfServices;

                }
                // ref: http://stackoverflow.com/questions/21682581/return-error-message-with-actionresult
                ModelState.AddModelError("", "Error in creating job post");
                return View(viewModel);
            }

            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                throw ex;
            }
        }

        public ActionResult SendSMS(string phone, string message, bool IsRedirectToAction = true)
        {
            try
            {
                // TODO
                //if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(message))
                //{
                //    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                //}

                //// var exception = Server.GetLastError();
                //// var message = string.Format("[Job Security Portal] ALERT! It appears the server is having issues. Exception: Go to: http://newrelic.com for more details.");

                //var notifier = new Notifier(new Repository<JobSeeker>(), new RestClient());
                //notifier.SendMessages(phone, message);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        public ActionResult JobPostingDetail(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var entity = jobPostingService.Get(x => x.Id == Id).FirstOrDefault<JobPosting>();
                var viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(entity);
                viewModel.AspNetUsersId = LoggedInUser.Id;
                viewModel.city = entity.city;
                viewModel.EducationLevel = entity.EducationLevel;
                viewModel.TypeOfService = entity.TypeOfService;
                viewModel.JobPostingFiles = entity.JobPostingFiles;
                viewModel.JobPostingTypeOfWorks = entity.JobPostingTypeOfWorks;

                if (viewModel == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var result = JsonConvert.SerializeObject(viewModel,
                            Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            }
                );

                return Content(result);
                // return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetJobPosting(DTParameters param, string Region = "0", string City = "0", string ServiceType = "0", string Title = "", string Description = "", string postalCode = "")
        {
            try
            {
                JobPostingViewModel viewModel = new JobPostingViewModel();

                var viewModels = new List<JobPostingViewModel>();

                var list = (IEnumerable<JobPosting>)jobPostingService.GetAll().OrderByDescending(x => x.Id);

                // search
                if (int.Parse(ServiceType) > 0)
                {
                    list = list.Where(x => x.TypeOfServiceId == int.Parse(ServiceType));
                }
                if (int.Parse(City) > 0)
                {
                    list = list.Where(x => x.CityId == int.Parse(City));
                }
                if (int.Parse(Region) > 0)
                {
                    list = list.Where(x => x.city != null && x.city.RegionID == int.Parse(Region));
                }
                if (!string.IsNullOrEmpty(Title))
                {
                    list = list.Where(x => x.Title.Contains(Title));
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    list = list.Where(x => x.Title.Contains(Description));
                }

                if (list != null && list.Count() > 0)
                {
                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(member);
                        viewModels.Add(viewModel);
                    }
                }

                List<JobPostingViewModel> data = new DataTabelResultSet().GetJobPosting(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels);
                int count = new DataTabelResultSet().Count<JobPostingViewModel>(param.Search.Value, viewModels.ToList<JobPostingViewModel>());

                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type

                var commonLibrary = new CommonLibrary();
                data = data.Select(x =>
                {
                    return new JobPostingViewModel()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        GuardRequestId = x.GuardRequestId,
                        CreateDate = x.CreateDate,
                        JobId = x.JobId,
                        TypeOfServiceId = x.TypeOfServiceId,
                        JobApplyCount = x.JobApplies.Count,
                        cityName = x.city != null ? x.city.City1 : "",
                        typeOfServiceName = x.TypeOfService != null ? x.TypeOfService.Name : "",
                        jobApplied = x.JobApplies.Where(y => y.JobSeeker.AspNetUsersId == LoggedInUser.Id).Any(),
                        IsHired = x.IsHired,
                        IsExpired = x.IsExpired,
                        IsDeleted = x.IsDeleted,
                        JobStatus = commonLibrary.GetJobStatus((bool)x.IsHired, (bool)x.IsDeleted, (bool)x.IsExpired, x.JobApplies.Count > 0, true)

                    };
                }).ToList<JobPostingViewModel>();

                DTResult<JobPostingViewModel> result = new DTResult<JobPostingViewModel>
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

        public JsonResult Delete(string deleteReason, int? Id = null)
        {
            try
            {
                var jobPosting = jobPostingService.Get(x => x.Id == (int)Id).FirstOrDefault<JobPosting>();
                if (jobPosting != null)
                {

                    jobPosting.IsDeleted = true;
                    jobPosting.DeleteReason = deleteReason;
                    jobPosting.DeleteDate = DateTime.Now;

                    jobPostingService.Change(jobPosting);


                    // NotificationQueue
                    try
                    {
                        var subject = string.Format("The job {0} has been removed.", jobPosting.JobId);

                        string content = subject + "<br/>";
                        content += "<ul style='text-align:left'>";
                        content += "<li>Job Post Id: " + jobPosting.JobId + "</li>";
                        content += "<li>Position: " + jobPosting.TypeOfPosition.Name + "</li>";
                        content += "<li>Job Title: " + jobPosting.Title + "</li>";
                        content += "<li>Central Office: " + jobPosting.BranchAddress.Name + "</li>";
                        content += "<li>Post Date: " + String.Format("{0: MM/dd/yyyy hh:mm:ss}", jobPosting.CreateDate) + "</li>";
                        content += "</ul>";

                        var body = WebUtil.PrepareMailBodyWith("BodyTitle", subject
                                                                , "BodyContent", content
                                                              );

                        // ServiceTeam(Guard Requester) and HR
                        var emailTo = new List<string>();
                        var guardRequesterEmail = guardRequestService.Get(x => x.Id == (int)jobPosting.GuardRequestId).FirstOrDefault<GuardRequest>().AspNetUser.Email;

                        emailTo.Add(guardRequesterEmail);
                        emailTo.Add(User.Identity.Name);

                        AddNotificationQueue(subject, body, emailTo, jobPosting.AspNetUsersId, jobPosting.GuardRequestId, null, jobPosting.Id, EnumNotificationType.JobDeleted);
                        // TempData["message"] += " Job has been successfully removed and the confirmation email has been sent";
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