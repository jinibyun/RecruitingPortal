using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.BLL.Service
{
    public abstract class PortalService<T> : IGenericDataRepository<T>
    {
        public abstract IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);

        public abstract T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);

        public abstract IList<T> Get(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);

        public abstract T Add(T items);

        public abstract void Change(T items);

        public abstract bool Delete(T items);
    }
}
