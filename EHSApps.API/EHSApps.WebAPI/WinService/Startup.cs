using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EHSApps.WebAPI.JSSE;
using Swashbuckle.Application;

namespace EHSApps.API.WinService
{
    public class Startup    
    {
        //  Hack from http://stackoverflow.com/a/17227764/19020 to load controllers in 

        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            //  Enable attribute based routing
            //  http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //WebApiConfig.Register(config);
            config.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "EHSApps.WebAPI");
                // c.IncludeXmlComments(GetXmlCommentsPath());
                //c.ResolveConflictingActions(x => x.First());

            }).EnableSwaggerUi();
            appBuilder.UseWebApi(config);

        }
    }
}