namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using Serilog;
    using System.Web;

    public class BaseController : Controller
    {
        // GET: Base
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string storeUrl = ConfigurationManager.AppSettings["STORE_URL"];
            string ssoUrl = ConfigurationManager.AppSettings["SSO_URL"];
            string returnSiteUrl = ConfigurationManager.AppSettings["RETURN_SITE_URL"];
            string returnUrl = ssoUrl + returnSiteUrl;

            // Begin of SSO Code
            var log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
            base.OnActionExecuting(filterContext);
            try
            {
                //var cookiesvalOnActionExecuting = filterContext.HttpContext.Request.Cookies["AAOMASTER"].Value;

                /*HttpCookie cookie_log = filterContext.HttpContext.Request.Cookies["AAOMASTER"];
                string cookieName = cookie_log.Name;
                string cookieValue = cookie_log.Value;
                DateTime expires = cookie_log.Expires;
                string expires_string = Convert.ToString(cookie_log.Expires);
                log.Information(" -----------------------------------");
                log.Information(" AAOMASTER -- cookieName: " + cookieName);
                log.Information(" AAOMASTER -- cookieValue: " + cookieValue);
                log.Information(" AAOMASTER -- expires: " + expires);
                log.Information(" AAOMASTER -- expires_string: " + expires_string);
                log.Information(" -----------------------------------");*/

                var cookiesvalOnActionExecuting = "s1HcVEY%2bxpClOWVMPf6S%2bfEDZSnOabSOH0QS7yZoYnip%2byawLtiVl8AL2OogijVEG1a9vJSZdSlJGd5ePQBkKQfRLY3lXur%2fs05A7nlPKiA3NrBNMNoEe3iYnRPBy9aF";
                // var cookiesvalOnActionExecuting = "0hTvGPTBwCma7NIxmYM81887U4b75KLhaywW3RRMkjN6GTQC % 2bX107xbrgINMs04oVeZGGgX2g53QQQd3qigxnT47G37uXZ0wZk13vS6yX5iga3B2kFttpe8DNyHBjw2S";

                if (cookiesvalOnActionExecuting != null && !string.IsNullOrEmpty(cookiesvalOnActionExecuting))
                {
                    log.Information("SSO is being called OnActionExecuting Test: " + cookiesvalOnActionExecuting);
                    string userDataCheck = string.Empty;

                    string sessionData = HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId"] == null ? null : HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId"].Value;
                    log.Information("Session Data: " + sessionData);
                    log.Information("storeUrl : " + storeUrl);
                    log.Information("returnUrl : " + returnUrl);

                    string tmp = string.Empty;
                    if (sessionData == null || System.Web.HttpContext.Current.User.Identity.Name == string.Empty)
                    {
                        string storeRedirect = HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"] == null ? null : HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"].Value;
                        if (storeRedirect == "1")
                        {
                            //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId");
                            //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId_store_redirect");

                            //System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId");
                            //System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId_store_redirect");

                            //Expiring Master and MasterCustomer cookie instead of Response.Cookies.Remove
                            if (HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId"] != null)
                            {
                                HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId");
                                cookie.Expires = DateTime.Now.AddDays(-1); // Expire immediately
                                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                            }

                            if (HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"] != null)
                            {
                                HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect");
                                cookie.Expires = DateTime.Now.AddDays(-1); // Expire immediately
                                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                            }

                            log.Information("storeRedirect == 1");
                            log.Information("STORE URL CALLED");
                            filterContext.Result = new RedirectResult(storeUrl);
                        }
                        else
                        {
                            UserController buser = new UserController();
                            List<UserDataDTO> lstUser = new List<UserDataDTO>();
                            lstUser = buser.GetUserData(cookiesvalOnActionExecuting, this._sAttrLogPath, filterContext);
                            if (lstUser.Count > 0)
                            {
                                if (lstUser[0].Role == "A")
                                {
                                    filterContext.Result = new RedirectToRouteResult(
                                 new RouteValueDictionary
                                     { { "controller", "AdminDashboard" }, { "action", "Index" } });
                                    return;
                                }
                                else
                                {
                                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                                { { "controller", "Dashboard" }, { "action", "Index" } });
                                    return;
                                }
                            }
                            else
                            {
                                //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId");
                                //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId_store_redirect");

                                //System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId");
                                //System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId_store_redirect");

                                //Expiring Master and MasterCustomer cookie instead of Response.Cookies.Remove
                                if (HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId"] != null)
                                {
                                    HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId");
                                    cookie.Expires = DateTime.Now.AddDays(-1); // Expire immediately
                                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                                }

                                if (HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"] != null)
                                {
                                    HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect");
                                    cookie.Expires = DateTime.Now.AddDays(-1); // Expire immediately
                                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                                }

                                log.Information("lstUser[0].Count is Zero");
                                log.Information("STORE URL CALLED");
                                filterContext.Result = new RedirectResult(storeUrl);
                            }
                        }
                    }
                    else
                    {
                        log.Information("SESSION ALREADY SET");
                        //if (Convert.ToInt32(System.Web.HttpContext.Current.Session["BCSC_AAO_MasterCustomerId_store_redirect"]) == 1)
                        if (Convert.ToString(HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"].Value) == "1")
                        {
                            //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId");
                            //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId_store_redirect");

                            //System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId");
                            //System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId_store_redirect");

                            //Expiring Master and MasterCustomer cookie instead of Response.Cookies.Remove
                            if (HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId"] != null)
                            {
                                HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId");
                                cookie.Expires = DateTime.Now.AddDays(-1); // Expire immediately
                                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                            }

                            if (HttpContext.Request.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"] != null)
                            {
                                HttpCookie cookie = new HttpCookie("BCSC_AAO_MasterCustomerId_store_redirect");
                                cookie.Expires = DateTime.Now.AddDays(-1); // Expire immediately
                                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                            }

                            log.Information("SessionData is exist or System.Web.HttpContext.Current.User.Identity.Name is not empty");
                            log.Information("STORE URL CALLED");
                            filterContext.Result = new RedirectResult(storeUrl);
                        }
                    }
                }
                else
                {
                    //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId");
                    System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId");
                    filterContext.Result = new RedirectResult(returnUrl);
                }
            }
            catch (Exception e)
            {
                //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId");
                System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId");
                Console.Write(e.Message);
                filterContext.Result = new RedirectResult(returnUrl);
                log.Information("Error: " + e.Message);
            }

            // End of SSO Code
            string userAgent;
            userAgent = Request.UserAgent;
            bool isMobile = Request.Browser.IsMobileDevice;

            // Check the Mobile compatability
            if (User.IsInRole("A"))
            {
                if (isMobile)
                {
                    TempData["Msg"] = "Admin access is not available from mobile devices.";
                    filterContext.Result = new RedirectToRouteResult(
                         new RouteValueDictionary
                             { { "controller", "Error" }, { "action", "Index" } });
                    return;
                }
            }

            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;
            if (!controllerName.Contains("Assessment") && User.IsInRole("U"))
            {
                // for Update Exam ConsumedTime
                // Get Exam Detail for Timer Update
                QuestionDetails question = new QuestionDetails();
                question.examId = Convert.ToInt32(System.Web.HttpContext.Current.Session["ExamId"]);

                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                question.userId = Convert.ToInt32(user._userId);
                if (question.examId != 0)
                {
                    string examPostDataJson = JsonConvert.SerializeObject(question);
                    string urlGetExam = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExamDetail";
                    string resultGetExamdetail = HttpProxy.HttpPost(urlGetExam, examPostDataJson, "application/json; charset=utf-8", "POST");
                    ExamDTO examDetail = new ExamDTO();
                    examDetail = JsonConvert.DeserializeObject<ExamDTO>(resultGetExamdetail);
                    if (Convert.ToInt32(examDetail.ExamTimeType) != Convert.ToInt32(ExamManagerEnum.ExamTimeType.Time))
                    {
                        UpdateSkipAnswered updateTimer = new UpdateSkipAnswered
                        {
                            examAttemptId = Convert.ToInt32(examDetail.ExamAttemptId),
                            consumedTimeDifference = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(Session["ExamStartTime"])).TotalSeconds),
                        };
                    }

                    System.Web.HttpContext.Current.Session.Remove("RemTime");
                    System.Web.HttpContext.Current.Session.Remove("ExamId");
                    System.Web.HttpContext.Current.Session.Remove("ExamStartTime");
                }
                else if (!controllerName.Contains("Assessment") && User.IsInRole("A"))
                {
                    System.Web.HttpContext.Current.Session.Remove("RemTime");
                    System.Web.HttpContext.Current.Session.Remove("ExamId");
                    System.Web.HttpContext.Current.Session.Remove("ExamStartTime");
                }
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var log = new Serilog.LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                        { { "controller", "Error" }, { "action", "Index" } });
            base.OnException(filterContext);
            var exception = filterContext.Exception;
            TempData["Msg"] = exception.Message.ToString();
            log.Information("Exception Message : " + filterContext.Exception);
            log.Information("Error Generated by System/filterContext : " + filterContext);
            filterContext.ExceptionHandled = true;
        }
    }
}