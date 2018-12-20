using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class JobSeekerScoreViewModel
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int? SecurityGuardExp { get; set; }
        public int? SecurityGuardLicense { get; set; }
        public int? ValidAidCertificate { get; set; }
        public int? SecurityCommunityCollegeCertificate { get; set; }
        public int? PoliceMilitaryExp { get; set; }
        public int? DriverLicense { get; set; }
        public int? HaveVehicle { get; set; }
        public int? FluenInEnglish { get; set; }
        public int? AvailableAllShift { get; set; }
        public int? CurrentlyWorking { get; set; }
        public int? ServiceTypeDesired { get; set; }
        public int? ResumeAttached { get; set; }
        public int? Extra { get; set; }
        public string ExtraDescription { get; set; }
        public int? TotalScore { get; set; }
    }
}