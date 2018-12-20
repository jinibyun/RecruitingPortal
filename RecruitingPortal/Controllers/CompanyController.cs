using AutoMapper;
using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RecruitingPortal.Controllers
{
    [Authorize]
    public class CompanyController : BaseController
    {
        public CompanyController(PortalService<Company> service, PortalService<Region> region, 
                                 PortalService<TypeOfWork> typeOfWork, PortalService<GuardRequest> guardRequest)
        {
            companyService = service;
            regionService = region;
            typeOfWorkServiceService = typeOfWork;
            guardRequestService = guardRequest;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard(string fromDate = "", string toDate = "")
        {
            // set default date (to improve performance)
            ViewData["fromDate"] = !string.IsNullOrEmpty(fromDate) ? fromDate : DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            ViewData["toDate"] = !string.IsNullOrEmpty(toDate) ? toDate : DateTime.Now.ToString("yyyy-MM-dd");

            var guardRequestList = guardRequestService.Get(x => (x.IsDeleted == null || x.IsDeleted == false) 
                                                                && x.CreateDate.Value >= DateTime.Parse(ViewData["fromDate"].ToString())
                                                                && x.CreateDate.Value <= DateTime.Parse(ViewData["toDate"].ToString()) );
            var groupedList = guardRequestList.GroupBy(x => new { x.StaffTeam.Name })
                            .Select(grp => new
                            {
                                Name = grp.Key.Name,
                                Count = grp.Count()
                            }).ToList();
            var results = new List<ServiceTeamRequestViewModel>();
            foreach (var member in groupedList)
            {
                results.Add(new ServiceTeamRequestViewModel
                {
                    Name = member.Name,
                    RequestCount = member.Count
                });
            }

            ViewData["guardRequestListCount"] = guardRequestList.Count;
            return View(results);
        }

        [Authorize]
        public ViewResult CreateOrEdit()
        {
            try
            {
                // TODO: 18 is hard coded (18 means company id of paragon security company)
                var entity = companyService.GetSingle(x => x.Id == 18); 
                CompanyViewModel viewModel = new CompanyViewModel();

                // ref: http://www.c-sharpcorner.com/UploadFile/rohatash/options-for-passing-data-between-controller-to-view-in-mvc3/
                ViewData["PageMode"] = PageMode.CREATE;

                if (entity != null) // profile, will redirect "edit" mode
                {
                    ViewData["PageMode"] = PageMode.EDIT;
                    viewModel = Mapper.Map<Company, CompanyViewModel>(entity);
                    SetLocation<CompanyViewModel>(ref viewModel);
                }
                else
                {
                    // note: even if there is no entity, populate province and city information
                    var listRegion = regionService.Get(x => x.CountryId == CountryId).OrderBy(y => y.Region1).ToList<Region>();
                    listRegion.Insert(0, new Region { RegionId = 0, Region1 = "Select Province" });

                    List<city> listCity = new List<city>();
                    listCity.Insert(0, new city { CityId = 0, City1 = "Select City" });

                    viewModel.RegionsInDropDown = listRegion.Select(x => new SelectListItem { Text = x.Region1, Value = x.RegionId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;
                    viewModel.CitiesInDropDown = listCity.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;  
                }
                
                

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
                    foreach (var lang in entity.CompanyLangs)
                    {
                        checkBoxListLanguages.Where(x => x.Id == lang.Lang.Id).SingleOrDefault<CheckBoxes>().Checked = true;
                    }
                }

                viewModel.CheckBoxListLanguages = checkBoxListLanguages;

                // type of work
                List<CheckBoxes> checkBoxListTypeOfWork = new List<CheckBoxes>();
                var typeOfWorkServices = typeOfWorkServiceService.GetAll();

                foreach (var member in typeOfWorkServices)
                {
                    checkBoxListTypeOfWork.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
                }
                if (entity != null)
                {
                    foreach (var jobtype in entity.CompanyTypeOfWorks)
                    {
                        checkBoxListTypeOfWork.Where(x => x.Id == jobtype.TypeOfWork.Id).SingleOrDefault<CheckBoxes>().Checked = true;
                    }
                }
                viewModel.CheckBoxListTypeOfWork = checkBoxListTypeOfWork;

                //// jobPosting
                //List<RecruitingPortal.Models.JobPostingViewModel> jobPostingViewModels = new List<RecruitingPortal.Models.JobPostingViewModel>();
                //if (viewModel.JobPostings != null && viewModel.JobPostings.Count > 0)
                //{
                //    foreach (var member in viewModel.JobPostings)
                //    {
                //        jobPostingViewModels.Add(Mapper.Map<JobPosting, JobPostingViewModel>(member));
                //    }
                //}

                //ViewData.Add("jobPostingViewModels", jobPostingViewModels.Where(x=> x.IsActive == true && x.IsDeleted == false).OrderByDescending(y=>y.CreateDate).ToList<JobPostingViewModel>());

                return View(viewModel);                
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }

        // POST: JobPosting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // see method "Edit" in this class. Simliar logic
        [ValidateAntiForgeryToken]
        // ref: http://stackoverflow.com/questions/1455528/a-potentially-dangerous-request-form-value-was-detected-from-the-client-asp-ne
        [Authorize]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CreateOrEdit(CompanyViewModel viewModel,
                                    FormCollection collection,
                                    HttpPostedFileBase fileAttachedLicense = null,
                                    HttpPostedFileBase fileAttachedCertificate = null)
        {
            try
            {
                // TODO
                //if (ModelState.IsValid)
                //{                    
                //    var entity = Mapper.Map<CompanyViewModel, Company>(viewModel);

                //    var pageMode = collection["PageMode"] != null ? (PageMode)Enum.Parse(typeof(PageMode), collection["PageMode"].ToString()) : PageMode.CREATE;

                //    UploadFiles = new Dictionary<EnumFilePostType, HttpPostedFileBase>();

                //    // note: extra mapping (manual) is required
                //    var session = WebUtil.GetSession<Member>("Session_Member");
                //    // entity.MemberId = session.Id;

                //    //if (fileAttachedLicense != null)
                //    //    UploadFiles.Add(EnumFilePostType.CompanyLicense, fileAttachedLicense);
                //    //if (fileAttachedCertificate != null)
                //    //    UploadFiles.Add(EnumFilePostType.CompanyCertificate, fileAttachedCertificate);

                //    // TODO: find better way to save parentt's object using child object
                //    var member = memberService.Get(session.Id);
                //    member.DefaultCityId = int.Parse(collection["CityId"]);
                //    memberService.Change(member);
                //    entity.Members.Add(member);
                //    //entity.MemberId = member.Id;

                //    if (pageMode == PageMode.EDIT) // edit mode
                //    {
                //        SetChildTable(viewModel, entity, pageMode, UploadFiles);
                //        companyService.Change(entity);
                //        TempData["message"] = "Company profile has been successfully changed";

                //    }
                //    else // create mode
                //    {
                //        var newAddedCompany = companyService.Add(entity);
                //        SetChildTable(viewModel, newAddedCompany, pageMode, UploadFiles);
                //        TempData["message"] = "Company profile has been successfully created";
                //    }

                //    return RedirectToAction("CreateOrEdit");
                //}


                //// ref: http://stackoverflow.com/questions/5212248/get-error-message-if-modelstate-isvalid-fails
                //// return View(viewModel); // validation error, so redisplay same view
                //var message = string.Join(" | ", ModelState.Values
                //    .SelectMany(v => v.Errors)
                //    .Select(e => e.ErrorMessage));
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest, message);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                throw ex;
            }
        }
    }
}