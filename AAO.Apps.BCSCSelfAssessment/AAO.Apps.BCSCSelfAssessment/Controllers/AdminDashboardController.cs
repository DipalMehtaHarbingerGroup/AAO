namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using Newtonsoft.Json;

    public class AdminDashboardController : BaseAdminController
    {
        // GET: AdminDashboard
        public ActionResult Index()
        {
            return RedirectToAction("Index", "BCSCMapper");
        }

        public ActionResult GetUserListIndex()
        {
            return View();
        }

        //public ActionResult ImportQASData()
        //{
        //    return View();
        //}

        //public ActionResult SubmitQASData()
        //{
        //    ServiceCallVM serviceCall = new ServiceCallVM
        //    {
        //        Status = 7,
        //    };
        //    string serviceJson = JsonConvert.SerializeObject(serviceCall);
        //    string urlSubmitData = AAOGlobalConstants.SiteWebAPIUrl + "admindashboard/SubmitQASData";
        //    string resultSubmitData = HttpProxy.HttpPost(urlSubmitData, serviceJson, "application/json; charset=utf-8", "POST");
        //    return View();
        //}

        //public ActionResult UpdateQASData()
        //{
        //    ServiceCallVM serviceCall = new ServiceCallVM
        //    {
        //        Status = 7,
        //        Modifiedafter = DateTime.Now.AddDays(-1),
        //    };
        //    string serviceJson = JsonConvert.SerializeObject(serviceCall);
        //    string urlUpdateData = AAOGlobalConstants.SiteWebAPIUrl + "admindashboard/UpdateQASData";
        //    string resultUpdateData = HttpProxy.HttpPost(urlUpdateData, serviceJson, "application/json; charset=utf-8", "POST");
        //    return View();
        //}

        //public ActionResult RetiredQASData()
        //{
        //    ServiceCallVM serviceCall = new ServiceCallVM
        //    {
        //        Status = 8,
        //        Modifiedafter = DateTime.Now.AddDays(-1),
        //    };
        //    string serviceJson = JsonConvert.SerializeObject(serviceCall);
        //    string urlRetiredData = AAOGlobalConstants.SiteWebAPIUrl + "admindashboard/RetiredQASData";
        //    string resultRetiredData = HttpProxy.HttpPost(urlRetiredData, serviceJson, "application/json; charset=utf-8", "POST");
        //    return View();
        //}

        public ActionResult GetQuestionCountFromQAS()
        {
            ServiceCallVM serviceCall = new ServiceCallVM
            {
                Status = 7,
            };
            string serviceJson = JsonConvert.SerializeObject(serviceCall);
            string urlGetCountdData = AAOGlobalConstants.SiteWebAPIUrl + "admindashboard/GetQuestionCount";
            string resultQuestionCount = HttpProxy.HttpPost(urlGetCountdData, serviceJson, "application/json; charset=utf-8", "POST");
            int questionCount = JsonConvert.DeserializeObject<int>(resultQuestionCount);
            return Json(questionCount, JsonRequestBehavior.AllowGet);
        }
    }
}