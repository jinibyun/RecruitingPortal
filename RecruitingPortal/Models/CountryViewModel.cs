using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RecruitingPortal.Models
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }
        public string Country1 { get; set; }
        public string FIPS104 { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
        public string ISON { get; set; }
        public string Internet { get; set; }
        public string Capital { get; set; }
        public string MapReference { get; set; }
        public string NationalitySingular { get; set; }
        public string NationalityPlural { get; set; }
        public string Currency { get; set; }
        public string CurrencyCode { get; set; }
        public long? Population { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }

        [ScriptIgnore]
        public ICollection<CityViewModel> cities { get; set; }
        [ScriptIgnore]
        public ICollection<RegionViewModel> Regions { get; set; }
    }
}