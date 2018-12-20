using RecruitingPortal.DAL.Implementation;
using RecruitingPortal.DAL.Interface;
using RecruitingPortal.Win.Service.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service
{
    public class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            // ref: http://www.martinsteel.co.uk/blog/2014/Ninject-dependency-injection-for-windows-services
            // mapping raw DAL, BLL
            Bind<ISendMailService>().To<SendMailService>();
            Bind<IDataRepository>().To<DataRepository>();
        }
    }
}
