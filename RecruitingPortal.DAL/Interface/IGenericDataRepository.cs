using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.DAL.Interface
{
    public interface IGenericDataRepository<T> // where T : class
    {              
        // ref: http://stackoverflow.com/questions/13617698/the-operation-cannot-be-completed-because-the-dbcontext-has-been-disposed-error
        // https://blog.magnusmontin.net/2013/05/30/generic-dal-using-entity-framework/        
        IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties);
        T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);
        IList<T> Get(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties);        
        // T Get(int id);
        T Add(T entity);
        void Change(T entity);
        bool Delete(T entity);
    }
}
