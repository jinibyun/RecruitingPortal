using RecruitingPortal.DAL;
using RecruitingPortal.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL.Service
{
    public class NotificationQueueService : BaseService<NotificationQueue>
    {
        public override IList<NotificationQueue> Get(Func<NotificationQueue, bool> where, params Expression<Func<NotificationQueue, object>>[] navigationProperties)
        {
            return base.Get(where,
                            (x => x.EmailTo),
                            (y => y.Subject),
                            (z => z.Body)
                );
        }        
    }
}
