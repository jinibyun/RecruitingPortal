using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class JobPostingService : BaseService<JobPosting>
    {
        public override IList<JobPosting> Get(Func<JobPosting, bool> where, params Expression<Func<JobPosting, object>>[] navigationProperties)
        {
            return base.Get(where,
                                (x => x.JobApplies),
                                //(x => x.JobApplies.Select(y => y.JobSeeker)),
                                //(x => x.JobApplies.Select(y => y.JobHires)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(z => z.AspNetUser)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(w => w.city).Select(u => u.Region)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(z => z.EducationLevel)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(z => z.TypeOfService)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(z => z.JobSeekerTypeOfWorks)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(z => z.JobSeekerLangs)),
                                //(x => x.JobApplies.Select(y => y.JobSeeker).Select(z => z.JobSeekerFiles)),
                                (x => x.city),
                                (x => x.city.Region),
                                (x => x.AspNetUser),
                                (x => x.EducationLevel),
                                (x => x.JobPostingFiles),
                                (x => x.JobPostingTypeOfWorks),
                                (x => x.JobPostingTypeOfWorks.Select(y => y.TypeOfWork)),
                                (x => x.TypeOfService),
                                (x => x.TypeOfPosition),
                                (x => x.BranchAddress),
                                (x => x.GuardRequest) 
                                );
        }

        public override IList<JobPosting> GetAll(params Expression<Func<JobPosting, object>>[] navigationProperties)
        {
            return base.GetAll(
                                (x => x.JobApplies),
                                (x => x.JobApplies.Select(y => y.JobSeeker)),
                                (x => x.city),
                                (x => x.city.Region),
                                (x => x.AspNetUser),
                                (x => x.EducationLevel),
                                (x => x.JobPostingFiles),
                                (x => x.JobPostingTypeOfWorks),
                                (x => x.JobPostingTypeOfWorks.Select(y => y.TypeOfWork)),
                                (x => x.TypeOfService),
                                (x => x.TypeOfPosition),
                                (x => x.BranchAddress),
                                (x => x.GuardRequest)
                                );
        }

        public override JobPosting Add(JobPosting entity)
        {
            var newJobPosting = base.Add(entity);
            if (newJobPosting.GuardRequestId.HasValue)
            {
                var guardRequest = new BaseService<GuardRequest>();

                var request = guardRequest.GetSingle(x => x.Id == (int)newJobPosting.GuardRequestId);
                request.RespondedByAspNetUsersId = newJobPosting.AspNetUsersId;
                request.IsResponded = true;
                guardRequest.Change(request);
            }
            return newJobPosting;
        }
    }
}
