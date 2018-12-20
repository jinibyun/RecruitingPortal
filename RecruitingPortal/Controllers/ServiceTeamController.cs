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
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    // [Authorize(Roles = "Staff")]
    public class ServiceTeamController : BaseController
    {
        private const int startJobId = 1000;
        // TODO: consider this http://stackoverflow.com/questions/14511811/massive-controller-constructor-argument-list-when-using-di-in-mvc
        public ServiceTeamController(PortalService<JobSeeker> jobSeeker, PortalService<TypeOfWork> typeOfWork,
                                    PortalService<RecruitingPortal.Domain.TypeOfService> typeOfService,
                                    PortalService<TypeOfSecurityExperience> typeOfSecurityExperience,
                                    PortalService<TypeOfJobNotice> typeOfJobNotice,
                                    PortalService<JobSeekerTypeOfWork> jobSeekerTypeOfWork,
                                    PortalService<JobSeekerSecurityExperience> jobSeekerSecurityExperience,
                                    PortalService<JobSeekerLang> jobSeekerLang,
                                    PortalService<Region> region, PortalService<city> city, PortalService<Company> company,
                                    PortalService<JobPosting> jobPosting, PortalService<JobPostingTypeOfWork> jobPostingTypeOfWork,
                                    PortalService<GuardRequest> guardRequest, PortalService<AspNetUser> aspNetUser,
                                    PortalService<TypeOfPosition> typeOfPosition, PortalService<BranchAddress> branchAddress,
                                    PortalService<StaffTeam> staffTeam, PortalService<GuardRequestTypeOfWork> guardRequestTypeofWork,
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
            guardRequestService = guardRequest;
            aspNetUserService = aspNetUser;
            typeOfPositionService = typeOfPosition;
            branchAddressService = branchAddress;
            staffTeamService = staffTeam;
            guardRequestTypeOfWorkService = guardRequestTypeofWork;
            notificationQueueService = notificationQueue;
        }

        public ViewResult CompanyPostedJob()
        {
            JobPostingViewModel viewModel = new JobPostingViewModel();
            var viewModels = new List<JobPostingViewModel>();
            IEnumerable<JobPosting> list = null;
            if (LoggedInUser != null)
            {
                if (LoggedInUser.Role == EnumMemberRole.Company)
                {
                    //if (session.Company != null)
                    //{
                    //    // int companyId = session.Company.Id;
                    //    list = jobPostingService.Where(x => (x.IsDeleted == false || x.IsDeleted == null) ).OrderByDescending(x => x.CreateDate);
                    //}
                    //else // first job posting just after company profile creation or just click "browse my job posting": session.Companies is null
                    //{
                    // TODO: 18 is hard coded
                    var company = companyService.Get(x => x.Id == 18).FirstOrDefault<Company>();
                    if (company != null)
                    {
                        // int companyId = company.Id;
                        list = jobPostingService.Get(x => (x.IsDeleted == false || x.IsDeleted == null)).OrderByDescending(x => x.CreateDate);
                    }
                    //}
                }
            }


            if (list != null && list.Count() > 0)
            {
                foreach (var member in list)
                {
                    viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(member);

                    //// viewModel.JobApplies = member.JobApplies;
                    //JobApplyViewModel applyViewModel = new JobApplyViewModel();
                    //foreach (var apply in member.JobApplies)
                    //{
                    //    applyViewModel = Mapper.Map<JobApply, JobApplyViewModel>(apply);
                    //    viewModel.JobApplies.Add(applyViewModel);
                    //}

                    //viewModel.city = member.city;
                    viewModels.Add(viewModel);
                }
            }

            return View(viewModels);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard(string fromDate = "", string toDate = "")
        {
            ViewData["fromDate"] = !string.IsNullOrEmpty(fromDate) ? fromDate : DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewData["toDate"] = !string.IsNullOrEmpty(toDate) ? toDate : DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [Authorize]
        public ActionResult CreateOrEditGuardRequest(int? Id = null)
        {
            GuardRequestViewModel viewModel = new GuardRequestViewModel();

            ViewData["PageMode"] = PageMode.CREATE;
            GuardRequest entity = null;
            if (Id.HasValue)
            {
                entity = guardRequestService.Get(x => x.Id == (int)Id).FirstOrDefault<GuardRequest>();

                viewModel = Mapper.Map<GuardRequest, GuardRequestViewModel>(entity);
                ViewData["PageMode"] = PageMode.EDIT;
            }

            SetInformation(entity, viewModel);
            return View(viewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateOrEditGuardRequest(GuardRequestViewModel viewModel,
                                         FormCollection collection,
                                         HttpPostedFileBase fileAttached1 = null)
        {
            try
            {
                var entity = Mapper.Map<GuardRequestViewModel, GuardRequest>(viewModel);
                if (ModelState.IsValid)
                {
                    var pageMode = collection["PageMode"] != null ? (PageMode)Enum.Parse(typeof(PageMode), collection["PageMode"].ToString()) : PageMode.CREATE;

                    entity.TypeOfServiceId = viewModel.TypeOfServiceId;
                    entity.TypeOfPositionId = viewModel.TypeOfPositionId;
                    entity.BranchAddressId = viewModel.BranchAddressId;
                    entity.StaffTeamId = viewModel.StaffTeamId;
                    entity.RespondedByAspNetUsersId = null;
                    entity.CreateDate = DateTime.Now;
                    entity.UpdateDate = DateTime.Now;
                    entity.IsDeleted = false;
                    entity.IsResponded = false;
                    entity.AspNetUsersId = LoggedInUser.Id;

                    if (pageMode == PageMode.EDIT) // edit mode
                    {
                        SetChildTable(viewModel, entity, pageMode, UploadFiles);
                        guardRequestService.Change(entity);
                        TempData["message"] = string.Format("A guard request has been successfully updated. The request id is {0}", entity.Id);
                    }
                    else // create mode
                    {
                        var newAdded = guardRequestService.Add(entity);
                        SetChildTable(viewModel, newAdded, PageMode.CREATE, UploadFiles);
                        TempData["message"] = string.Format("A guard request has been successfully created. The request id is {0}", entity.Id);

                        // NotificationQueue
                        try
                        {
                            var newAddedGuard = guardRequestService.Get(x => x.Id == newAdded.Id).FirstOrDefault<GuardRequest>();
                            var subject = "Guard Request has been requested.";

                            string content = "You have successfully requested for the job. The description is as follows: <br />";
                            content += "<ul style='text-align:left'>";
                            content += "<li>Reques Id: " + entity.Id.ToString() + "</li>";
                            content += "<li>Reques Team: " + newAddedGuard.StaffTeam.Name + "</li>";
                            content += "<li>Position: " + newAddedGuard.TypeOfPosition.Name + "</li>";
                            content += "<li>Service Type: " + newAddedGuard.TypeOfService.Name + "</li>";
                            content += "<li>Service Location: " + entity.ServiceLocation + "</li>";
                            content += "<li>Description: " + entity.Description + "</li>";
                            content += "<li>Special Remarks: " + entity.SpecialRemark + "</li>";
                            content += "<li>Central Office: " + newAddedGuard.BranchAddress.Name + "</li>";
                            content += "<li>Request Date: " + String.Format("{0: MM/dd/yyyy hh:mm:ss}", entity.CreateDate) + "</li>";
                            content += string.Format("<a href='{0}{1}'>View Security Guard Request List</a>", ConfigurationManager.AppSettings["mainHost"], Url.Action("Index", "ServiceTeam"));


                            var body = WebUtil.PrepareMailBodyWith(
                                                                "BodyTitle", subject
                                                                ,"BodyContent", content
                                                                );

                            var emailTo = new List<string>();

                            emailTo.Add(User.Identity.Name);
                            var companyAdmins = aspNetUserService.Get(x => x.AspNetRoles.Where(y => y.Name.Equals("company", StringComparison.InvariantCultureIgnoreCase)).Any());
                            foreach(var admin in companyAdmins)
                            {
                                emailTo.Add(admin.Email); // all HR admin email                             
                            }

                            AddNotificationQueue(subject, body, emailTo, entity.AspNetUsersId, entity.Id, null, null, EnumNotificationType.JobRequested);
                            TempData["message"] += " and the confirmation email has been sent";
                        }
                        catch (Exception ex)
                        {
                            log.Error(this, ex);
                        }
                    }
                    return RedirectToAction("Index", "ServiceTeam", new { area = "" });
                }
                else
                {
                    SetInformation(entity, viewModel);

                }
                // ref: http://stackoverflow.com/questions/21682581/return-error-message-with-actionresult
                ModelState.AddModelError("", "Error in requesting security guard");
                return View(viewModel);
            }

            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult DeleteGuard(int Id)
        {
            try
            {
                var entity = guardRequestService.GetSingle(x => x.Id == Id);
                if (entity != null)
                {
                    // note: do not delete but just change flag
                    entity.IsDeleted = true;
                    guardRequestService.Change(entity);

                    TempData["message"] = string.Format("Guard request: {0} was deleted", entity.Id);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        public ActionResult Detail(int? Id = null)
        {
            try
            {
                if (!Id.HasValue)
                {
                    ModelState.AddModelError("", "Error: jobPostingViewModel is null");
                    throw new Exception();
                }

                JobPostingViewModel viewModel = new JobPostingViewModel();

                // note: where method implements eager loading
                JobPosting entity = jobPostingService.Get(x => x.Id == (int)Id).FirstOrDefault<JobPosting>();


                viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(entity);
                return View(viewModel);

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        // jquery controller
        public JsonResult GetGuardRequest(DTParameters param, string PostedOrNotPosted = "")
        {
            // ref: https://www.echosteg.com/jquery-datatables-asp.net-mvc5-server-side
            try
            {
                GuardRequestViewModel viewModel = new GuardRequestViewModel();

                var viewModels = new List<GuardRequestViewModel>();

                IEnumerable<GuardRequest> list = null;
                if (LoggedInUser.Role == EnumMemberRole.Staff)
                {
                    list = guardRequestService.Get(x => x.IsDeleted == false && x.AspNetUsersId == LoggedInUser.Id)
                                           .OrderByDescending(x => x.Id) as IEnumerable<GuardRequest>;
                }
                else if (LoggedInUser.Role == EnumMemberRole.Company)
                {
                    list = guardRequestService.Get(x => x.IsDeleted == false)
                                           .OrderByDescending(x => x.Id) as IEnumerable<GuardRequest>;
                }

                if (!string.IsNullOrEmpty(PostedOrNotPosted))
                {
                    if (string.Equals(PostedOrNotPosted, "jp", StringComparison.InvariantCultureIgnoreCase))
                    {
                        list = list.Where(x => x.IsResponded == true);
                    }
                    else if (string.Equals(PostedOrNotPosted, "jnp", StringComparison.InvariantCultureIgnoreCase))
                    {
                        list = list.Where(x => x.IsResponded == false || x.IsResponded == null);
                    }
                }

                if (list != null && list.Count() > 0)
                {
                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<GuardRequest, GuardRequestViewModel>(member);
                        viewModel.Requestor = member.AspNetUser.Email;
                        if (viewModel.IsResponded.HasValue && (bool)viewModel.IsResponded)
                        {
                            viewModel.RespondedBy = aspNetUserService.GetSingle(x => x.Id == viewModel.RespondedByAspNetUsersId).Email;

                            // var jobPosting = jobPostingService.Where(x => x.GuardRequestId == viewModel.Id).FirstOrDefault<JobPosting>();
                            var jobPostingViewModel = viewModel.JobPostings.FirstOrDefault();
                            if (jobPostingViewModel != null)
                            {
                                viewModel.JobPostId = jobPostingViewModel.Id;
                                viewModel.JobId = jobPostingViewModel.JobId;
                                viewModel.JobPostDate = jobPostingViewModel.CreateDate;
                            }
                        }

                        viewModels.Add(viewModel);
                    }
                }

                List<GuardRequestViewModel> data = new DataTabelResultSet().GetGuardRequest(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels);
                int count = new DataTabelResultSet().Count<GuardRequestViewModel>(param.Search.Value, viewModels.ToList<GuardRequestViewModel>());

                var commonLibrary = new CommonLibrary();
                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                data = data.Select(x =>
                {
                    var jobPostingViewModel = x.JobPostings != null ? x.JobPostings.FirstOrDefault<JobPostingViewModel>() : null;
                    var isHired = jobPostingViewModel != null ? jobPostingViewModel.IsHired : false;
                    var isDeleted = jobPostingViewModel != null ? jobPostingViewModel.IsDeleted : false;
                    var isExpired = jobPostingViewModel != null ? jobPostingViewModel.IsExpired : false;
                    var isApplied = jobPostingViewModel != null ? jobPostingViewModel.JobApplies.Count > 0 : false;
                    var isPosted = jobPostingViewModel != null;

                    return new GuardRequestViewModel()
                    {
                        Id = x.Id,
                        Requestor = x.Requestor,
                        CreateDate = x.CreateDate,
                        JobPostDate = x.JobPostDate,
                        RespondedBy = x.RespondedBy,
                        JobId = x.JobId,
                        JobPostId = x.JobPostId,
                        TypeOfServiceId = x.TypeOfServiceId,
                        IsResponded = x.IsResponded,
                        ServiceLocation = x.ServiceLocation,
                        TypeOfPositionId = x.TypeOfPositionId,
                        JobStatus = commonLibrary.GetJobStatus((bool)isHired, (bool)isDeleted, (bool)isExpired, isApplied, isPosted)

                    };
                }).ToList<GuardRequestViewModel>();

                DTResult<GuardRequestViewModel> result = new DTResult<GuardRequestViewModel>
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

        public ActionResult GetGuardRequestDetail(int Id)
        {
            try
            {
                var entity = guardRequestService.Get(x => x.Id == Id).FirstOrDefault<GuardRequest>();
                // var entity = jobSeekerService.Get((int)Id);
                var viewModel = Mapper.Map<GuardRequest, GuardRequestViewModel>(entity);

                if (viewModel == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var list = JsonConvert.SerializeObject(viewModel,
                            Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            }
                );

                return Content(list, "application/json");

                // return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetBranchAddress(int Id)
        {
            try
            {
                var entity = branchAddressService.Get(x => x.Id == Id).FirstOrDefault<BranchAddress>();
                var viewModel = Mapper.Map<BranchAddress, BranchAddressViewModel>(entity);

                if (viewModel == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var list = JsonConvert.SerializeObject(viewModel,
                            Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                            }
                );

                return Content(list, "application/json");
                // return Json(viewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SetInformation(GuardRequest entity, GuardRequestViewModel viewModel)
        {
            // ref: http://stackoverflow.com/questions/24770756/how-to-set-the-selected-value-in-enumdropdownlistfor
            // type of work
            List<CheckBoxes> checkBoxListTypeOfWork = new List<CheckBoxes>();
            var typeOfWorkServices = typeOfWorkServiceService.GetAll();

            foreach (var member in typeOfWorkServices)
            {
                checkBoxListTypeOfWork.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
            }

            if (entity != null)
            {
                foreach (var jobtype in entity.GuardRequestTypeOfWorks)
                {
                    checkBoxListTypeOfWork.Where(x => x.Id == jobtype.TypeOfWorkId).SingleOrDefault<CheckBoxes>().Checked = true;
                }
            }
            viewModel.CheckBoxListTypeOfWork = checkBoxListTypeOfWork;

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
            viewModel.TypeOfServices = typeOfServices;

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

            // team
            List<SelectListItem> staffTeams = new List<SelectListItem>();
            staffTeams.Add(new SelectListItem { Text = "Select Team", Value = "" });
            var staffs = staffTeamService.GetAll();
            foreach (var member in staffs)
            {
                staffTeams.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }
            if (entity != null)
            {
                staffTeams.Where(x => x.Value == entity.StaffTeamId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
            }
            viewModel.StaffTeams = staffTeams;
        }
    }
}