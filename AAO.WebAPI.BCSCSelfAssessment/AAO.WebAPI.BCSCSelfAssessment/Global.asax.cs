using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

//using OpenTelemetry;
//using OpenTelemetry.Trace;
//using Honeycomb.OpenTelemetry;
//using OpenTelemetry.Instrumentation.AWSLambda;
//using OpenTelemetry.Resources;
//using OpenTelemetry.Trace;

namespace AAO.WebAPI.BCSCSelfAssessment
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //private TracerProvider _tracerProvider;

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           /* _tracerProvider = Sdk.CreateTracerProviderBuilder()
               .AddHoneycomb(new HoneycombOptions
               {
                    ServiceName = "my-app",
                    ApiKey = "DFoPtp8tBmfOXvioJhTcSE",
                    TracesEndpoint = "https://ui.honeycomb.io/girishkumar.naranje-gettingstarted/environments/test/send-data/v1/traces",
               })
               .Build();*/
        }

        /*protected void Application_End()
        {
            _tracerProvider?.Dispose();
        }*/
    }
}