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
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RecruitingPortal.Controllers
{
    public class JobSeekerController : BaseController
    {
        // TODO: consider this http://stackoverflow.com/questions/14511811/massive-controller-constructor-argument-list-when-using-di-in-mvc
        public JobSeekerController(PortalService<JobSeeker> jobSeeker, PortalService<TypeOfWork> typeOfWork,
                                    PortalService<RecruitingPortal.Domain.TypeOfService> typeOfService,
                                    PortalService<TypeOfSecurityExperience> typeOfSecurityExperience,
                                    PortalService<TypeOfJobNotice> typeOfJobNotice,
                                    PortalService<JobAlert> jobAlert, PortalService<JobSeekerScore> jobSeekerScore,
                                    PortalService<JobSeekerTypeOfWork> jobSeekerTypeOfWork,
                                    PortalService<JobSeekerSecurityExperience> jobSeekerSecurityExperience,
                                    PortalService<JobSeekerLang> jobSeekerLang,
                                    PortalService<Region> region, PortalService<city> city,
                                    PortalService<JobApply> jobApply, PortalService<JobSeekerContactLog> jobSeekerContactLog,
                                    PortalService<JobSeekerFile> jobSeekerFile, PortalService<JobSeekerStatu> jobSeekerStatus
                                    )
        {
            jobSeekerService = jobSeeker;
            typeOfWorkServiceService = typeOfWork;
            typeOfServiceService = typeOfService;
            typeOfSecurityExperienceService = typeOfSecurityExperience;
            typeOfJobNoticeService = typeOfJobNotice;
            jobAlertService = jobAlert;
            jobSeekerScoreService = jobSeekerScore;
            jobSeekerTypeOfWorkService = jobSeekerTypeOfWork;
            jobSeekerSecurityExperienceService = jobSeekerSecurityExperience;
            jobSeekerLangService = jobSeekerLang;
            cityService = city;
            regionService = region;
            jobApplyService = jobApply;
            jobSeekerContactLogService = jobSeekerContactLog;
            jobSeekerFileService = jobSeekerFile;
            jobSeekerStatusService = jobSeekerStatus;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ViewResult CreateOrEdit()
        {
            try
            {
                var entity = jobSeekerService.Get(x => x.AspNetUsersId == LoggedInUser.Id).FirstOrDefault<JobSeeker>();

                JobSeekerViewModel viewModel = new JobSeekerViewModel();

                // ref: http://www.c-sharpcorner.com/UploadFile/rohatash/options-for-passing-data-between-controller-to-view-in-mvc3/
                ViewData["PageMode"] = PageMode.CREATE;

                if (entity != null) // profile, will redirect "edit" mode
                {
                    ViewData["PageMode"] = PageMode.EDIT;
                    viewModel = Mapper.Map<JobSeeker, JobSeekerViewModel>(entity);
                }

                SetLocation<JobSeekerViewModel>(ref viewModel);

                // ref: http://stackoverflow.com/questions/972307/can-you-loop-through-all-enum-values
                // languages
                List<CheckBoxes> checkBoxListLanguages = new List<CheckBoxes>();
                int langId = 1;
                foreach (EnumLang member in Enum.GetValues(typeof(EnumLang)))
                {
                    checkBoxListLanguages.Add(new CheckBoxes { Text = member.ToString(), Id = langId });
                    langId++;
                }
                if (entity != null)
                {
                    foreach (var lang in entity.JobSeekerLangs)
                    {
                        checkBoxListLanguages.Where(x => x.Id == lang.Lang.Id).SingleOrDefault<CheckBoxes>().Checked = true;
                    }
                }

                viewModel.CheckBoxListLanguages = checkBoxListLanguages;

                // type of work
                List<CheckBoxes> checkBoxListTypeOfWork = new List<CheckBoxes>();

                var typeOfWorkServices = typeOfWorkServiceService.GetAll();

                // foreach (EnumTypeOfWork member in Enum.GetValues(typeof(EnumTypeOfWork)))
                foreach (var member in typeOfWorkServices)
                {
                    checkBoxListTypeOfWork.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
                }
                if (entity != null)
                {
                    foreach (var jobtype in entity.JobSeekerTypeOfWorks)
                    {
                        checkBoxListTypeOfWork.Where(x => x.Id == jobtype.TypeOfWork.Id).SingleOrDefault<CheckBoxes>().Checked = true;
                    }
                }
                viewModel.CheckBoxListTypeOfWork = checkBoxListTypeOfWork;


                // type of Security Experience
                List<CheckBoxes> checkBoxListTypeOfSecurityExp = new List<CheckBoxes>();
                var typeOfSecurityExperience = typeOfSecurityExperienceService.Get(x => x.SubId == null).OrderBy(y => y.Seq);
                foreach (var member in typeOfSecurityExperience)
                {
                    checkBoxListTypeOfSecurityExp.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
                }
                if (entity != null)
                {
                    foreach (var sec in entity.JobSeekerSecurityExperiences)
                    {
                        if (checkBoxListTypeOfSecurityExp.Any(x => x.Id == sec.SecurityExperienceId))
                            checkBoxListTypeOfSecurityExp.Where(x => x.Id == sec.SecurityExperienceId).SingleOrDefault<CheckBoxes>().Checked = true;
                    }
                }
                viewModel.CheckBoxListSecurityExperience = checkBoxListTypeOfSecurityExp;


                // sub experiece of Security 
                List<CheckBoxes> checkBoxListTypeOfSecuritySubExp = new List<CheckBoxes>();
                var typeOfSecuritySubExperience = typeOfSecurityExperienceService.Get(x => x.SubId != null).OrderBy(y => y.Seq);
                foreach (var member in typeOfSecuritySubExperience)
                {
                    checkBoxListTypeOfSecuritySubExp.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
                }
                if (entity != null)
                {
                    foreach (var sec in entity.JobSeekerSecurityExperiences)
                    {
                        if (checkBoxListTypeOfSecuritySubExp.Any(x => x.Id == sec.SecurityExperienceId))
                            checkBoxListTypeOfSecuritySubExp.Where(x => x.Id == sec.SecurityExperienceId).SingleOrDefault<CheckBoxes>().Checked = true;
                    }
                }
                viewModel.CheckBoxListSecuritySubExperience = checkBoxListTypeOfSecuritySubExp;


                // education level
                List<SelectListItem> educationLevels = new List<SelectListItem>();
                foreach (EnumEducationLevel member in Enum.GetValues(typeof(EnumEducationLevel)))
                {
                    educationLevels.Add(new SelectListItem { Text = member.ToShortString(), Value = ((int)member).ToString() });
                }
                if (entity != null)
                {
                    educationLevels.Where(x => x.Value == entity.EducationLevelId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
                }
                viewModel.EducationLevels = educationLevels;

                // job seeker status
                List<SelectListItem> seekerStatus = new List<SelectListItem>();
                seekerStatus.Add(new SelectListItem { Text = "Select Status", Value = "" });
                var jobSeekerStatusList = jobSeekerStatusService.GetAll();
                foreach (var member in jobSeekerStatusList)
                {
                    seekerStatus.Add(new SelectListItem { Text = member.Name.ToString(), Value = member.Id.ToString() });
                }
                if (entity != null)
                {
                    seekerStatus.Where(x => x.Value == entity.JobSeekerStatusId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
                }
                viewModel.StatusList = seekerStatus;

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

                // job notice type
                List<SelectListItem> typeofNotices = new List<SelectListItem>();
                var typeOfNotices = typeOfJobNoticeService.GetAll();
                foreach (var member in typeOfNotices)
                {
                    typeofNotices.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
                }
                if (entity != null)
                {
                    if (entity.TypeOfNoticeId.HasValue)
                        typeofNotices.Where(x => x.Value == entity.TypeOfNoticeId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
                }
                viewModel.TypeOfNotices = typeofNotices;


                // job seeker status
                var jobApplyViewModels = new List<JobApplyViewModel>();

                if (viewModel.JobApplies != null && viewModel.JobApplies.Count > 0)
                {
                    foreach (var apply in viewModel.JobApplies)
                    {
                        jobApplyViewModels.Add(Mapper.Map<JobApply, JobApplyViewModel>(apply));
                    }
                }

                ViewData["JobApplies"] = jobApplyViewModels;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateOrEdit(JobSeekerViewModel viewModel,
                                    FormCollection collection,
                                    HttpPostedFileBase fileAttachedResume = null
                //HttpPostedFileBase SecurityLicenseCertificate = null,
                //HttpPostedFileBase Eligibility = null,
                //HttpPostedFileBase CPRFirstAid = null,
                //HttpPostedFileBase Education = null
                )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var pageMode = collection["PageMode"] != null ? (PageMode)Enum.Parse(typeof(PageMode), collection["PageMode"].ToString()) : PageMode.CREATE;
                    if (pageMode == PageMode.EDIT) // edit mode
                    {
                        var entity = Mapper.Map<JobSeekerViewModel, JobSeeker>(viewModel);
                        entity.AspNetUsersId = LoggedInUser.Id;
                        UploadFiles = new Dictionary<EnumFilePostType, HttpPostedFileBase>();

                        if (fileAttachedResume != null)
                            UploadFiles.Add(EnumFilePostType.Resume, fileAttachedResume);
                        //if (SecurityLicenseCertificate != null)
                        //    UploadFiles.Add(EnumFilePostType.SecurityLicenseCertificate, SecurityLicenseCertificate);
                        //if (Eligibility != null)
                        //    UploadFiles.Add(EnumFilePostType.EligibilityFile, Eligibility);
                        //if (CPRFirstAid != null)
                        //    UploadFiles.Add(EnumFilePostType.CPRFirstAidCertificate, CPRFirstAid);
                        //if (Education != null)
                        //    UploadFiles.Add(EnumFilePostType.EducationFile, Education);

                        // TODO: find better way to save parentt's object using child object
                        entity.DefaultCityId = int.Parse(collection["CityId"]);
                        entity.EducationLevelId = viewModel.EducationLevel.Id;
                        entity.JobSeekerStatusId = viewModel.JobSeekerStatu.Id; // (int)EnumJobSeekerStatus.Open; //viewModel.JobSeekerStatusId;
                        entity.TypeOfServiceId = viewModel.TypeOfService.Id;
                        entity.UpdateDate = DateTime.Now;

                        SetChildTable(viewModel, entity, pageMode, UploadFiles);
                        jobSeekerService.Change(entity);

                        SetScore(entity);
                        TempData["message"] = "Your profile has been successfully updated";
                    }
                    else // create mode
                    {
                        var jobSeeker = jobSeekerService.GetSingle(x => x.Id == viewModel.Id);
                        if (viewModel.Id > 0 && jobSeeker != null)
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Job seeker already exist.");

                        var entity = Mapper.Map<JobSeekerViewModel, JobSeeker>(viewModel);
                        entity.AspNetUsersId = LoggedInUser.Id;
                        UploadFiles = new Dictionary<EnumFilePostType, HttpPostedFileBase>();

                        if (fileAttachedResume != null)
                            UploadFiles.Add(EnumFilePostType.Resume, fileAttachedResume);
                        //if (SecurityLicenseCertificate != null)
                        //    UploadFiles.Add(EnumFilePostType.SecurityLicenseCertificate, SecurityLicenseCertificate);
                        //if (Eligibility != null)
                        //    UploadFiles.Add(EnumFilePostType.EligibilityFile, Eligibility);
                        //if (CPRFirstAid != null)
                        //    UploadFiles.Add(EnumFilePostType.CPRFirstAidCertificate, CPRFirstAid);
                        //if (Education != null)
                        //    UploadFiles.Add(EnumFilePostType.EducationFile, Education);

                        entity.DefaultCityId = int.Parse(collection["CityId"]);
                        entity.EducationLevelId = viewModel.EducationLevel.Id;
                        entity.JobSeekerStatusId = (int)EnumJobSeekerStatus.Open;//viewModel.JobSeekerStatusId;
                        entity.TypeOfServiceId = viewModel.TypeOfService.Id;

                        // note: http://stackoverflow.com/questions/25441027/how-do-i-stop-entity-framework-from-trying-to-save-insert-child-objects
                        entity.EducationLevel = null;
                        entity.TypeOfService = null;

                        entity.CreateDate = entity.UpdateDate = DateTime.Now;
                        var newAddedSeeker = jobSeekerService.Add(entity);
                        SetChildTable(viewModel, newAddedSeeker, pageMode, UploadFiles);

                        SetScore(entity);

                        TempData["message"] = "Your profile has been successfully created";
                    }


                    return RedirectToAction("CreateOrEdit");
                }
                // ref: http://stackoverflow.com/questions/5212248/get-error-message-if-modelstate-isvalid-fails
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }


        private void SetScore(JobSeeker entity)
        {
            int maxScore = 5;
            int minScore = 0;

            var jobSeekerScore = jobSeekerScoreService.Get(x => x.JobSeekerId == entity.Id).FirstOrDefault<JobSeekerScore>();

            if (jobSeekerScore == null) // add
            {
                var addedJobSeekerSocre = new JobSeekerScore
                {
                    JobSeekerId = entity.Id,
                    AvailableAllShift = entity.IsShiftAvailable.HasValue && (bool)entity.IsShiftAvailable ? maxScore : minScore,
                    CurrentlyWorking = entity.IsCurrentlyWorking.HasValue && (bool)entity.IsCurrentlyWorking ? maxScore : minScore,
                    HaveVehicle = entity.HasCar ? maxScore : minScore,
                    FluenInEnglish = entity.JobSeekerLangs.Where(x => x.Lang.Name.Equals("Englisth", StringComparison.InvariantCultureIgnoreCase)).Any() ? maxScore : minScore,
                    ResumeAttached = entity.JobSeekerFiles.Where(x => !string.IsNullOrEmpty(x.ResumeFilePath)).Any() ? maxScore : minScore,
                    SecurityCommunityCollegeCertificate = entity.HasLawSecurityCommunityCollegeCertificate.HasValue && (bool)entity.HasLawSecurityCommunityCollegeCertificate ? maxScore : minScore,
                    SecurityGuardExp = entity.JobSeekerSecurityExperiences.Count > 0 ? maxScore : minScore,
                    SecurityGuardLicense = entity.HasSecurityLicense
                                           && entity.SecurityLicenseExpiry.HasValue
                                           && DateTime.Compare(DateTime.Now, (DateTime)entity.SecurityLicenseExpiry) <= 0 ? maxScore : minScore,
                    ServiceTypeDesired = entity.TypeOfServiceId > 0 ? maxScore : minScore,
                    ValidAidCertificate = entity.HasValidFirstAidCertificate.HasValue && (bool)entity.HasValidFirstAidCertificate ? maxScore : minScore,

                };
                addedJobSeekerSocre.TotalScore = addedJobSeekerSocre.AvailableAllShift +
                                                 addedJobSeekerSocre.CurrentlyWorking +
                                                 addedJobSeekerSocre.HaveVehicle +
                                                 addedJobSeekerSocre.FluenInEnglish +
                                                 addedJobSeekerSocre.PoliceMilitaryExp +
                                                 addedJobSeekerSocre.ResumeAttached +
                                                 addedJobSeekerSocre.SecurityCommunityCollegeCertificate +
                                                 addedJobSeekerSocre.SecurityGuardExp +
                                                 addedJobSeekerSocre.SecurityGuardLicense +
                                                 addedJobSeekerSocre.ServiceTypeDesired +
                                                 addedJobSeekerSocre.ValidAidCertificate;

                jobSeekerScoreService.Add(addedJobSeekerSocre);
            }
            else // update
            {
                jobSeekerScore.AvailableAllShift = entity.IsShiftAvailable.HasValue && (bool)entity.IsShiftAvailable ? maxScore : minScore;
                jobSeekerScore.CurrentlyWorking = entity.IsCurrentlyWorking.HasValue && (bool)entity.IsCurrentlyWorking ? maxScore : minScore;
                jobSeekerScore.HaveVehicle = entity.HasCar ? maxScore : minScore;
                jobSeekerScore.FluenInEnglish = entity.JobSeekerLangs.Where(x => x.Lang.Name.Equals("Englisth", StringComparison.InvariantCultureIgnoreCase)).Any() ? maxScore : minScore;
                jobSeekerScore.ResumeAttached = entity.JobSeekerFiles.Where(x => !string.IsNullOrEmpty(x.ResumeFilePath)).Any() ? maxScore : minScore;
                jobSeekerScore.SecurityCommunityCollegeCertificate = entity.HasLawSecurityCommunityCollegeCertificate.HasValue && (bool)entity.HasLawSecurityCommunityCollegeCertificate ? maxScore : minScore;
                jobSeekerScore.SecurityGuardExp = entity.JobSeekerSecurityExperiences.Count > 0 ? maxScore : minScore;
                jobSeekerScore.SecurityGuardLicense = entity.HasSecurityLicense
                                       && entity.SecurityLicenseExpiry.HasValue
                                       && DateTime.Compare(DateTime.Now, (DateTime)entity.SecurityLicenseExpiry) <= 0 ? maxScore : minScore;
                jobSeekerScore.ServiceTypeDesired = entity.TypeOfServiceId > 0 ? maxScore : minScore;
                jobSeekerScore.ValidAidCertificate = entity.HasValidFirstAidCertificate.HasValue && (bool)entity.HasValidFirstAidCertificate ? maxScore : minScore;
                jobSeekerScore.TotalScore = jobSeekerScore.AvailableAllShift +
                                            jobSeekerScore.CurrentlyWorking +
                                            jobSeekerScore.HaveVehicle +
                                            jobSeekerScore.FluenInEnglish +
                                            jobSeekerScore.PoliceMilitaryExp +
                                            jobSeekerScore.ResumeAttached +
                                            jobSeekerScore.SecurityCommunityCollegeCertificate +
                                            jobSeekerScore.SecurityGuardExp +
                                            jobSeekerScore.SecurityGuardLicense +
                                            jobSeekerScore.ServiceTypeDesired +
                                            jobSeekerScore.ValidAidCertificate;

                jobSeekerScoreService.Change(jobSeekerScore);
            }
        }

        // jquery controller
        public JsonResult GetSecurityGuard(DTParameters param, string ScoringRange = "0", string Region = "0", string City = "0",
                                                  string SeekerServiceType = "0", string ExpYears = "0", string ContactedBy = "")
        {
            // ref: https://www.echosteg.com/jquery-datatables-asp.net-mvc5-server-side
            try
            {
                JobSeekerViewModel viewModel = new JobSeekerViewModel();

                var viewModels = new List<JobSeekerViewModel>();
                var list = jobSeekerService.GetAll()
                                           .OrderBy(x => x.FirstName) as IEnumerable<JobSeeker>;

                // search
                if (int.Parse(ScoringRange) > 0)
                {
                    list = list.Where(x => x.JobSeekerScores.Where(y => y.TotalScore.HasValue && (int)y.TotalScore >= int.Parse(ScoringRange)).Any());
                }
                if (!string.IsNullOrEmpty(ContactedBy))
                {
                    list = list.Where(x => x.JobSeekerContactLogs.Where(y => y.AspNetUsersId.Equals(ContactedBy, StringComparison.InvariantCultureIgnoreCase)).Any());
                }
                if (int.Parse(SeekerServiceType) > 0)
                {
                    list = list.Where(x => x.TypeOfServiceId == int.Parse(SeekerServiceType));
                }
                if (int.Parse(City) > 0)
                {
                    list = list.Where(x => x.DefaultCityId == int.Parse(City));
                }
                if (int.Parse(Region) > 0)
                {
                    list = list.Where(x => x.city != null && x.city.RegionID == int.Parse(Region));
                }

                if (int.Parse(ExpYears) > 0)
                {
                    list = list.Where(x => (int)x.YearsOfExperience >= int.Parse(ExpYears));
                }

                if (list != null && list.Count() > 0)
                {
                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<JobSeeker, JobSeekerViewModel>(member);
                        viewModel.Score = member.JobSeekerScores != null && member.JobSeekerScores.Count == 1 ? member.JobSeekerScores.FirstOrDefault().TotalScore ?? 0 : 0;
                        viewModel.JobSeekerScoreId = member.JobSeekerScores != null && member.JobSeekerScores.Count == 1 ? (int)member.JobSeekerScores.FirstOrDefault().Id : 0;
                        viewModel.JobSeekerContactLogExist = (member.JobSeekerContactLogs != null && member.JobSeekerContactLogs.Count > 0) ? true : false;
                        viewModel.AppliedDate = member.JobApplies != null && member.JobApplies.Count > 0 ? member.JobApplies.OrderByDescending(y => y.AppliedDate).FirstOrDefault().AppliedDate.ToString("MM/dd/yyyy hh:mm:ss tt") : "No History";
                        viewModel.LastJobAppliedId = member.JobApplies != null && member.JobApplies.Count > 0 ? member.JobApplies.OrderByDescending(y => y.AppliedDate).FirstOrDefault().Id : -1;
                        viewModels.Add(viewModel);
                    }
                }

                List<JobSeekerViewModel> data = new DataTabelResultSet().GetJobSeeker(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels.ToList<JobSeekerViewModel>());
                int count = new DataTabelResultSet().Count<JobSeekerViewModel>(param.Search.Value, viewModels.ToList<JobSeekerViewModel>());

                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                data = data.Select(x =>
                {
                    return new JobSeekerViewModel()
                    {
                        Id = x.Id,
                        JobSeekerScoreId = x.JobSeekerScoreId,
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        PostalCode = x.PostalCode,
                        Score = x.Score,
                        AppliedDate = x.AppliedDate,
                        LastJobAppliedId = x.LastJobAppliedId,
                        CityName = x.city.City1,
                        Region = x.city.Region.Region1,
                        JobSeekerContactLogExist = x.JobSeekerContactLogExist

                    };
                }).ToList<JobSeekerViewModel>();

                DTResult<JobSeekerViewModel> result = new DTResult<JobSeekerViewModel>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                var jsonData = Json(result);
                return jsonData;
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        public ActionResult JobSeekerDetail(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                var entity = jobSeekerService.Get(x => x.Id == (int)Id).FirstOrDefault<JobSeeker>();
                var viewModel = Mapper.Map<JobSeeker, JobSeekerViewModel>(entity);
                viewModel.EducationLevel = entity.EducationLevel;
                viewModel.TypeOfService = entity.TypeOfService;

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
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JobSeekerScoreDetail(int Id)
        {
            try
            {
                var entity = jobSeekerScoreService.GetSingle(x => x.Id == Id);
                var viewModel = Mapper.Map<JobSeekerScore, JobSeekerScoreViewModel>(entity);

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
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult JobSeekerHistory(DTParameters param, int Id)
        {
            try
            {
                var entity = jobApplyService.Get(x => x.JobSeekerId == Id).ToList<JobApply>();
                var viewModels = Mapper.Map<List<JobApply>, List<JobApplyViewModel>>(entity);

                if (viewModels == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }

                List<JobApplyViewModel> data = new DataTabelResultSet().GetJobApply(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels.ToList<JobApplyViewModel>());
                int count = new DataTabelResultSet().Count<JobApplyViewModel>(param.Search.Value, viewModels.ToList<JobApplyViewModel>());

                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                data = data.Select(x =>
                {
                    return new JobApplyViewModel()
                    {
                        JobPostingId = x.JobPostingId,
                        JobPostingJobId = x.JobPosting.JobId,
                        JobPostingTitle = x.JobPosting.Title,
                        TotalAppliedNumber = x.JobPosting.JobApplies.Count,
                        AppliedDate = x.AppliedDate,
                        PostingDate = (DateTime)x.JobPosting.CreateDate,
                        PostingIsActive = (bool)x.JobPosting.IsActive

                    };
                }).ToList<JobApplyViewModel>();

                DTResult<JobApplyViewModel> result = new DTResult<JobApplyViewModel>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                var jsonData = Json(result);
                return jsonData;

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult CreateContactLog(int id)
        {
            try
            {
                var entity = jobSeekerService.Get(x => x.Id == id).FirstOrDefault<JobSeeker>();

                JobSeekerViewModel viewModel = new JobSeekerViewModel();

                viewModel = Mapper.Map<JobSeeker, JobSeekerViewModel>(entity);

                ViewData["MemberId"] = LoggedInUser.Id;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateContactLog(FormCollection collection)
        {
            try
            {
                var jobSeekerId = collection["jobSeekerId"] != null ? int.Parse(collection["jobSeekerId"].ToString()) : 0;
                var contactLog = collection["ContactLog"] != null ? collection["ContactLog"].ToString() : "";
                var fullName = collection["fullName"] != null ? collection["fullName"].ToString() : "";

                if (jobSeekerId < 1)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Job Seeker Id is null or empty");
                }
                if (string.IsNullOrEmpty(contactLog))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Contact log is null or empty");
                }

                jobSeekerContactLogService.Add(new JobSeekerContactLog
                {
                    ContactLog = contactLog,
                    JobSeekerId = jobSeekerId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    IsDeleted = false,
                    AspNetUsersId = LoggedInUser.Id
                });

                TempData["message"] = string.Format("Job seeker ( {0} ) contact log has been successfuly created.", fullName);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        public JsonResult JobSeekerGetContactLog(DTParameters param, int Id)
        {
            try
            {
                var entity = jobSeekerContactLogService.Get(x => x.JobSeekerId == Id).ToList<JobSeekerContactLog>();
                var viewModels = Mapper.Map<List<JobSeekerContactLog>, List<JobSeekerContactLogViewModel>>(entity);

                if (viewModels == null)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }


                List<JobSeekerContactLogViewModel> data = new DataTabelResultSet().GetJobSeekerContactLog(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels.ToList<JobSeekerContactLogViewModel>());
                int count = new DataTabelResultSet().Count<JobSeekerContactLogViewModel>(param.Search.Value, viewModels.ToList<JobSeekerContactLogViewModel>());

                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                data = data.Select(x =>
                {
                    var IdXX = x.Id;
                        var  JobIdXX = x.JobApply != null ? x.JobApply.JobPosting.JobId : "NO";
                    var ContactLogXX = x.ContactLog;
                    var LoggerXX = x.AspNetUser.Email;
                    var JobSeekerFullName = string.IsNullOrEmpty(x.JobSeeker.MiddleName) ? string.Format("{0} {1}", x.JobSeeker.FirstName, x.JobSeeker.LastName) : string.Format("{0} {1} {2}", x.JobSeeker.FirstName, x.JobSeeker.MiddleName, x.JobSeeker.LastName);
                    var CreateDate = (DateTime)x.CreateDate;

                    return new JobSeekerContactLogViewModel()
                    {
                        Id = x.Id,
                        JobId = x.JobApply != null ? x.JobApply.JobPosting.JobId : "NO",
                        ContactLog = x.ContactLog,
                        Logger = x.AspNetUser.Email,
                        JobSeekerFullName = string.IsNullOrEmpty(x.JobSeeker.MiddleName) ? string.Format("{0} {1}", x.JobSeeker.FirstName, x.JobSeeker.LastName) : string.Format("{0} {1} {2}", x.JobSeeker.FirstName, x.JobSeeker.MiddleName, x.JobSeeker.LastName),
                        CreateDate = (DateTime)x.CreateDate,
                    };
                }).ToList<JobSeekerContactLogViewModel>();

                DTResult<JobSeekerContactLogViewModel> result = new DTResult<JobSeekerContactLogViewModel>
                {
                    draw = param.Draw,
                    data = data,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                var jsonData = Json(result);
                return jsonData;

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