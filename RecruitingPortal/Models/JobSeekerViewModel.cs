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
    public class JobSeekerViewModel
    {
        public int Id { get; set; }
        public string AspNetUsersId { get; set; }
        public int? DefaultCityId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an Address")]
        public string Address { get; set; }


        [Display(Name = "Postal Code")]
        [StringLength(7)]
        [Required(ErrorMessage = "Please enter a Postal Code")]
        // ref: http://stackoverflow.com/questions/20123758/regular-expression-for-canadian-postal-code-to-also-allow-for-lowercase-letters
        // note: (?i) is not recognized in javascript
        [RegularExpression("^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$", ErrorMessage = "Please enter a valid Postal Code")]
        public string PostalCode { get; set; }


        [Display(Name = "Cell Phone")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter a Cell Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone Number")]
        public string CellPhone { get; set; }

        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone Number")]
        public string HomePhone { get; set; }

        [Display(Name = "Office Phone")]
        public string OfficePhone { get; set; }

        [Display(Name = "Please specify education")]
        public string OtherEduction { get; set; }

        [Display(Name = "Education level")]        
        public int EducationLevelId { get; set; }

        [Display(Name = "Willing to be trained")]        
        public bool? IsWillingToBeTrained { get; set; }

        [Display(Name = "Have Vehicle?")]
        [Required(ErrorMessage = "Please select if you have a car or not")]
        public bool HasCar { get; set; }

        [Display(Name = "Year of Security Experience")]
        [Required(ErrorMessage = "Please select how many years of experience you have")]
        public byte YearsOfExperience { get; set; }

        [Display(Name = "Can provide references")]
        [Required(ErrorMessage = "Please select if you can provide the employer with references")]
        public bool WillProvidereference { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Please select a status")]
        public int JobSeekerStatusId { get; set; }

        [Display(Name = "Service Type")]        
        public int TypeOfServiceId { get; set; }

        [Display(Name = "Rate Desired Per Hour")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Please enter a positive number")]
        public Nullable<decimal> RateDesiredPerHour { get; set; }

        [Display(Name = "Benefits Desired")]
        public Nullable<bool> BenefitDesired { get; set; }

        [Display(Name = "Is Shift Available?")] 
        public Nullable<bool> IsShiftAvailable { get; set; }

        [Display(Name = "Valid CPR/First Aid Certificate")] 
        public Nullable<bool> HasValidFirstAidCertificate { get; set; }

        [Display(Name = "Law & Security Community College Certificate")] 
        public Nullable<bool> HasLawSecurityCommunityCollegeCertificate { get; set; }

        [Display(Name = "Currently Working?")] 
        public Nullable<bool> IsCurrentlyWorking { get; set; }        

        [Display(Name = "Education level")]        
        public EducationLevel EducationLevel { get; set; }

        [ScriptIgnore]
        public ICollection<JobApply> JobApplies { get; set; }

        [Display(Name = "Status")]        
        public JobSeekerStatu JobSeekerStatu { get; set; }

        [Display(Name="Notice Of Availability")]
        public int? TypeOfNoticeId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "License Expiry Date")]
        public DateTime? SecurityLicenseExpiry { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Work Permit Expiry Date")]
        public DateTime? WorkPermitExpiry { get; set; }

        [Display(Name = "Create Date")] 
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Update Date")] 
        public DateTime? UpdateDate { get; set; }
         
        [Display(Name = "Security License")]
        [Required(ErrorMessage = "Please select if you have a security license")]
        public bool HasSecurityLicense { get; set; }

        [Display(Name = "Another Language")]
        public string AnotherLanguage { get; set; }
        
        public TypeOfService TypeOfService { get; set; }
                
        public TypeOfJobNotice TypeOfNoticeFrequency { get; set; }

        public city city { get; set; }

        [ScriptIgnore]
        public ICollection<JobSeekerFile> JobSeekerFiles { get; set; }
        [ScriptIgnore]
        public ICollection<Reference> References { get; set; }
        [ScriptIgnore]
        public ICollection<JobSeekerLang> JobSeekerLangs { get; set; }
        [ScriptIgnore]
        public ICollection<JobSeekerTypeOfWork> JobSeekerTypeOfWorks { get; set; }
        [ScriptIgnore]
        public ICollection<JobSeekerTypeOfWorkAvailability> JobSeekerTypeOfWorkAvailabilities { get; set; }

        [ScriptIgnore]
        public ICollection<JobSeekerSecurityExperience> JobSeekerSecurityExperiences { get; set; }
        [ScriptIgnore]
        public ICollection<JobSeekerScore> JobSeekerScores { get; set; }        
        [ScriptIgnore]
        public ICollection<JobSeekerContactLog> JobSeekerContactLogs { get; set; }

        public AspNetUser AspNetUser { get; set; }
        // ------------------ Added ------------------
        public IEnumerable<SelectListItem> RegionsInDropDown { get; set; }
        public IEnumerable<SelectListItem> CitiesInDropDown { get; set; }
        public List<CheckBoxes> CheckBoxListLanguages { get; set; }
        public List<CheckBoxes> CheckBoxListTypeOfWork { get; set; }
        public List<CheckBoxes> CheckBoxListSecurityExperience { get; set; }
        public List<CheckBoxes> CheckBoxListSecuritySubExperience { get; set; }
        
        // Added for jquery  result
        public int LastJobAppliedId { get; set; }
        public int JobSeekerScoreId { get; set; }
        public bool JobSeekerContactLogExist { get; set; }
        public int Score { get; set; }
        public string AppliedDate { get; set; }        
        
        public string CityName { get; set; }
        public string Region { get; set; }

        [Display(Name = "Education Level")]        
        public List<SelectListItem> EducationLevels { get; set; }

        [Display(Name = "Status")]
        public List<SelectListItem> StatusList { get; set; }

        [Display(Name = "Service Type")]        
        public List<SelectListItem> TypeOfServices { get; set; }

        [Display(Name = "Notice Of Availability")]
        public List<SelectListItem> TypeOfNotices { get; set; }

        public AspNetUsersViewModel AspNetUsers { get; set; }    // change password        
    }
}