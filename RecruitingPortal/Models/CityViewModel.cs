using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.WebPages.Html;
using RecruitingPortal.Domain;
namespace RecruitingPortal.Models
{
    // ref: http://stackoverflow.com/questions/4452144/should-data-annotations-be-on-the-model-or-the-view-model
    // ref: http://tecexplorer.blogspot.ca/2013/01/using-automapper-with-aspnet-mvc.html
    public class CityViewModel
    {
        [Required(ErrorMessage = "City is required.")]
        public string CityId { get; set; }

        public int CountryID { get; set; }

        public int RegionID { get; set; }

        public string City1 { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string TimeZone { get; set; }

        public int DmaId { get; set; }

        public string Code { get; set; }

        public Country Country { get; set; }

        public Region Region { get; set; }

        [ScriptIgnore]
        public ICollection<JobPosting> JobPostings { get; set; }

        [ScriptIgnore]
        public ICollection<JobSeeker> JobSeekers { get; set; }

        [ScriptIgnore]
        public ICollection<BranchAddress> BranchAddresses { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> ItemsInDropDown { get; set; }

    }
}