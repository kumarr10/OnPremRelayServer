﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Hosting.Engine;
using Microsoft.Owin.Hosting.ServerFactory;
using Microsoft.Owin.Hosting.Services;
using Owin;

namespace EHS.AzureServiceBusRelay.Server
{
    // Based on Microsoft.Owin.Testing.TestServer
    public class AzureServiceBusOwinServer : IDisposable
    {
        bool disposed = false;
        public static AzureServiceBusOwinServer Create(AzureServiceBusOwinServiceConfiguration config, Action<IAppBuilder> startup)
        {
            var server = new AzureServiceBusOwinServer();
            server.Configure(config, startup);
            return server;
        }

        private void Configure(AzureServiceBusOwinServiceConfiguration config, Action<IAppBuilder> startup)
        {
            if (startup == null)
            {
                throw new ArgumentNullException("startup");
            }

            var options = new StartOptions();
            if (string.IsNullOrWhiteSpace(options.AppStartup))
            {
                // Populate AppStartup for use in host.AppName
                options.AppStartup = startup.Method.ReflectedType.FullName;
            }

            var serverFactory = new AzureServiceBusOwinServerFactory(config);
            var services = ServicesFactory.Create();
            var engine = services.GetService<IHostingEngine>();
            var context = new StartContext(options)
            {
                ServerFactory = new ServerFactoryAdapter(serverFactory),
                Startup = startup
            };
            _started = engine.Start(context);
        }
                 
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            //_started.Dispose();
        }
   
        private IDisposable _started;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {                
                // Free any other managed objects here.                
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
    }
}