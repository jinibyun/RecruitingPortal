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
    public class JobAlertViewModel
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int JobAlertFrequencyId { get; set; }

        [Display(Name = "Keyword")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please enter a keyword")]
        public string Keyword { get; set; }

        [Display(Name = "Distance")]
        public int? WithinKilometer { get; set; }
        
        public bool? IsActive { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }
        
        public bool? IsDeleted { get; set; }

        [Display(Name = "How often")]
        public JobAlertFrequency JobAlertFrequency { get; set; }

        public JobSeeker JobSeeker { get; set; }

        // Added
        [Display(Name = "How often")]        
        public List<SelectListItem> Frequencies { get; set; }

        [Display(Name = "Distance")]        
        public List<SelectListItem> Distance { get; set; }

        public string frequencyString { get; set; }

    }
}