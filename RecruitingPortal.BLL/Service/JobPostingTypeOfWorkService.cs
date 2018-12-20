using RecruitingPortal.DAL;
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
    public class JobPostingTypeOfWorkService : BaseService<JobPostingTypeOfWork>
    {
        public override IList<JobPostingTypeOfWork> Get(Func<JobPostingTypeOfWork, bool> where, params Expression<Func<JobPostingTypeOfWork, object>>[] navigationProperties)
        {
            return base.Get(where, (x => x.TypeOfWork));
        }
    }
}
