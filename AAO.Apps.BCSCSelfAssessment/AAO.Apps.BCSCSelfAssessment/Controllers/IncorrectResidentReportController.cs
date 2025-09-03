namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using Proxy;

    public class IncorrectResidentReportController : Controller
    {
        // POST: IncorrectRessidentReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncorrectQuestionDetails(int? subspecialtyId, DateTime? examStartDate = null, DateTime? examCompletedDate = null, int noOfRecords = 10, int pageNo = 1)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                IncorrectQuestionDetailsDTO incorrectReportDetails = new IncorrectQuestionDetailsDTO();
                incorrectReportDetails.UserId = user._userId;
                incorrectReportDetails.SubspecialtyId = Convert.ToInt32(subspecialtyId);
                incorrectReportDetails.ExamStartDate = examStartDate;
                incorrectReportDetails.ExamCompletedDate = examCompletedDate;
                incorrectReportDetails.NoOfRecords = noOfRecords;
                incorrectReportDetails.PageNo = pageNo;
                incorrectReportDetails.Year = 0;

                string examPostDataJson = JsonConvert.SerializeObject(incorrectReportDetails);
                var data = System.Text.Encoding.UTF8.GetBytes(examPostDataJson);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "IncorrectResidentReport/IncorrectQuestionDetails";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<QuestionDetails> incorrectQuestiondetails = new List<QuestionDetails>();
                incorrectQuestiondetails = JsonConvert.DeserializeObject<List<QuestionDetails>>(result);
                ViewBag.incorrectQuestiondetailscount = incorrectQuestiondetails.Count;
                ViewBag.incorrectQuestiondetailsdata = incorrectQuestiondetails;
                ViewBag.SubSpecialityId = subspecialtyId;
                ViewBag.NoOfRecords = noOfRecords;
                ViewBag.PageNo = pageNo;
                ViewBag.FirstSerialNum = incorrectQuestiondetails[0].serialNumber;
                ViewBag.LastSerialNum = incorrectQuestiondetails[incorrectQuestiondetails.Count - 1].serialNumber;
                ViewBag.SubSpecialityNumber = incorrectQuestiondetails[0].Section;
                ViewBag.SubSpeciality = incorrectQuestiondetails[0].SubSpeciality;
                ViewBag.RecordCount = incorrectQuestiondetails.Count > 0 ? incorrectQuestiondetails[0].QuestionCount : 0;
                ViewBag.ExamStartDate = examStartDate;
                ViewBag.ExamCompletedDate = examCompletedDate;
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}