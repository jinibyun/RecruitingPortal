using Ninject.Modules;
using RecruitingPortal.DAL.Implementation;
using RecruitingPortal.DAL.Interface;
using RecruitingPortal.Win.Service.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class DerivedNinjectModule : NinjectModule
    {
        public override void Load()
        {    
            Bind<ISendMailService>().To<SendMailService>();
            Bind<IDataRepository>().To<DataRepository>();
        }
    }
}
