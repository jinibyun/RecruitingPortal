using RecruitingPortal.DAL;
using RecruitingPortal.DAL.Implementation;
using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Model;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.BLL.Service
{
    public class JobSeekerService : BaseService<JobSeeker>
    {
        public override IList<JobSeeker> Get(Func<JobSeeker, bool> where, params Expression<Func<JobSeeker, object>>[] navigationProperties)
        {
            try
            {
                return base.Get(where, 
                                (x => x.JobSeekerLangs.Select(y => y.Lang)),
                                (x => x.JobSeekerFiles),
                                (x => x.JobSeekerTypeOfWorks.Select(y => y.TypeOfWork)),
                                (x => x.JobSeekerSecurityExperiences),
                                (x => x.JobSeekerScores),
                                (x => x.TypeOfService),
                                (x => x.TypeOfJobNotice),
                                (x => x.JobSeekerTypeOfWorkAvailabilities.Select(y => y.TypeOfWorkAvailability)),
                                (x => x.EducationLevel),
                                (x => x.JobSeekerStatu),
                                (x => x.AspNetUser),
                                (x => x.AspNetUser.AspNetRoles),
                                (x => x.city),
                                (x => x.city.Region),
                                (x => x.TypeOfService),
                                (x => x.References),
                                (x => x.JobApplies),
                                (x => x.JobSeekerContactLogs),
                                (x => x.JobApplies.Select(y => y.JobPosting))
                               );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IList<JobSeeker> GetAll(params Expression<Func<JobSeeker, object>>[] navigationProperties)
        {
            try
            {
                return base.GetAll(
                                (x => x.JobSeekerLangs.Select(y => y.Lang)),
                                (x => x.JobSeekerFiles),
                                (x => x.JobSeekerTypeOfWorks.Select(y => y.TypeOfWork)),
                                (x => x.JobSeekerSecurityExperiences),
                                (x => x.JobSeekerScores),
                                (x => x.TypeOfService),
                                (x => x.TypeOfJobNotice),
                                (x => x.JobSeekerTypeOfWorkAvailabilities.Select(y => y.TypeOfWorkAvailability)),
                                (x => x.EducationLevel),
                                (x => x.JobSeekerStatu),
                                (x => x.AspNetUser),
                                (x => x.AspNetUser.AspNetRoles),
                                (x => x.city),
                                (x => x.city.Region),
                                (x => x.TypeOfService),
                                (x => x.References),
                                (x => x.JobApplies),
                                (x => x.JobSeekerContactLogs),
                                (x => x.JobApplies.Select(y => y.JobPosting))
                               );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
