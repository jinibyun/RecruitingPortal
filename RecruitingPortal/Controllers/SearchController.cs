using AutoMapper;
using PagedList;
using RecruitingPortal.BLL;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.Domain;
using RecruitingPortal.Infrastructure;
using RecruitingPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RecruitingPortal.Controllers
{
    public class SearchController : BaseController
    {
        public SearchController(PortalService<JobSeeker> jobSeeker, PortalService<TypeOfWork> typeOfWork,
                                    PortalService<RecruitingPortal.Domain.TypeOfService> typeOfService,                                    
                                    PortalService<JobPosting> jobPosting, PortalService<Region> region, PortalService<city> city,
                                    PortalService<JobSeekerContactLog> jobSeekerContactLog)
        {            
            jobSeekerService = jobSeeker;
            typeOfWorkServiceService = typeOfWork;
            typeOfServiceService = typeOfService;
            jobPostingService = jobPosting;            
            regionService = region;
            cityService = city;
            jobSeekerContactLogService = jobSeekerContactLog;
        }

        //TODO: (JINI) called twice ...why??
        // ref: http://stackoverflow.com/questions/6861547/action-called-twice
        public PartialViewResult JobSearch(SearchControlDirection searchControlDirection = SearchControlDirection.Vertical, bool IsAdvancedSearch = false)
        {
            var viewModel = new SearchViewModel();

            // type of service
            List<SelectListItem> typeOfServices = new List<SelectListItem>();
            typeOfServices.Add(new SelectListItem { Text = "Select Service Type", Value = "" });
            
            var typeServices = typeOfServiceService.GetAll();
            foreach (var member in typeServices)
            {
                typeOfServices.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }

            // type of work
            List<CheckBoxes> checkBoxListTypeOfWork = new List<CheckBoxes>();

            var typeOfWorkServices = typeOfWorkServiceService.GetAll();
            
            foreach (var member in typeOfWorkServices)
            {
                checkBoxListTypeOfWork.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
            }

            viewModel.TypeOfServices = typeOfServices;
            viewModel.CheckBoxListTypeOfWork = checkBoxListTypeOfWork;
            // viewModel.JobPostResults = SetViewModel<JobPostingViewModel>();

            if (!IsAdvancedSearch)
            {
                if (searchControlDirection == SearchControlDirection.Vertical)
                {
                    return PartialView("~/Views/Shared/_JobSearch.cshtml", viewModel);
                }
                else
                {
                    return PartialView("~/Views/Shared/_JobSearch_HorizontalDisplay.cshtml", viewModel);
                }
            }
            return PartialView("~/Views/Shared/_AdvancedJobSearch.cshtml", viewModel);

        }

        // ref: http://stackoverflow.com/questions/6861547/action-called-twice
        public PartialViewResult JobSeekerSearch(SearchControlDirection searchControlDirection = SearchControlDirection.Vertical, bool IsAdvancedSearch = false)
        {
            var viewModel = new SearchViewModel();

            // type of service
            List<SelectListItem> typeOfServices = new List<SelectListItem>();
            typeOfServices.Add(new SelectListItem { Text = "Select Service Type", Value = "" });
            
            var typeServices = typeOfServiceService.GetAll();
            foreach (var member in typeServices)
            {
                typeOfServices.Add(new SelectListItem { Text = member.Name, Value = member.Id.ToString() });
            }

            // type of work
            List<CheckBoxes> checkBoxListTypeOfWork = new List<CheckBoxes>();

            var typeOfWorkServices = typeOfWorkServiceService.GetAll();
            foreach (var member in typeOfWorkServices)
            {
                checkBoxListTypeOfWork.Add(new CheckBoxes { Text = member.Name, Id = member.Id });
            }

            //// type of work availability
            //List<CheckBoxes> checkBoxListTypeOfWorkAvailabilities = new List<CheckBoxes>();
            //int typeOfWorkIdAvailability = 1;
            //foreach (EnumTypeOfWorkAvailability member in Enum.GetValues(typeof(EnumTypeOfWorkAvailability)))
            //{
            //    checkBoxListTypeOfWorkAvailabilities.Add(new CheckBoxes { Text = member.ToString(), Id = typeOfWorkIdAvailability });
            //    typeOfWorkIdAvailability++;
            //}

            // type of work
            List<CheckBoxes> checkBoxListAvailableLanguages = new List<CheckBoxes>();
            int availableLangs = 1;
            foreach (EnumLang member in Enum.GetValues(typeof(EnumLang)))
            {
                checkBoxListAvailableLanguages.Add(new CheckBoxes { Text = member.ToString(), Id = availableLangs });
                availableLangs++;
            }

            // contacted by
            List<SelectListItem> contactedBy = new List<SelectListItem>();
            var contactedBys = jobSeekerContactLogService.Get(x => x.IsDeleted == false).GroupBy(x => x.AspNetUser.Email).Select(x => x.First());
            contactedBy.Add(new SelectListItem { Text = "Select Contactor", Value = "" });
            if (contactedBys != null && contactedBys.Count() > 0)
            {
                foreach (var member in contactedBys)
                {
                    contactedBy.Add(new SelectListItem{ Text = member.AspNetUser.Email, Value = member.AspNetUser.Id.ToString() });
                }
            }

            // year of experience
            List<SelectListItem> yearsOfExperiences = new List<SelectListItem>();
            yearsOfExperiences.Add(new SelectListItem { Text = "Select Years", Value = "" });
            yearsOfExperiences.Add(new SelectListItem { Text = "1-3 years", Value = "3" });
            yearsOfExperiences.Add(new SelectListItem { Text = "4-6 years", Value = "6" });
            yearsOfExperiences.Add(new SelectListItem { Text = "7-9 years", Value = "9" });
            yearsOfExperiences.Add(new SelectListItem { Text = "10+ years", Value = "10" });

            // job seeker scoring range
            List<SelectListItem> jobScoringRange = new List<SelectListItem>();
            jobScoringRange.Add(new SelectListItem { Text = "Select Range", Value = "" });
            jobScoringRange.Add(new SelectListItem { Text = "10 point +", Value = "10" });
            jobScoringRange.Add(new SelectListItem { Text = "20 point +", Value = "20" });
            jobScoringRange.Add(new SelectListItem { Text = "30 point +", Value = "30" });
            jobScoringRange.Add(new SelectListItem { Text = "40 point +", Value = "40" });
            jobScoringRange.Add(new SelectListItem { Text = "50 point +", Value = "50" });
            jobScoringRange.Add(new SelectListItem { Text = "60 point +", Value = "60" });

            viewModel.TypeOfServices = typeOfServices;
            viewModel.CheckBoxListTypeOfWork = checkBoxListTypeOfWork;
            // viewModel.CheckBoxListTypeOfWorkAvailabilities = checkBoxListTypeOfWorkAvailabilities;
            viewModel.CheckBoxListLanguages = checkBoxListAvailableLanguages;
            viewModel.ContactedBy = contactedBy;
            viewModel.YearsOfExperience = yearsOfExperiences;
            viewModel.JobScoringRange = jobScoringRange;
            if (!IsAdvancedSearch)
            {
                if (searchControlDirection == SearchControlDirection.Vertical)
                {
                    return PartialView("~/Views/Shared/_JobSeekerSearch.cshtml", viewModel);
                }
                else
                {
                    return PartialView("~/Views/Shared/_JobSeekerSearch_HorizontalDisplay.cshtml", viewModel);
                }
            }
            return PartialView("~/Views/Shared/_AdvancedJobSeekerSearch.cshtml", viewModel);
        }

        [HttpPost]
        public PartialViewResult JobSearch(FormCollection frmCollection, int? page = null, bool ShowGrid = false)
        {
            try
            {
                JobPostingViewModel viewModel = new JobPostingViewModel();
                var viewModels = new List<JobPostingViewModel>();
                IEnumerable<JobPosting> list;
                
                if (LoggedInUser != null)
                {       
                    if(LoggedInUser.Role == EnumMemberRole.Company)
                    {                    
                        list = jobPostingService.Get(x => (x.IsDeleted == false || x.IsDeleted == null)).OrderByDescending(x => x.CreateDate);
                    }
                    else
                    {
                        list = jobPostingService.Get(x => x.IsDeleted == false || x.IsDeleted == null).OrderByDescending(x => x.CreateDate) as IEnumerable<JobPosting>;
                    }
                }
                else
                {
                    list = jobPostingService.Get(x => x.IsDeleted == false || x.IsDeleted == null).OrderByDescending(x => x.CreateDate) as IEnumerable<JobPosting>;
                }                

                if (list != null && list.Count() > 0)
                {
                    if (frmCollection != null)
                    {
                        if (frmCollection["jobNameOrContent"] != null && !string.IsNullOrEmpty(frmCollection["jobNameOrContent"]))
                        {
                            ApplyFilter<JobPosting>(ref list, frmCollection, null);
                        }
                        else
                        {
                            // note: to retain value for each html control after postback
                            ViewData["Title"] = frmCollection["Title"];
                            ViewData["Description"] = frmCollection["Description"];
                            //ViewData["MinRate"] = resultMinRate == 0.0M ? "" : resultMinRate.ToString();
                            //ViewData["MaxRate"] = resultMaxRate == 0.0M ? "" : resultMaxRate.ToString();
                            //ViewData["ServiceType"] = frmCollection["ServiceType"];
                            ViewData["City"] = frmCollection["CityId"];
                            ViewData["Region"] = frmCollection["Region"];

                            ApplyFilter<JobPosting>(ref list, frmCollection, null);
                        }
                        
                    }

                    

                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<JobPosting, JobPostingViewModel>(member);
                        
                        viewModel.AspNetUsersId = LoggedInUser.Id;
                        if(member.city != null)
                            viewModel.city = member.city;
                        if(member.city != null && member.city.Region != null)
                            viewModel.city.Region = member.city.Region;
                        // viewModel.JobApplies = member.JobApplies;
                        JobApplyViewModel applyViewModel = new JobApplyViewModel();
                        foreach (var apply in member.JobApplies)
                        {
                            applyViewModel = Mapper.Map<JobApply, JobApplyViewModel>(apply);
                            viewModel.JobApplies.Add(applyViewModel);
                        }

                        viewModels.Add(viewModel);
                    }
                }

                int currentPageIndex = page.HasValue ? page.Value : 1;
                var pagedList = viewModels.ToPagedList(currentPageIndex, PageSize);

                // http://stackoverflow.com/questions/14516174/can-a-razor-view-have-more-then-one-model-without-using-a-viewmodel-object
                var searchViewModel = new SearchViewModel();
                

                // filtering option
                Dictionary<string, object> filteringOption = new Dictionary<string, object>();

                if (frmCollection["jobNameOrContent"] != null && !string.IsNullOrEmpty(frmCollection["jobNameOrContent"]))
                {                    
                    // searchViewModel.JobNameOrContent = frmCollection["jobNameOrContent"];
                    filteringOption.Add("jobNameOrContent", frmCollection["jobNameOrContent"]);
                }
                else
                {
                    int regionId = !string.IsNullOrEmpty(frmCollection["Region"]) ? int.Parse(frmCollection["Region"]) : 0;
                    var region = regionService.Get(x => x.RegionId == regionId).FirstOrDefault<Region>();
                    var regionName = region != null ? region.Region1: string.Empty ;

                    int cityId = !string.IsNullOrEmpty(frmCollection["CityId"]) ? int.Parse(frmCollection["CityId"]) : 0;
                    var city = cityService.Get(x => x.CityId == cityId).FirstOrDefault<city>();
                    var cityName = city != null ? city.City1 : string.Empty;

                    var typeOfServicename = string.Empty;
                    int serviceTypeId = !string.IsNullOrEmpty(frmCollection["ServiceType"]) ? int.Parse(frmCollection["ServiceType"]) : 0;
                    if(serviceTypeId > 0)
                    {
                        var typeService = typeOfServiceService.GetSingle(x => x.Id == serviceTypeId);
                        typeOfServicename = typeService.Name;
                    }

                    filteringOption.Add("Region", regionName);
                    filteringOption.Add("City", cityName);
                    filteringOption.Add("ServiceType", typeOfServicename);
                    filteringOption.Add("Title", frmCollection["Title"]);
                    filteringOption.Add("Description", frmCollection["Description"]);
                    filteringOption.Add("PostalCode", frmCollection["PostalCode"]);
                    
                    if (!string.IsNullOrEmpty(frmCollection["PostalCode"]))
                    {
                        filteringOption.Add("Distance", frmCollection["distance"] + " km");
                    }
                    else
                    {
                        filteringOption.Add("Distance", string.Empty);
                    }                    
                }

                TempData["searchJobCondition"] = filteringOption;
                // result
                searchViewModel.JobPostResults = pagedList;
                searchViewModel.TotalFilteredJobPostCount = list.Count();

                if (!ShowGrid) // main page (list style): return paged list
                    return PartialView("~/Views/Shared/_JobSearchResult.cshtml", searchViewModel);
                else // grid style: return whole list (not paged list)
                    return PartialView("~/Views/Shared/_JobPostingGrid.cshtml", viewModels);

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return PartialView();
            }
        }

        [HttpPost]
        public PartialViewResult JobSeekerSearch(FormCollection frmCollection, int? page = null, bool ShowGrid = false)
        {
            try
            {
                JobSeekerViewModel viewModel = new JobSeekerViewModel();
                var viewModels = new List<JobSeekerViewModel>();
                
                var list = jobSeekerService.GetAll().OrderByDescending(x => x.UpdateDate) as IEnumerable<JobSeeker>;

                if (list != null && list.Count() > 0)
                {
                    if (frmCollection != null)
                    {
                        if (frmCollection["jobSeekerNameOrContent"] != null && !string.IsNullOrEmpty(frmCollection["jobSeekerNameOrContent"]))
                        {
                            ApplyFilter<JobSeeker>(ref list, frmCollection, null);
                        }
                        else
                        {
                            // note: to retain value for each html control after postback
                            ViewData["JobSeekerName"] = frmCollection["JobSeekerName"];                            
                            ViewData["SeekerServiceType"] = frmCollection["SeekerServiceType"];
                            ViewData["ExpYears"] = frmCollection["ExpYears"];
                            // ViewData["SeekerCity"] = frmCollection["City"];
                            ViewData["SeekerCity"] = frmCollection["CityId"];
                            ViewData["SeekerRegion"] = frmCollection["Region"];
                            ViewData["ScoringRange"] = frmCollection["ScoringRange"];
                            ViewData["ContactedBy"] = frmCollection["ContactedBy"];
                            ApplyFilter<JobSeeker>(ref list, frmCollection, null);
                        }
                    }

                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<JobSeeker, JobSeekerViewModel>(member);
                        //viewModel.CompanyContact = member.CompanyContact;
                        //viewModel.city = member.city;
                        //viewModel.city.Region = member.city.Region;
                        //viewModel.JobApplies = member.JobApplies;
                        viewModels.Add(viewModel);
                    }
                }
                int currentPageIndex = page.HasValue ? page.Value : 1;
                var pagedList = viewModels.ToPagedList(currentPageIndex, PageSize);

                // http://stackoverflow.com/questions/14516174/can-a-razor-view-have-more-then-one-model-without-using-a-viewmodel-object
                var searchViewModel = new SearchViewModel();

                // filtering option
                Dictionary<string, object> filteringOption = new Dictionary<string, object>();

                if (frmCollection["jobSeekerNameOrContent"] != null && !string.IsNullOrEmpty(frmCollection["jobSeekerNameOrContent"]))
                {
                    // searchViewModel.JobSeekerNameOrContent = frmCollection["jobSeekerNameOrContent"];
                    filteringOption.Add("jobSeekerNameOrContent", frmCollection["jobSeekerNameOrContent"]);
                }
                else
                {
                    int regionId = !string.IsNullOrEmpty(frmCollection["Region"]) ? int.Parse(frmCollection["Region"]) : 0;
                    var region = regionService.Get(x => x.RegionId == regionId).FirstOrDefault<Region>();
                    var regionName = region != null ? region.Region1 : string.Empty;

                    int cityId = !string.IsNullOrEmpty(frmCollection["CityId"]) ? int.Parse(frmCollection["CityId"]) : 0;
                    var city = cityService.Get(x => x.CityId == cityId).FirstOrDefault<city>();
                    var cityName = city != null ? city.City1 : string.Empty;

                    var typeOfServicename = string.Empty;
                    int serviceTypeId = !string.IsNullOrEmpty(frmCollection["SeekerServiceType"]) ? int.Parse(frmCollection["SeekerServiceType"]) : 0;
                    if (serviceTypeId > 0)
                    {
                        var typeService = typeOfServiceService.GetSingle(x => x.Id == serviceTypeId);
                        typeOfServicename = typeService.Name;
                    }

                    bool isWeekday = false;
                    bool isShift = false;
                    if (frmCollection["chkAvailabilities"] != null)
                    {
                        List<string> chkAvailabilities = frmCollection["chkAvailabilities"].Replace("true,false", "true").Split(',').ToList();
                        isWeekday = bool.Parse(chkAvailabilities[0]);
                        isShift = bool.Parse(chkAvailabilities[1]);
                    }

                    bool isFulltimePermanent = false;
                    bool isFullTimeContract = false;
                    bool isPartTimeContract = false;
                    
                    if (frmCollection["chkWorkType"] != null)
                    {
                        List<string> chkWorkType = frmCollection["chkWorkType"].Replace("true,false", "true").Split(',').ToList();
                        isFulltimePermanent = bool.Parse(chkWorkType[0]);
                        isFullTimeContract = bool.Parse(chkWorkType[1]);
                        isPartTimeContract = bool.Parse(chkWorkType[2]);
                    }

                    // available languages 
                    bool isEnglish = false;
                    bool isFrench = false;
                    if (frmCollection["chkLanguages"] != null)
                    {
                        List<string> chkLanguages = frmCollection["chkLanguages"].Replace("true,false", "true").Split(',').ToList();
                        isEnglish = bool.Parse(chkLanguages[0]);
                        isFrench = bool.Parse(chkLanguages[1]);
                    }

                    filteringOption.Add("Region", regionName);
                    filteringOption.Add("City", cityName);
                    filteringOption.Add("JobSeekerName", frmCollection["JobSeekerName"]);
                    filteringOption.Add("ExpYears", frmCollection["ExpYears"]);
                    filteringOption.Add("ScoringRange", frmCollection["ScoringRange"]);
                    filteringOption.Add("ContactedBy", frmCollection["ContactedBy"]);
                    filteringOption.Add("ServiceType", typeOfServicename);
                    //filteringOption.Add("MinRate", frmCollection["MinRate"]);

                    filteringOption.Add("isWeekday", isWeekday? "yes": "n/a");
                    filteringOption.Add("isShift", isShift ? "yes" : "n/a");

                    filteringOption.Add("isFulltimePermanent", isFulltimePermanent ? "yes" : "n/a");
                    filteringOption.Add("isFullTimeContract", isFullTimeContract ? "yes" : "n/a");
                    filteringOption.Add("isPartTimeContract", isPartTimeContract ? "yes" : "n/a");

                    filteringOption.Add("isEnglish", isEnglish ? "yes" : "n/a");
                    filteringOption.Add("isFrench", isFrench ? "yes" : "n/a");

                    filteringOption.Add("PostalCode", frmCollection["PostalCode"]);
                    if (!string.IsNullOrEmpty(frmCollection["PostalCode"]))
                    {
                        filteringOption.Add("Distance", frmCollection["distance"] + " km");
                    }
                    else
                    {
                        filteringOption.Add("Distance", string.Empty);
                    }
                }

                TempData["searchJobSeekerCondition"] = filteringOption;
                // result
                searchViewModel.JobSeekerResults = pagedList;
                searchViewModel.TotalFilteredJobSeekerCount = list.Count();


                if (!ShowGrid) // main page (list style): return paged list
                    return PartialView("~/Views/Shared/_JobSeekerSearchResult.cshtml", searchViewModel);
                else // grid style: return whole list (not paged list)
                    return PartialView("~/Views/Shared/_JobSeekerGrid.cshtml", viewModels);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return PartialView();
            }
        }

        public PartialViewResult ShowProvinceCity(string pageName, bool isRequired = false, SearchControlDirection searchControlDirection = SearchControlDirection.Vertical)
        {
            var listRegion = regionService.Get(x => x.CountryId == CountryId).OrderBy(y => y.Region1).ToList<Region>();
            listRegion.Insert(0, new Region { RegionId = 0, Region1 = "Select Province" });

            List<city> listCity = new List<city>();
            listCity.Insert(0, new city { CityId = 0, City1 = "Select City" });

            ViewData["regions"] = listRegion.Select(x => new SelectListItem { Text = x.Region1, Value = x.RegionId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;
            ViewData["cities"] = listCity.Select(x => new SelectListItem { Text = x.City1, Value = x.CityId.ToString() }).AsEnumerable<SelectListItem>();// as IEnumerable<SelectListItem>;                    
            ViewData["isRequired"] = isRequired;
            ViewData["pageName"] = pageName; // JobSearch or JobSeekerSearch
            ViewData["searchControlDirection"] = searchControlDirection;
            return PartialView("~/Views/Shared/_ProvinceCity.cshtml");
        }

        public ActionResult GetCities(int provinceId)
        {
            try
            {
                var cityList = cityService.Get(x => x.RegionID == provinceId).OrderBy(y => y.City1).ToList<city>();
                
                var result = from r in cityList
                             select new { Name = r.City1, Id = r.CityId };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public PartialViewResult JobSearchWithPage(int? page = null, int? RegionId = null, int? CityId = null, int? ServiceTypeId = null, string Title = null, string Description = null, string JobNameOrContent = null)
        {
            try
            {
                JobPostingViewModel viewModel = new JobPostingViewModel();
                var viewModels = new List<JobPostingViewModel>();

                var list = jobPostingService.Get(x => x.IsDeleted == false || x.IsDeleted == null).OrderByDescending(x => x.CreateDate) as IEnumerable<JobPosting>;


                if (list != null && list.Count() > 0)
                {
                    Dictionary<string, string> searchWithPageCollection = new Dictionary<string, string>();
                    searchWithPageCollection.Add("JobNameOrContent", JobNameOrContent);
                    searchWithPageCollection.Add("RegionId", RegionId.HasValue ? RegionId.ToString() : null);
                    searchWithPageCollection.Add("CityId", CityId.HasValue ? CityId.ToString() : null);
                    searchWithPageCollection.Add("ServiceTypeId", ServiceTypeId.HasValue ? ServiceTypeId.ToString() : null);
                    searchWithPageCollection.Add("Title", Title);
                    searchWithPageCollection.Add("Description", Description);

                    ApplyFilter<JobPosting>(ref list, null, searchWithPageCollection);

                    foreach (var member in list)
                    {             
                        viewModel.AspNetUsersId = LoggedInUser.Id;

                        if (member.city != null)
                            viewModel.city = member.city;

                        if (member.city != null && member.city.Region != null)
                            viewModel.city.Region = member.city.Region;

                        // viewModel.JobApplies = member.JobApplies;
                        JobApplyViewModel applyViewModel = new JobApplyViewModel();
                        foreach (var apply in member.JobApplies)
                        {
                            applyViewModel = Mapper.Map<JobApply, JobApplyViewModel>(apply);
                            viewModel.JobApplies.Add(applyViewModel);
                        }

                        viewModels.Add(viewModel);
                    }
                }

                int currentPageIndex = page.HasValue ? page.Value : 1;
                var pagedList = viewModels.ToPagedList(currentPageIndex, PageSize);


                // http://stackoverflow.com/questions/14516174/can-a-razor-view-have-more-then-one-model-without-using-a-viewmodel-object
                var searchViewModel = new SearchViewModel();


                // filtering option
                if (!string.IsNullOrEmpty(JobNameOrContent))
                {
                    searchViewModel.JobNameOrContent = JobNameOrContent;
                }
                else
                {
                    searchViewModel.RegionId = RegionId.HasValue ? (int)RegionId : 0;
                    searchViewModel.CityId = CityId.HasValue ? (int)CityId : 0;
                    searchViewModel.ServiceTypeId = ServiceTypeId.HasValue ? (int)ServiceTypeId : 0;
                    searchViewModel.Title = Title;
                    searchViewModel.Description = Description;
                }

                searchViewModel.JobPostResults = pagedList;

                return PartialView("~/Views/Shared/_JobSearchResult.cshtml", searchViewModel);

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return PartialView();
            }
        }

        public PartialViewResult JobSeekerSearchWithPage(int? page = null
                                                        , int? RegionId = null
                                                        , int? CityId = null
                                                        , int? ServiceTypeId = null
                                                        , int? ExpYears = null
                                                        , string JobSeekerName = null
                                                        , string JobSeekerNameOrContent = null
                                                        , bool isWeekday = false
                                                        , bool isShift = false
                                                        , bool isFulltimePermanent = false
                                                        , bool isFullTimeContract = false
                                                        , bool isPartTimeContract = false
                                                        , bool isEnglish = false
                                                        , bool isFrench = false )
        {
            try
            {
                JobSeekerViewModel viewModel = new JobSeekerViewModel();
                var viewModels = new List<JobSeekerViewModel>();

                var list = jobSeekerService.GetAll().OrderByDescending(x => x.UpdateDate) as IEnumerable<JobSeeker>;

                if (list != null && list.Count() > 0)
                {
                    Dictionary<string, string> searchWithPageCollection = new Dictionary<string, string>();
                    searchWithPageCollection.Add("JobSeekerNameOrContent", JobSeekerNameOrContent);
                    searchWithPageCollection.Add("RegionId", RegionId.HasValue ? RegionId.ToString() : null);
                    searchWithPageCollection.Add("CityId", CityId.HasValue ? CityId.ToString() : null);
                    searchWithPageCollection.Add("ServiceTypeId", ServiceTypeId.HasValue ? ServiceTypeId.ToString() : null);
                    searchWithPageCollection.Add("JobSeekerName", JobSeekerName);
                    searchWithPageCollection.Add("ExpYears", ExpYears.HasValue ? ExpYears.ToString() : null);
                    searchWithPageCollection.Add("isWeekday", isWeekday.ToString());
                    searchWithPageCollection.Add("isShift", isShift.ToString());
                    searchWithPageCollection.Add("isFulltimePermanent", isFulltimePermanent.ToString());
                    searchWithPageCollection.Add("isFullTimeContract", isFullTimeContract.ToString());
                    searchWithPageCollection.Add("isPartTimeContract", isPartTimeContract.ToString());
                    searchWithPageCollection.Add("isEnglish", isEnglish.ToString());
                    searchWithPageCollection.Add("isFrench", isFrench.ToString());                    

                    ApplyFilter<JobSeeker>(ref list, null, searchWithPageCollection);

                    foreach (var member in list)
                    {
                        viewModel = Mapper.Map<JobSeeker, JobSeekerViewModel>(member);
                        //viewModel.CompanyContact = member.CompanyContact;
                        //viewModel.city = member.city;
                        //viewModel.city.Region = member.city.Region;
                        //viewModel.JobApplies = member.JobApplies;
                        viewModels.Add(viewModel);
                    }
                }

                int currentPageIndex = page.HasValue ? page.Value : 1;
                var pagedList = viewModels.ToPagedList(currentPageIndex, PageSize);

                // http://stackoverflow.com/questions/14516174/can-a-razor-view-have-more-then-one-model-without-using-a-viewmodel-object
                var searchViewModel = new SearchViewModel();

                // filtering option
                if (!string.IsNullOrEmpty(JobSeekerNameOrContent))
                {
                    searchViewModel.JobSeekerNameOrContent = JobSeekerNameOrContent;
                }
                else
                {
                    searchViewModel.RegionId = RegionId.HasValue ? (int)RegionId : 0;
                    searchViewModel.CityId = CityId.HasValue ? (int)CityId : 0;
                    searchViewModel.ServiceTypeId = ServiceTypeId.HasValue ? (int)ServiceTypeId : 0;
                    searchViewModel.JobSeekerName = JobSeekerName;
                    searchViewModel.ExpYears = ExpYears.HasValue ? (int)ExpYears : 0; ;
                    searchViewModel.isWeekday = isWeekday;
                    searchViewModel.isShift = isShift;
                    searchViewModel.isFulltimePermanent = isFulltimePermanent;
                    searchViewModel.isFullTimeContract = isFullTimeContract;
                    searchViewModel.isPartTimeContract = isPartTimeContract;
                    searchViewModel.isEnglish = isEnglish;
                    searchViewModel.isFrench = isFrench;
                }

                searchViewModel.JobSeekerResults = pagedList;

                return PartialView("~/Views/Shared/_JobSeekerSearchResult.cshtml", searchViewModel);

            }
            catch (Exception ex)
            {
                log.Error(this, ex);
                ModelState.AddModelError("", "Error: " + ex.Message);
                return PartialView();
            }
        }

    }
}