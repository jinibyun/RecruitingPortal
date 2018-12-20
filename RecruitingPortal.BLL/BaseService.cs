using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Implementation;
using System.Linq.Expressions;

namespace RecruitingPortal.BLL
{
    public class BaseService<T> : PortalService<T> where T : class
    {
        private IGenericDataRepository<T> repo { get; set; }
        public BaseService()
        {
            repo = new GenericDataRepository<T>();
        }        

        public override IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            return repo.GetAll(navigationProperties);
        }

        public override T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            return repo.GetSingle(where, navigationProperties);
        }

        public override IList<T> Get(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            return repo.Get(where, navigationProperties);
        }

        public override T Add(T entity)
        {
            return repo.Add(entity);
        }

        public override void Change(T entity)
        {
            repo.Change(entity);
        }

        public override bool Delete(T entity)
        {
            return repo.Delete(entity);
        }
    }
}
