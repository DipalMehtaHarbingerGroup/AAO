namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;

    [AllowAnonymous]
    public class HomeController : BaseController
    {
        // protected override void OnActionExecuting(ActionExecutingContext filterContext)
        // {
        //    //var userSession;
        //    var userSession = Session["UserID"];
        //    if (userSession == null)
        //    {
        //        TempData["Msg"] = "Access denied !";
        //        filterContext.Result = new RedirectToRouteResult(
        //             new RouteValueDictionary
        //                 {{"controller", "Login"}, {"action", "SignOut"}});
        //        return;
        //    }
        // }
        public ActionResult Index()
        {
            string storeUrl = ConfigurationManager.AppSettings["STORE_URL"];
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("A"))
                {
                    return RedirectToAction("Index", "AdminDashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            else
            {
                //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId");
                //System.Web.HttpContext.Current.Session.Remove("BCSC_AAO_MasterCustomerId_store_redirect");
                System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId");
                System.Web.HttpContext.Current.Response.Cookies.Remove("BCSC_AAO_MasterCustomerId_store_redirect");
                return Redirect(storeUrl);
            }

            // string encryptedString = Cryptography.Encrypt("TestUser");
            // string decryptedString = Cryptography.Decrypt(encryptedString);
            // return View();
        }
    }
}