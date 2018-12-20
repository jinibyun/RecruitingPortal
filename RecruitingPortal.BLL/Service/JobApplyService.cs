using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class JobApplyService : BaseService<JobApply>
    {
        public override IList<JobApply> Get(Func<JobApply, bool> where, params Expression<Func<JobApply, object>>[] navigationProperties)
        {
            return base.Get(where,
                    (x => x.JobHires),
                    (x => x.JobPosting),
                    (x => x.JobPosting.TypeOfPosition),
                    (x => x.JobPosting.TypeOfService),
                    (x => x.JobPosting.AspNetUser),
                    (x => x.JobSeeker),
                    (x => x.JobSeeker.AspNetUser),
                    (x => x.JobSeeker.EducationLevel),
                    (x => x.JobSeeker.TypeOfService),
                    (x => x.JobSeeker.city),
                    (x => x.JobSeeker.city.Region),
                    (x => x.JobPosting.city),
                    (x => x.JobPosting.city.Region),
                    (x => x.JobPosting.BranchAddress),
                    (x => x.JobPosting.TypeOfService)
                );            
        }        
    }
}
