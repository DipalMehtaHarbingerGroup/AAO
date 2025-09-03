namespace AAO.Apps.BCSCSelfAssessment
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

/*
        protected void Application_BeginRequest()
        {
            HttpCookie sessionTimeout;
            Uri myReferrer = Request.UrlReferrer;
            string pageRefferrer = Convert.ToString(myReferrer);
            if (!string.IsNullOrEmpty(pageRefferrer) && pageRefferrer.Contains("SignOut"))
            {
                Response.Cookies["SessionUnixTimeoutExpireAt"].Expires = DateTime.Now.AddDays(-1);
            }
            else
            {
                if (Request.Cookies["SessionUnixTimeoutExpireAt"] == null)
                {
                    sessionTimeout = new HttpCookie("SessionUnixTimeoutExpireAt");
                }
                else
                {
                    sessionTimeout = Request.Cookies["SessionUnixTimeoutExpireAt"];
                }

                //DateTime expireAt = DateTime.Now.AddMinutes(5).ToUniversalTime();
                DateTime expireAt = DateTime.Now.Add(FormsAuthentication.Timeout).ToUniversalTime();
                sessionTimeout.Value = expireAt.ToString("MM/dd/yyyy HH:mm:ss");

                //sessionTimeout.Expires = DateTime.Now.AddMinutes(5);
                sessionTimeout.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
                Response.Cookies.Add(sessionTimeout);
            }
        }
*/

        protected void Application_EndRequest()
        {
            if (Context.Response.StatusCode == 404)
            {
                if ((!Request.RawUrl.Contains("style")) && (!Request.RawUrl.Contains("images")))
                {
                    Response.Clear();
                    if (Response.StatusCode == 404)
                    {
                        Response.Redirect("/Error/Index");
                    }
                }
            }
        }
    }
}