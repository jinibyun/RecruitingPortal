using RecruitingPortal.BLL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    // ref: http://stackoverflow.com/questions/4452144/should-data-annotations-be-on-the-model-or-the-view-model
    // ref: http://tecexplorer.blogspot.ca/2013/01/using-automapper-with-aspnet-mvc.html
    public class JobPostingViewModel
    {
        public int Id { get; set; }

        public string JobId { get; set; }

        public string AspNetUsersId { get; set; }
        public string DeleteAspNetUsersId { get; set; }
        public AspNetUser AspNetUser { get; set; }

        public int? GuardRequestId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int? CityId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        // [Column(TypeName = "Text")]       
        [Required(ErrorMessage = "Please enter a Job Description")]
        [DataType(DataType.MultilineText)] // ref: http://stackoverflow.com/questions/10294699/jquery-validate-unobtrusive-is-not-working-for-textarea
        [AllowHtml] // ref: http://www.codeproject.com/Articles/830925/Integrating-TinyMCE-into-an-MVC-Project
        public string Description { get; set; }

        // ref: http://stackoverflow.com/questions/19811180/best-data-annotation-for-a-decimal18-2

        [Display(Name = "Rate(/hr)")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Please enter only numerical characters with or without decimal")]
        [Range(1.00, 999.99, ErrorMessage = "Please enter between 1.00 and 999.99")]
        public decimal? Rate { get; set; }

        public int? EducationLevelId { get; set; }

        public int? TypeOfServiceId { get; set; }

        public int? TypeOfWorkId { get; set; }

        public int? TypeOfPositionId { get; set; }
        public int? BranchAddressId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsExpired { get; set; }

        public DateTime? ExpiryDue { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public bool? IsHired { get; set; }
       
        public DateTime? DeleteDate { get; set; }

        public string DeleteReason { get; set; }

        public bool? IsDeleted { get; set; }

        [Display(Name = "Update Date")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        public city city { get; set; }

        //[Required(ErrorMessage = "Please enter an Address")]
        [StringLength(100)]
        public string Address { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(7)]
        // [Required(ErrorMessage = "Please enter a postal code")]
        // ref: http://stackoverflow.com/questions/20123758/regular-expression-for-canadian-postal-code-to-also-allow-for-lowercase-letters
        // note: (?i) is not recognized in javascript
        [RegularExpression("^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$", ErrorMessage = "Please enter a valid Postal Code")]
        public string PostalCode { get; set; }

        [ScriptIgnore]
        public ICollection<JobPostingFile> JobPostingFiles { get; set; }

        [ScriptIgnore]
        public ICollection<JobPostingTypeOfWork> JobPostingTypeOfWorks { get; set; }

        public AspNetUsersViewModel aspNetusers { get; set; }

        public GuardRequest guardRequest { get; set; }

        [ScriptIgnore]
        public ICollection<JobApplyViewModel> JobApplies { get; set; }

        [Display(Name = "Education Level")]

        public EducationLevel EducationLevel { get; set; }

        [Display(Name = "Service Type")]

        public TypeOfService TypeOfService { get; set; }

        public BranchAddressViewModel BranchAddress { get; set; }
        public TypeOfPositionViewModel TypeOfPosition { get; set; }

        // ---------------- Added        
        [Display(Name = "Province")]
        public IEnumerable<SelectListItem> RegionsInDropDown { get; set; }

        [Display(Name = "City")]
        public IEnumerable<SelectListItem> CitiesInDropDown { get; set; }

        [Display(Name = "Work Type")]
        public List<CheckBoxes> CheckBoxListTypeOfWork { get; set; }

        [Display(Name = "Service Type")]
        public List<SelectListItem> TypeOfServices { get; set; }

        public PagingInfo PagingInfo { get; set; }

        [Display(Name = "Position")]
        public List<SelectListItem> TypeOfPositions { get; set; }

        [Display(Name = "Education level")]        
        public List<SelectListItem> EducationLevels { get; set; }

        [Display(Name = "Central Office")]
        public List<SelectListItem> BranchOffices { get; set; }

        public bool jobApplied { get; set; }
        public int JobApplyCount { get; set; }

        public string cityName { get; set; }
        public string typeOfServiceName { get; set; }        
        public EnumJobStatus JobStatus { get; set; }
    }
}