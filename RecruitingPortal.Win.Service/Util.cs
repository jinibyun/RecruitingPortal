using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service
{
    public static class Util
    {
        public static string GetServiceNameAppConfig(string serviceName)
        {
            var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);
            return config.AppSettings.Settings[serviceName].Value;
        }
    }
}
