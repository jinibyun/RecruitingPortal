using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class AspNetUserService : BaseService<AspNetUser>
    {
        public override IList<AspNetUser> Get(Func<AspNetUser, bool> where, params Expression<Func<AspNetUser, object>>[] navigationProperties)
        {
            return base.Get(where,
                    (x => x.AspNetRoles)                    
                );
        }
    }
}
