using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please enter a Company Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an Address")]
        public string Address { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(7)]
        [Required(ErrorMessage = "Please enter a Postal Code")]
        // ref: http://stackoverflow.com/questions/20123758/regular-expression-for-canadian-postal-code-to-also-allow-for-lowercase-letters
        // note: (?i) is not recognized in javascript
        [RegularExpression("^[ABCEGHJ-NPRSTVXY]{1}[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[ ]?[0-9]{1}[ABCEGHJ-NPRSTV-Z]{1}[0-9]{1}$", ErrorMessage = "Please enter a proper Postal Code.")]
        public string PostalCode { get; set; }

        [Display(Name = "Web Site")]
        [Required(ErrorMessage = "Please enter a web site")]
        public string WebSite { get; set; }

        [Required(ErrorMessage = "Please enter a company introduction")]
        [DataType(DataType.MultilineText)] // ref: http://stackoverflow.com/questions/10294699/jquery-validate-unobtrusive-is-not-working-for-textarea
        public string Introduction { get; set; }

        [Display(Name = "Number Of Employees")]
        [Required(ErrorMessage = "Please enter a number of employees")]
        [Range(1, 99999, ErrorMessage = "Please enter a number between 1 and 99999")]
        public int NumberOfEmployees { get; set; }

        [Display(Name = "Years of Experience")]
        [Required(ErrorMessage = "Please enter a year of experience")]
        [Range(1, 200, ErrorMessage = "Please enter a number between 1 and 200")]
        public byte YearsOfExperience { get; set; }

        [Display(Name = "Liability Insurance")]
        [Required(ErrorMessage = "Please select if you have a Liability Insurance")]
        public bool LiabilityInsurance { get; set; }

        [Display(Name = "Liability Insurance Amount")]
        [Range(1.00, 9999999.99, ErrorMessage = "Please enter a number between 1.00 and 9999999.99")]
        public Nullable<decimal> LiabilityInsuranceAmount { get; set; }

        [Display(Name = "Agree Terms And Conditions")]
        public string AgreeTermsAndCondition { get; set; }

        [Display(Name = "Legal Rights")]        
        public string LegalRight { get; set; }

        public byte[] ElectroniceSignature { get; set; }

        [ScriptIgnore]
        public ICollection<CompanyFile> CompanyFiles { get; set; }

        public ICollection<BranchAddress> BranchAddresses { get; set; }

        [ScriptIgnore]
        public ICollection<CompanyTypeOfWork> CompanyTypeOfWorks { get; set; }

        [ScriptIgnore]
        public ICollection<CompanyLang> CompanyLangs { get; set; }

        // Added
        public IEnumerable<SelectListItem> RegionsInDropDown { get; set; }
        public IEnumerable<SelectListItem> CitiesInDropDown { get; set; }

        [Display(Name = "Types of Work")]   
        public List<CheckBoxes> CheckBoxListTypeOfWork { get; set; }

        [Display(Name = "Available Languages")]   
        public List<CheckBoxes> CheckBoxListLanguages { get; set; }
    }
}