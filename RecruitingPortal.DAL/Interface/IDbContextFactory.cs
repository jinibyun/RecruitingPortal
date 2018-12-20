using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.DAL.Interface
{
    public interface IDbContextFactory
    {
        DbContext GetContext(bool lazyLoadingEnabled = false, bool proxyCreationEnabled = false);
    }
}
