using RecruitingPortal.BLL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.BLL
{
    public static class ServiceFactory
    {
        public static T GetService<T>() where T : class
        {
            var type = typeof(T);
            return Activator.CreateInstance(type) as T;
        } 
        //public T GetService<T>() where T : class
        //{
        //    var type = typeof(T);
        //    return Activator.CreateInstance(type) as T;
        //}
        
    }
}
