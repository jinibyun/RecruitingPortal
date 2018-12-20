using RecruitingPortal.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.DAL.Implementation
{
    public class GenericDataRepository<T> : IGenericDataRepository<T> where T : class
    {
        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {            
            List<T> list;
            using (var context = (new DbContextFactory()).GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<T>();
            }
            return list;
        }

        public virtual IList<T> Get(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            List<T> list;
            using (var context = (new DbContextFactory()).GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .Where(where)
                    .ToList<T>();
            }
            return list;
        }

        public virtual T GetSingle(Func<T, bool> where, params Expression<Func<T, object>>[] navigationProperties)
        {
            T item = null;
            using (var context = (new DbContextFactory()).GetContext())
            {
                IQueryable<T> dbQuery = context.Set<T>();

                //Apply eager loading
                foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<T, object>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(where); //Apply where clause
            }
            return item;
        }

        public virtual T Add(T entity)
        {
            using (var dbContext = (new DbContextFactory()).GetContext())
            {
                try
                {
                    // var newEntity = dbContext.Set<T>().Add(entity);
                    dbContext.Set<T>().Attach(entity);
                    var newEntity = dbContext.Set<T>().Add(entity);
                    dbContext.SaveChanges();

                    return newEntity;
                }
                catch (DbEntityValidationException e)
                {
                    // ref: http://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert                    
                    var newException = new FormattedDbEntityValidationException(e);
                    throw newException;
                }
            }
        }

        public virtual bool Delete(T entity)
        {
            using (var dbContext = (new DbContextFactory()).GetContext())
            {
                try
                {
                    // ref: http://stackoverflow.com/questions/7791149/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
                    dbContext.Set<T>().Attach(entity);
                    var removeEntity = dbContext.Set<T>().Remove(entity);
                    dbContext.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException e)
                {
                    // ref: http://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert                    
                    var newException = new FormattedDbEntityValidationException(e);
                    throw newException;
                }
            }
        }

        public virtual void Change(T entity)
        {
            using (var dbContext = (new DbContextFactory()).GetContext())
            {
                try
                {
                    // http://stackoverflow.com/questions/14902408/why-does-entity-framework-insert-children-when-i-update-the-parent
                    dbContext.Set<T>().Attach(entity);
                    dbContext.Entry(entity).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    // ref: http://stackoverflow.com/questions/7795300/validation-failed-for-one-or-more-entities-see-entityvalidationerrors-propert                    
                    var newException = new FormattedDbEntityValidationException(e);
                    throw newException;
                }
            }
        }
    }
}
