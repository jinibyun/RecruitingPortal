using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class MemberStaffTeamViewModel
    {
        public int Id { get; set; }
        public int StaffTeamId { get; set; }
        public int MemberId { get; set; }
        
        public AspNetUsersViewModel AspNetUsers { get; set; }
        public StaffTeamViewModel StaffTeam { get; set; }
    }
}