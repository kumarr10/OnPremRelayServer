using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Http.SelfHost;
using WebActivatorEx;
using EHSApps.WebAPI.JSSE;
using Swashbuckle.Application;
using EHS.AzureServiceBusRelay.Server;
using System.Configuration;

namespace EHSApps.API.WinService
{  
    partial class EHSAppsAPIRelayService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // EHSAppsAPIRelayService
            // 
            this.ServiceName = "EHS Apps API Relay Service";

        }
        #endregion
    }

    partial class EHSAppsAPIRelayService : ServiceBase
    {
        //private IDisposable _server = null;
        public static string ServiceAddress = ConfigurationManager.AppSettings["jsseDevHostURL"];
        static string jsseAzureSBURL = ConfigurationManager.AppSettings["jsseAzureSBRelayURL"];
        static string jsseAzureSASKeyName = ConfigurationManager.AppSettings["jsseAzureSASKeyName"];
        static string jsseAzureSASKeyValue = ConfigurationManager.AppSettings["jsseAzureSASKeyValue"];
        public EHSAppsAPIRelayService()
        {
            InitializeComponent();
        }

        
        protected override void OnStart(string[] args)
        {
            try
            {
                //Owin hosting REST API  - from localhost  - working
                var _server = WebApp.Start<Startup>(url: ServiceAddress);

                //Owin hosting REST API relay - from Azure  - working
               
                var config = new AzureServiceBusOwinServiceConfiguration(jsseAzureSASKeyName, jsseAzureSASKeyValue, jsseAzureSBURL);

                //  config.Routes.MapHttpRoute("default", "{controller}/{id}", new { id = RouteParameter.Optional });
                var server = new AzureServiceBusOwinServer();
                AzureServiceBusOwinServer.Create(config, app =>
                {
                    var config1 = new HttpConfiguration();
                    config1.MapHttpAttributeRoutes();
                    config1.Routes.MapHttpRoute("default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
                    config1.EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "EHSApps.WebAPI");
                        // c.IncludeXmlComments(GetXmlCommentsPath());
                        //c.ResolveConflictingActions(x => x.First());

                    }).EnableSwaggerUi();
                    //app.Use((ctx, next) =>
                    //{
                    //    Trace.TraceInformation(ctx.Request.Uri.ToString());
                    //    return next();
                    //});
                    app.UseWebApi(config1);
                   
                });
            }
            catch(Exception ex)
            {             
                EventLog.WriteEntry("Application", ex.Message + " " + ex.StackTrace, EventLogEntryType.Error, 3333);
            }
        }

        protected override void OnStop()
        {
            //if (_server != null)
            //{
            //    _server.Dispose();
            //}
            base.OnStop();
        }
    }
}