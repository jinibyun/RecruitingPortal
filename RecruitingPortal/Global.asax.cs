using RecruitingPortal.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Ninject.Web.Common.WebHost;
using Ninject;
using RecruitingPortal.Domain;
using RecruitingPortal.BLL.Service;
using RecruitingPortal.BLL;

namespace RecruitingPortal
{
    public class MvcApplication : NinjectHttpApplication //System.Web.HttpApplication
    {
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly HttpRequest initialRequest;

        static MvcApplication()
        {
            initialRequest = HttpContext.Current.Request;
        }

        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // initialize automapper between domain and modelView
            AutoMapperConfiguration.Configure();

            // ref: http://stackoverflow.com/questions/14662643/how-to-disable-code-first-feature-in-ef-mvc4-visual-studio-2012
            // Database.SetInitializer<RecruitingPortalEntities>(null);
            

            // logging
            ConfigureLogging();           

            // ref: http://stackoverflow.com/questions/4700172/unrequired-property-keeps-getting-data-val-required-attribute
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            RegisterServices(kernel);
            return kernel;
        }

        private void RegisterServices(IKernel kernel)
        {
            kernel.Bind<PortalService<AspNetUser>>().To<AspNetUserService>();
            kernel.Bind<PortalService<JobPosting>>().To<JobPostingService>();
            kernel.Bind<PortalService<JobPostingFile>>().To<JobPostingFileService>();
            kernel.Bind<PortalService<JobPostingTypeOfWork>>().To<JobPostingTypeOfWorkService>();
            kernel.Bind<PortalService<city>>().To<CityService>();
            kernel.Bind<PortalService<Region>>().To<RegionService>();
            kernel.Bind<PortalService<Company>>().To<CompanyService>();
            kernel.Bind<PortalService<CompanyLang>>().To<CompanyLangService>();
            kernel.Bind<PortalService<CompanyTypeOfWork>>().To<CompanyTypeOfWorkService>();
            kernel.Bind<PortalService<CompanyFile>>().To<CompanyFileService>();            
            kernel.Bind<PortalService<JobSeeker>>().To<JobSeekerService>();
            kernel.Bind<PortalService<JobSeekerFile>>().To<JobSeekerFileService>();
            kernel.Bind<PortalService<JobSeekerLang>>().To<JobSeekerLangService>();
            kernel.Bind<PortalService<JobSeekerTypeOfWork>>().To<JobSeekerTypeOfWorkService>();
            kernel.Bind<PortalService<JobSeekerTypeOfWorkAvailability>>().To<JobSeekerTypeOfWorkAvailabilityService>();
            kernel.Bind<PortalService<JobApply>>().To<JobApplyService>();
            kernel.Bind<PortalService<JobHire>>().To<JobHireService>();
            kernel.Bind<PortalService<NotificationQueue>>().To<NotificationQueueService>();
            kernel.Bind<PortalService<JobAlert>>().To<JobAlertService>();
            kernel.Bind<PortalService<RecruitingPortal.Domain.TypeOfService>>().To<RecruitingPortal.BLL.Service.TypeOfService>();
            kernel.Bind<PortalService<TypeOfPosition>>().To<TypeOfPositionService>();
            kernel.Bind<PortalService<TypeOfWork>>().To<TypeOfWorkService>();
            kernel.Bind<PortalService<TypeOfSecurityExperience>>().To<TypeOfSecurityExperienceService>();
            kernel.Bind<PortalService<JobSeekerSecurityExperience>>().To<JobSeekerSecurityExperienceService>();
            kernel.Bind<PortalService<BranchAddress>>().To<BranchAddressService>();
            kernel.Bind<PortalService<TypeOfJobNotice>>().To<TypeOfJobNoticeService>();
            kernel.Bind<PortalService<JobSeekerScore>>().To<JobSeekerScoreService>();
            kernel.Bind<PortalService<GuardRequest>>().To<GuardRequestService>();
            kernel.Bind<PortalService<JobSeekerContactLog>>().To<JobSeekerContactLogService>();
            kernel.Bind<PortalService<StaffTeam>>().To<StaffTeamService>();
            kernel.Bind<PortalService<GuardRequestTypeOfWork>>().To<GuardRequestTypeOfWorkService>();
            kernel.Bind<PortalService<JobSeekerStatu>>().To<JobSeekerStatusService>();
            kernel.Bind<IBusinessLayer>().To<BusinessLayerService>();
        }

        private void ConfigureLogging()
        {
            // ref: http://stackoverflow.com/questions/7757524/request-is-not-available-in-this-context-in-global-asax-what-is-replace
            // ref: http://sammyageil.com/post/2011/05/08/Request-is-not-available-in-this-context-exception-in-Globalasaxs-Application_Start-IIS-7-Integrated-mode.aspx
            // note: If you are hosting your application in IIS7 integrated pipeline HttpContext objects are not available in Application_Start
            string logFile = initialRequest.PhysicalApplicationPath + "log4net.config";
            if (System.IO.File.Exists(logFile))
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(logFile));
        }
    }
}
