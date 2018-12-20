using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class GuardRequestService : BaseService<GuardRequest>
    {
        public override IList<GuardRequest> Get(Func<GuardRequest, bool> where, params Expression<Func<GuardRequest, object>>[] navigationProperties)
        {
            return base.Get(where,
                (x => x.AspNetUser),
                (x => x.BranchAddress),
                (x => x.BranchAddress.city),
                (x => x.StaffTeam),
                (x => x.TypeOfService),
                (x => x.GuardRequestTypeOfWorks),
                (x => x.GuardRequestTypeOfWorks.Select(y => y.TypeOfWork)),
                (x => x.TypeOfPosition),
                (x => x.JobPostings),
                (x => x.JobPostings.Select(y => y.JobApplies))

                );
        }
    }
}
