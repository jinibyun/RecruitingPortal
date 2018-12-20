using AutoMapper;
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
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class JobAlertController : BaseController
    {
        // TODO: consider this http://stackoverflow.com/questions/14511811/massive-controller-constructor-argument-list-when-using-di-in-mvc
        public JobAlertController(PortalService<JobSeeker> jobSeeker, 
                                    PortalService<JobAlert> jobAlert                                
                                    )
        {
            jobSeekerService = jobSeeker;
            jobAlertService = jobAlert;
        }

        [Authorize]
        public ViewResult Index(int? Id = null)
        {
            try
            {
                var jobSeeker = jobSeekerService.Get(x => x.AspNetUsersId == LoggedInUser.Id).FirstOrDefault<JobSeeker>();
                if(jobSeeker == null)
                {
                    return View("NoJobSeekerProfile");
                }

                // frequency
                List<SelectListItem> frequency = new List<SelectListItem>();
                frequency.Add(new SelectListItem { Text = "Select Frequency", Value = "" });
                foreach (EnumFrequency member in Enum.GetValues(typeof(EnumFrequency)))
                {
                    frequency.Add(new SelectListItem { Text = member.ToString(), Value = ((int)member).ToString() });
                }

                // distance
                List<SelectListItem> distance = new List<SelectListItem>();
                distance.Add(new SelectListItem { Text = "Select Ditance", Value = "" });
                foreach (EnumDistance member in Enum.GetValues(typeof(EnumDistance)))
                {
                    string text = string.Empty;
                    if (member.ToString() == "Fifteen")
                    {
                        text = "10 mi (15 km)";
                    }
                    else if (member.ToString() == "Fourty")
                    {
                        text = "25 mi (40 km)";
                    }
                    else if (member.ToString() == "FiftyFive")
                    {
                        text = "35 mi (55 km)";
                    }
                    distance.Add(new SelectListItem { Text = text, Value = ((int)member).ToString() });
                }

                if (Id.HasValue) // edit
                {
                    var entity = jobAlertService.Get(x => x.Id == Id).FirstOrDefault<JobAlert>();
                    var viewModel = Mapper.Map<JobAlert, JobAlertViewModel>(entity);

                    frequency.Where(x => x.Value == entity.JobAlertFrequencyId.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
                    viewModel.Frequencies = frequency;

                    distance.Where(x => x.Value == entity.WithinKilometer.ToString()).SingleOrDefault<SelectListItem>().Selected = true;
                    viewModel.Distance = distance;

                    return View("JobAlertEdit", viewModel);
                }
                else // create and list
                {
                    //var entity = jobAlertService.Get(x => x.JobSeeker.AspNetUsersId == LoggedInUser.Id).FirstOrDefault<JobAlert>();
                    //var viewModel = Mapper.Map<JobAlert, JobAlertViewModel>(entity);
                    
                    var viewModel = new JobAlertViewModel();
                    viewModel.Frequencies = frequency;
                    viewModel.Distance = distance;

                    ViewData["PostalCode"] = jobSeeker.PostalCode;
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateAlert(JobAlertViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (LoggedInUser == null) RedirectToAction("Index", "Home");

                    var entity = Mapper.Map<JobAlertViewModel, JobAlert>(viewModel);
                    var jobSeeker = jobSeekerService.Get(x => x.AspNetUsersId == LoggedInUser.Id).FirstOrDefault<JobSeeker>();

                    entity.JobSeekerId = jobSeeker.Id;
                    entity.JobAlertFrequencyId = viewModel.JobAlertFrequencyId;
                    entity.WithinKilometer = (int)viewModel.WithinKilometer;
                    entity.IsActive = true;
                    entity.IsDeleted = false;
                    entity.CreateDate = DateTime.Now;
                    jobAlertService.Add(entity);

                    TempData["message"] = "Your alert has been successfully created";

                    return RedirectToAction("Index");
                }
                // ref: http://stackoverflow.com/questions/5212248/get-error-message-if-modelstate-isvalid-fails
                // return View(viewModel); // validation error, so redisplay same view
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

        [HttpPost]
        [Authorize]
        public ActionResult EditAlert(JobAlertViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = jobAlertService.Get(x => x.Id == viewModel.Id).FirstOrDefault<JobAlert>();
                    entity.JobAlertFrequencyId = viewModel.JobAlertFrequencyId;
                    entity.WithinKilometer = (int)viewModel.WithinKilometer;
                    entity.Keyword = viewModel.Keyword;
                    jobAlertService.Change(entity);

                    TempData["message"] = "Your alert has been successfully edited";

                    return RedirectToAction("JobAlert");
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

        [HttpPost]
        [Authorize]
        public ActionResult DeleteJobAlert(int Id)
        {
            try
            {
                var entity = jobAlertService.GetSingle(x => x.Id == Id);
                if (entity != null)
                {
                    // note: do not delete but just change flag
                    entity.IsDeleted = true;
                    jobAlertService.Change(entity);

                    TempData["message"] = string.Format("{0} was deleted", entity.Keyword);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        public JsonResult GetJobAlerts(DTParameters param)
        {
            try
            {
                if (LoggedInUser == null) RedirectToAction("Index", "Home");

                var entity = jobAlertService.Get(x => x.JobSeeker.AspNetUsersId == LoggedInUser.Id && x.IsDeleted == false && x.IsActive == true).ToList<JobAlert>();
                var viewModels = Mapper.Map<List<JobAlert>, List<JobAlertViewModel>>(entity);

                List<JobAlertViewModel> data = new DataTabelResultSet().GetJobAlert(param.Search.Value, param.SortOrder, param.Start, param.Length, viewModels.ToList<JobAlertViewModel>());
                int count = new DataTabelResultSet().Count<JobAlertViewModel>(param.Search.Value, viewModels.ToList<JobAlertViewModel>());

                // to avoid circular reference
                // ref: http://stackoverflow.com/questions/14592781/json-a-circular-reference-was-detected-while-serializing-an-object-of-type
                data = data.Select(x =>
                {
                    return new JobAlertViewModel()
                    {
                        Id = x.Id,
                        Keyword = x.Keyword,
                        frequencyString = x.JobAlertFrequency != null ? x.JobAlertFrequency.Name : "",
                        WithinKilometer = x.WithinKilometer.HasValue ? x.WithinKilometer : 0,
                        CreateDate = (DateTime)x.CreateDate,
                    };
                }).ToList<JobAlertViewModel>();

                DTResult<JobAlertViewModel> result = new DTResult<JobAlertViewModel>
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