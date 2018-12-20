using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecruitingPortal.Win.Service
{
    public partial class MailSending : BaseClass
    {
        public MailSending()
        {
            InitializeComponent();
            this.ServiceName = "RecruitingPortal Notification";
            this.EventLog.Source = "RecruitingPortal Email Service";
            this.EventLog.Log = eventLogScope;
            this.CanHandlePowerEvent = true;
            this.CanHandleSessionChangeEvent = true;
            this.CanPauseAndContinue = true;
            this.CanShutdown = true;
            this.CanStop = true;

            //if (!EventLog.SourceExists(serviceName))
            //    EventLog.CreateEventSource(serviceName, "Application");
        }

        protected override void OnStart(string[] args)
        {
            // NOTE: windows service debugging
            // System.Diagnostics.Debugger.Launch();

            this.m_status = ServiceControllerStatus.Running;

            int nSecs = IntervalSeconds;

            // Remove this from onStart() for performance
            //int nRetExecute = Execute();
            //int nRetMonitor = MonitorStatus();
            //int nRet = Execute();
            // create our threadstart object to wrap our delegate method

            m_thread = new Thread(() => this.ServiceMain());
            //thread.Start();
            //ThreadStart ts = new ThreadStart(this.ServiceMain);

            // create the manual reset event and set it to an initial state of unsignaled
            m_shutdownEvent = new ManualResetEvent(false);

            // create the worker thread
            // m_thread = new Thread(ts);

            // go ahead and start the worker thread
            m_thread.Start();

            // call the base class so it has a chance to perform any work it needs to
            base.OnStart(args);

            // this.EventLog.WriteEntry("... OnStart");
        }

        protected override void OnStop()
        {
            // Abort this running thread
            if (this.m_thread != null)
                this.m_thread.Abort();

            base.OnStop();
        }

        public void onDebug()
        {
            OnStart(null);
        }
    }
}
