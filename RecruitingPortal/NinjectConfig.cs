using Ninject;
using Ninject.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecruitingPortal
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            //Suport WebAPI Injection
            // GlobalConfiguration.Configuration.DependencyResolver = new WebApiContrib.IoC.Ninject.NinjectResolver(kernel);

            RegisterServices(kernel);
            return kernel;

        }
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<DataMetrexMetaEntities>().To<DataMetrexMetaEntities>().InRequestScope();
            kernel.Bind<ClientModelEntities>().To<ClientModelEntities>().InRequestScope();
            kernel.Bind<DataMetrexScannerEntities>().To<DataMetrexScannerEntities>().InRequestScope();

            // mapping raw DAL, BLL
            kernel.Bind<IMetaModelRepository>().To<MetaModelRepository>().InRequestScope();
            kernel.Bind<IClientModelRepository>().To<ClientModelRepository>().InRequestScope();
            kernel.Bind<IScannerModelRepository>().To<ScannerModelRepository>().InRequestScope();

            kernel.Bind<IDTDeviceDataService>().To<DTDeviceDataService>().InRequestScope();
            kernel.Bind<IReceiptPrinterParser>().To<ReceiptPrinterParser>().InRequestScope();
        }
    }
}