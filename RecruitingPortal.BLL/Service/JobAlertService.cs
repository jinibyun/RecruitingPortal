using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class JobAlertService : BaseService<JobAlert>
    {
        public override IList<JobAlert> Get(Func<JobAlert, bool> where, params Expression<Func<JobAlert, object>>[] navigationProperties)
        {
            return base.Get(where, 
                            (x => x.JobAlertFrequency),
                            (x => x.JobSeeker)

                            );
        }
    }
}
