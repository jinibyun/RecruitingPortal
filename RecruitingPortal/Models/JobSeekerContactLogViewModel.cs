using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class JobSeekerContactLogViewModel
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public int? JobApplyId { get; set; }
        public string AspNetUsersId { get; set; }

        [Display(Name = "Log Contents")]
        [Required(ErrorMessage = "Please enter content")]
        [DataType(DataType.MultilineText)] // ref: http://stackoverflow.com/questions/10294699/jquery-validate-unobtrusive-is-not-working-for-textarea
        public string ContactLog { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public AspNetUser AspNetUser { get; set; }
        public JobApplyViewModel JobApply { get; set; }
        public JobSeekerViewModel JobSeeker { get; set; }
        public AspNetUsersViewModel AspNetUsers { get; set; }

        // added
        public string JobId { get; set; }
        public string Logger { get; set; }
        public string JobSeekerFullName { get; set; }
    }
}