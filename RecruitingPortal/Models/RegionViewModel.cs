using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class RegionViewModel
    {
        [Required(ErrorMessage = "Province is required.")]
        public string RegionId { get; set; }

        public int CountryId { get; set; }
        public string Region1 { get; set; }
        public string Code { get; set; }
        public string ADM1Code { get; set; }

        [ScriptIgnore]
        public ICollection<city> cities { get; set; }
        
        public Country Country { get; set; }

    }
}