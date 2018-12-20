using RecruitingPortal.BLL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class LoggedInUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public EnumMemberRole Role { get; set; }
        public EnumLoginFrom LoginFrom { get; set; }
    }
}