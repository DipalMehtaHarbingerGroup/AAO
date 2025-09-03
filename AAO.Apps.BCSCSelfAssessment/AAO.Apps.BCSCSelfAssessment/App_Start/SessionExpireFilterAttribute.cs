using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AAO.Apps.BCSCSelfAssessment.App_Start
{
    public class SessionExpireFilterAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            // check if session is supported  
            if (ctx.Session != null)
            {
                // check if a new session id was generated  
                if (ctx.Session["checkSessionExpired"] == null || ctx.Session.IsNewSession)
                {
                    //Check is Ajax request  
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                    }
                    // check if a new session id was generated  
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Login/SignOut");
                    }
                }
            }
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}