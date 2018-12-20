using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal.Models
{
    public class GuardRequestTypeOfWorkViewModel
    {
        public int GuardRequestId { get; set; }
        public int TypeOfWorkId { get; set; }
        public string Description { get; set; }
        public DateTime? CreateDate { get; set; }

        public GuardRequestViewModel GuardRequest { get; set; }
        public TypeOfWorkViewModel TypeOfWork { get; set; }
    }
}