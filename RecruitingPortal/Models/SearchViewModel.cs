using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecruitingPortal.Models
{
    public class SearchViewModel
    {               
        // search filter for both seeker and job
        public int RegionId { get; set; }

        public int CityId { get; set; }

        public int ServiceTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string JobNameOrContent { get; set; }

        public string distance { get; set; }

        public string JobSeekerNameOrContent { get; set; }

        public string JobSeekerName { get; set; }

        public int ExpYears { get; set; }

        public int JobScore { get; set; }

        public bool isWeekday { get; set; }

        public bool isShift { get; set; }

        public bool isFulltimePermanent { get; set; }

        public bool isFullTimeContract { get; set; }

        public bool isPartTimeContract { get; set; }

        public bool isEnglish { get; set; }

        public bool isFrench { get; set; }

        [Display(Name = "Post Code")]
        [StringLength(7)]
        [RegularExpression("^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$", ErrorMessage = "Please enter a proper postal code")]
        public string PostalCode { get; set; }

        [Range(1.00, 999.99, ErrorMessage = "Please enter between 1.00 and 999.99")]
        public decimal? MinRate { get; set; }

        [Range(1.00, 999.99, ErrorMessage = "Please enter between 1.00 and 999.99")]
        public decimal? MaxRate { get; set; }

        [Display(Name = "Service Type")]
        public List<SelectListItem> TypeOfServices { get; set; }

        [Display(Name = "Work Type")]
        public List<CheckBoxes> CheckBoxListTypeOfWork { get; set; }

        [Display(Name = "Years of Experience")]
        [Range(1, 200, ErrorMessage = "Please enter between 1 and 200")]
        public List<SelectListItem> YearsOfExperience { get; set; }

        public List<SelectListItem> JobScoringRange { get; set; }

        public List<SelectListItem> ContactedBy { get; set; }

        public List<CheckBoxes> CheckBoxListLanguages { get; set; }

        public List<SelectListItem> ScoringRange { get; set; }

        public int TotalJobPostCount { get; set; }
        public int TotalJobSeekerCount { get; set; }

        public int TotalFilteredJobPostCount { get; set; }
        public int TotalFilteredJobSeekerCount { get; set; }

        public IPagedList<JobPostingViewModel> JobPostResults { get; set; }
        public IPagedList<JobSeekerViewModel> JobSeekerResults { get; set; }


    }
}