using RecruitingPortal.BLL;
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
    public class GuardRequestViewModel
    {
        public int Id { get; set; }
        public string RespondedByAspNetUsersId { get; set; }       

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int? CityId { get; set; }

        public int? TypeOfServiceId { get; set; }

        // [Column(TypeName = "Text")]       
        [Required(ErrorMessage = "Please enter a Job Description")]
        [DataType(DataType.MultilineText)] // ref: http://stackoverflow.com/questions/10294699/jquery-validate-unobtrusive-is-not-working-for-textarea
        [AllowHtml] // ref: http://www.codeproject.com/Articles/830925/Integrating-TinyMCE-into-an-MVC-Project
        public string Description { get; set; }

        [StringLength(500)]
        [Display(Name="Service Location")]
        [Required(ErrorMessage = "Please enter service location")]
        public string ServiceLocation { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsResponded { get; set; }
        public string Priority { get; set; }

        public city city { get; set; }        
        public AspNetUser AspNetUser { get; set; }
        public TypeOfServiceViewModel TypeOfService { get; set; }
        public TypeOfWorkViewModel TypeOfWork { get; set; }

        [ScriptIgnore]
        public ICollection<JobPostingViewModel> JobPostings { get; set; }

        [ScriptIgnore]
        public ICollection<GuardRequestTypeOfWorkViewModel> GuardRequestTypeOfWorks { get; set; }

        public int? TypeOfPositionId { get; set; }
        public TypeOfPositionViewModel TypeOfPosition { get; set; }
        
        public int? StaffTeamId { get; set; }
        public StaffTeamViewModel StaffTeam { get; set; }

        public int? BranchAddressId { get; set; }
        public BranchAddressViewModel BranchAddress { get; set; }

        [Display(Name = "Special Remarks [Office Use Only]")]
        [DataType(DataType.MultilineText)] // ref: http://stackoverflow.com/questions/10294699/jquery-validate-unobtrusive-is-not-working-for-textarea
        [AllowHtml] // ref: http://www.codeproject.com/Articles/830925/Integrating-TinyMCE-into-an-MVC-Project
        public string SpecialRemark { get; set; }

        // added
        public string RespondedBy { get; set; }
        public int JobPostId { get; set; }
        public string JobId { get; set; }
        public DateTime? JobPostDate { get; set; }
        public string Requestor { get; set; }

        [Display(Name = "Work Type")]
        public List<CheckBoxes> CheckBoxListTypeOfWork { get; set; }

        [Display(Name = "Position")]
        public List<SelectListItem> TypeOfPositions { get; set; }

        [Display(Name = "Service Type")]
        public List<SelectListItem> TypeOfServices { get; set; }

        [Display(Name = "Central Office")]
        public List<SelectListItem> BranchOffices { get; set; }

        [Display(Name = "Requested By Team")]
        public List<SelectListItem> StaffTeams { get; set; }

        [Display(Name = "Province")]
        public IEnumerable<SelectListItem> RegionsInDropDown { get; set; }

        [Display(Name = "City")]
        public IEnumerable<SelectListItem> CitiesInDropDown { get; set; }

        public EnumJobStatus JobStatus { get; set; }

    }
}