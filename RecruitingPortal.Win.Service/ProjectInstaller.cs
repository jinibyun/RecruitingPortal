using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private const string serviceName = "RecruitingPortal Notification";
        private const string serviceDescription = "Recruiting Portal Email Notification Service";
        /// <summary>
        /// Public Constructor for WindowsServiceInstaller.
        /// - Put all of your Initialization code here.
        /// </summary>
        public ProjectInstaller()
        {
            //# Service Account Information
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;
            this.Installers.Add(serviceProcessInstaller);

            //# Service Information
            ServiceInstaller serviceInstaller = new ServiceInstaller();
            serviceInstaller.DisplayName = serviceName;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            // Note: This must be identical to the WindowsService.ServiceBase name set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.Description = serviceDescription;
            this.Installers.Add(serviceInstaller);
        }
    }
}