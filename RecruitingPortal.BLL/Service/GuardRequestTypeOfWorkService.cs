using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class GuardRequestTypeOfWorkService : BaseService<GuardRequestTypeOfWork>
    {
        public override IList<GuardRequestTypeOfWork> Get(Func<GuardRequestTypeOfWork, bool> where, params Expression<Func<GuardRequestTypeOfWork, object>>[] navigationProperties)
        {
            return base.Get(where, (x => x.TypeOfWork));
        }        
    }
}
