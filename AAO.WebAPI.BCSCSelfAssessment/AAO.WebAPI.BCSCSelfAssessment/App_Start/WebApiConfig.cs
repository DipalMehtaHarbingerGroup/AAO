using AAO.WebAPI.BCSCSelfAssessment.ExceptionFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AAO.WebAPI.BCSCSelfAssessment
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{moduleId}",
                defaults: new { id = RouteParameter.Optional }
                );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Filters.Add(new CustomExceptionFilter());
        }
    }
}
