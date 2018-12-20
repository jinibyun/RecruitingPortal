using RecruitingPortal.DAL.Interface;
using RecruitingPortal.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.DAL
{
    public class DbContextFactory : IDbContextFactory
    {
        /*
        Applying singleton pattern throw exception: DbContext
        
        private readonly DbContext dbContext;
        static readonly DbContextFactory _instance = new DbContextFactory();
        
        public DbContextFactory()
        {
            dbContext = new RecruitingPortalEntities(); 
        }

        public static DbContextFactory Instance
        {
            get { return _instance; }
        }

        public DbContext GetContext()
        {            
            return dbContext;
        } 
        */


        private readonly DbContext dbContext;
        public DbContextFactory()
        {
            dbContext = new RecruitingPortalEntities();       
        }

        // note: The default value of LazyLoadingEnabled() is false.
        public DbContext GetContext(bool lazyLoadingEnabled = false, bool proxyCreationEnabled = true)
        {
            // ref: http://www.itorian.com/2013/06/what-is-eager-loading-and-what-is-lazy.html
            // ref: https://ovaismehboob.wordpress.com/2013/03/29/enabling-lazy-loading-in-entity-framework-code-first-model/
            // ref: http://stackoverflow.com/questions/2967214/disable-lazy-loading-by-default-in-entity-framework-4
            dbContext.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
            dbContext.Configuration.ProxyCreationEnabled = proxyCreationEnabled;                        

            return dbContext;
        }
    }
}
