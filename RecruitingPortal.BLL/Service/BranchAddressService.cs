using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class BranchAddressService : BaseService<BranchAddress>
    {
        public override IList<BranchAddress> Get(Func<BranchAddress, bool> where, params Expression<Func<BranchAddress, object>>[] navigationProperties)
        {
            return base.Get(where, 
                            (x => x.city),
                            (x => x.city.Region)

            );
        }
    }
}
