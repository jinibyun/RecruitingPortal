using Ninject;
using RecruitingPortal.Domain;
using RecruitingPortal.Win.Service.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service
{
    public class BaseClass: ServiceBase
    {
        protected const string eventLogScope = "Application";
        protected Thread m_thread;
        protected ManualResetEvent m_shutdownEvent;
        protected TimeSpan m_delay;
        protected ServiceControllerStatus m_status;
        protected int IntervalSeconds = ConfigurationManager.AppSettings["IntervalSeconds"] != null ? int.Parse(ConfigurationManager.AppSettings["IntervalSeconds"]) : 5;
        protected ISendMailService service { get; set; }

        public BaseClass()
        {
            // ref: http://www.martinsteel.co.uk/blog/2014/Ninject-dependency-injection-for-windows-services
            StandardKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            service = kernel.Get<ISendMailService>();
        }

        // Service Main Thread
        protected void ServiceMain()
        {
            bool bSignaled = false;
            int nReturnCode = 0;

            while (true)
            {
                // get interval time for excutable function
                int nSecs = IntervalSeconds;
                m_delay = new TimeSpan(0, 0, nSecs);

                // wait for the event to be signaled or for the configured delay
                bSignaled = m_shutdownEvent.WaitOne(m_delay, true);

                // if we were signaled to shutdow, exit the loop
                if (bSignaled == true)
                    break;

                // actual work
                nReturnCode = SendEmail();
            }
        }

        private int SendEmail()
        {
            // NOTE: all logging process is being done in class library project. NOT this windows project
            service.SendMail();
            return -1;
        }
    }
}
