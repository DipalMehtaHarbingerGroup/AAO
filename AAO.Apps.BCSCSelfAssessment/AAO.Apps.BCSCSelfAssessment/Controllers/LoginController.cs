namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Security;

    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            string ssoUrl = ConfigurationManager.AppSettings["SSO_URL"];
            string returnSiteUrl = ConfigurationManager.AppSettings["RETURN_SITE_URL"];
            string returnUrl = ssoUrl + returnSiteUrl;
            return Redirect(returnUrl);

            // if (User.Identity.IsAuthenticated)
            // {
            //    bool userRoleCheck = User.IsInRole("A");
            //    if (userRoleCheck)
            //    {
            //        return RedirectToAction("Index", "AdminDashboard");
            //    }
            //    else
            //    {
            //        return RedirectToAction("Index", "Dashboard");
            //    }
            // }
            // else { return View(); }
        }

        [HttpPost]
        public ActionResult SignIn(UserDTO user)
        {
            try
            {
                UserDTO objUser = new UserDTO();
                objUser.UserEmail = user.UserEmail;
                objUser.Password = user.Password;
                string userdataLoginJson = JsonConvert.SerializeObject(objUser);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "login/ValidateUser";
                string result = HttpProxy.HttpPost(url, userdataLoginJson, "application/json; charset=utf-8", "POST");
                List<UserDTO> objUserResult = JsonConvert.DeserializeObject<List<UserDTO>>(result);
                if (objUserResult.Count > 0)
                {
                    FormsAuthentication.SetAuthCookie(objUserResult[0].UserEmail, false);
                    Session["UserID"] = (objUserResult[0].UserId);
                    Session["UserNameFullName"] = objUserResult[0].FirstName + ' ' + objUserResult[0].LastName;
                    TempData["Msg"] = "You are successfully logged in";
                    if (objUserResult[0].Role == "A")
                    {
                        return RedirectToAction("Index", "AdminDashboard");
                    }
                    else if (objUserResult[0].Role == "U")
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    TempData["Msg"] = "Login Failed  ";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Something Went Wrong  " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session["UserID"] = null;
            Session.Remove("RemTime");
            Session.Remove("ExamId");
            Session.Remove("ExamStartTime");
            //Response.Cookies["SessionUnixTimeoutExpireAt"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER_SUBDOMAIN"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["BCSC_AAO_MasterCustomerId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[".ASPXROLES"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__RequestVerificationToken"].Expires = DateTime.Now.AddDays(-1);
            Session.RemoveAll();
            Response.Cookies.Clear();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SignOutfromlogoutbutton()
        {
            UserDTO user = new UserDTO();
            bool isSuccess = false;
            string ssologout_Url = ConfigurationManager.AppSettings["SSOlogout_URL"];
            try
            {
                FormsAuthentication.SignOut();
                Session["UserID"] = null;
                Session.Remove("RemTime");
                Session.Remove("ExamId");
                Session.Remove("ExamStartTime");
                Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["BCSC_AAO_MasterCustomerId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies[".ASPXROLES"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies["AAOMASTER"].Expires = DateTime.Now.AddDays(-1);
                return Json(new { isSuccess = true, ssologout_Url = ssologout_Url }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error - " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public ActionResult LogOffafterTimeOut()
        {
            string ssologout_Url = ConfigurationManager.AppSettings["SSOlogout_URL1"];
            string returnSiteUrl = ConfigurationManager.AppSettings["RETURN_SITE_URL"];
            string returnUrl1 = ssologout_Url + returnSiteUrl;

            FormsAuthentication.SignOut();
            Session["UserID"] = null;
            Session.Remove("RemTime");
            Session.Remove("ExamId");
            Session.Remove("ExamStartTime");
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER_SUBDOMAIN"].Expires = DateTime.Now.AddDays(-1); //AAOMASTER_SUBDOMAIN
            Response.Cookies["BCSC_AAO_MasterCustomerId"].Expires = DateTime.Now.AddDays(-1); //BCSC_AAO_MasterCustomerId
            Response.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"].Expires = DateTime.Now.AddDays(-1); //BCSC_AAO_MasterCustomerId_store_redirect
            Response.Cookies[".ASPXROLES"].Expires = DateTime.Now.AddDays(-1); //.ASPXROLES
            /*Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__RequestVerificationToken"].Expires = DateTime.Now.AddDays(-1);
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }*/

            return Redirect(returnUrl1);
        }

        public void CookiesSessionClear()
        {
            FormsAuthentication.SignOut();
            Session["UserID"] = null;
            Session.Remove("RemTime");
            Session.Remove("ExamId");
            Session.Remove("ExamStartTime");

            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();

            Response.Cookies[FormsAuthentication.FormsCookieName].Expires = DateTime.Now.AddDays(-1);

            //bcscsap-qa.aao.org
            Response.Cookies[".ASPXROLES"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER_SUBDOMAIN"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["BCSC_AAO_MasterCustomerId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["BCSC_AAO_MasterCustomerId_store_redirect"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[".ASPXANONYMOUS"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER - SOFT"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AT"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["BE_CLA3"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["OptanonAlertBoxClosed"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["OptanonConsent"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["PInfo"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__RequestVerificationToken"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__gads"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__gpi"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_clck"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_clsk"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_ga"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_ga_3PN52QWGQQ"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_gcl_au"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["dnn_IsMobile"].Expires = DateTime.Now.AddDays(-1);

            //Secure-qa.aao.org
            Response.Cookies["_ga"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_gid"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["feathr_session_id"].Expires = DateTime.Now.AddDays(-1);

            Response.Cookies["AAOMASTER-SOFT"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_gat_%5Bobject%20Object%5D"].Expires = DateTime.Now.AddDays(-1);

            //aao.org
            Response.Cookies[".ASPXANONYMOUS"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies[".DOTNETNUKE"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOCOOKIE"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AAOMASTER-SOFT"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["AT"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["BE_CLA3"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["LastPageId"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["OptanonAlertBoxClosed"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["OptanonConsent"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["PInfo"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__gads"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["__gpi"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_clck"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_clsk"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_dc_gtm_UA-2930883-9"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_ga"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_ga_3PN52QWGQQ"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_gcl_au"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_gid"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["dnn_IsMobile"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["language"].Expires = DateTime.Now.AddDays(-1);

            Response.Cookies["_dc_gtm_UA-216502086-1"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_dc_gtm_UA - 216502086 - 2"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_dc_gtm_UA - 2930883 - 9"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_ga_3PN52QWGQQ"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["_gat"].Expires = DateTime.Now.AddDays(-1);
        }

        //For Timeout check.
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult
            {
                Data = "Beat Generated"
            };
        }
    }
}
