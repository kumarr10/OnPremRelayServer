using Microsoft.Owin.Hosting;
using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Web.Http.SelfHost;

namespace EHS.WebAPI.JSSE.WinService
{
    public partial class JSSEWinService : ServiceBase
    {
        //internal ServiceHost serviceHost = null;
        private IDisposable _server = null;
        public const string ServiceAddress = "http://localhost:8081";
        public JSSEWinService()
        {
            InitializeComponent();          
        }

        protected override void OnStart(string[] args)
        {            
            try
            {
                Thread.Sleep(10000);
                _server = WebApp.Start<Startup>(url: ServiceAddress);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message + " " + ex.StackTrace, EventLogEntryType.Error, 111);               
            }            
        }

        protected override void OnStop()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
            base.OnStop();
        }
    }
}
