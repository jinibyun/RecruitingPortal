using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class JobSeekerContactLogService : BaseService<JobSeekerContactLog>
    {
        public override IList<JobSeekerContactLog> Get(Func<JobSeekerContactLog, bool> where, params Expression<Func<JobSeekerContactLog, object>>[] navigationProperties)
        {
            return base.Get(where, (x => x.AspNetUser),
                                   (x => x.JobSeeker)
                );
        }
    }
}
