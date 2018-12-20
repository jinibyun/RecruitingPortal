using RecruitingPortal.DAL;
using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.BLL.Service
{
    public class MemberService : BaseService<Member>
    {
        // ref: http://stackoverflow.com/questions/21612505/entity-framework-child-objects-not-populating
        public override IEnumerable<Member> Where(System.Linq.Expressions.Expression<Func<Member, bool>> predicate)
        {
            using (var dbContext = (new DbContextFactory()).GetContext())
            {
                return dbContext.Set<Member>()
                                .Include(x=>x.MemberRoles)
                                .Include(y=>y.JobSeekers)
                                .Include(z=>z.Company)  
                                .Include(z=>z.city)
                                .Include(z => z.city.Region)
                                .Where(predicate).ToList<Member>();            
            }
        }
    }
}
