using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class BranchAddressViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public int ListOrder { get; set; }

        public CityViewModel city { get; set; }
        public CompanyViewModel Company { get; set; }
    }
}