using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class CityService: BaseService<city>
    {
        public override IList<city> Get(Func<city, bool> where, params Expression<Func<city, object>>[] navigationProperties)
        {
            return base.Get(where, (x => x.Region));
        }        
    }
}
