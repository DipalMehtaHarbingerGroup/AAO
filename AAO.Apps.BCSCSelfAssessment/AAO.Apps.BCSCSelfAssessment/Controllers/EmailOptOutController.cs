using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using System.Web.Mvc;

    /*[AllowAnonymous]*/
    public class EmailOptOutController : Controller
    {
        // GET: EmailOptOut
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OptOut(int optOut)
        {
            ExamQuestionDTO optOutob = new ExamQuestionDTO();
            optOutob.Id = optOut;

            string optOutId = JsonConvert.SerializeObject(optOutob);
            string optOutIdUrl = AAOGlobalConstants.SiteWebAPIUrl + "ExamHistory/OptOut";
            string optOutIdResult = HttpProxy.HttpPost(optOutIdUrl, optOutId, "application/json; charset=utf-8", "POST");
            string optOutIdDetails = JsonConvert.DeserializeObject<string>(optOutIdResult);

            // Pass optOutIdDetails to the Index view using ViewBag
            ViewBag.OptOutIdDetails = optOutIdDetails;

            return View("Index");
        }
    }
}