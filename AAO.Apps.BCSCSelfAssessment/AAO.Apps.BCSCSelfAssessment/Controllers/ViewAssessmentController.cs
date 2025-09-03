namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using System.Linq;

    public class ViewAssessmentController : BaseResidentController
    {
        // GET: ViewExamHistory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAssessmentDetails(int? ExamId)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
           {
                ExamQuestionDTO examque = new ExamQuestionDTO();
                examque.UserId = user._userId;
                examque.ExamId = Convert.ToInt32(ExamId);
                string examPostDataJson = JsonConvert.SerializeObject(examque);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "ViewAssessment/ViewAssessmentDetails";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");

                List<QuestionDetails> viewAssessmentdetails = new List<QuestionDetails>();
                viewAssessmentdetails = JsonConvert.DeserializeObject<List<QuestionDetails>>(result);
                ViewBag.viewAssessmentdetailscount = viewAssessmentdetails.Count;
                ViewBag.viewAssessmentdetails = viewAssessmentdetails;
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult FilterByQuestions(int? ExamId,string Filter, int NoOfRecords = 10, int PageNo = 1)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                 AssesmentDetailVM assDetail = new AssesmentDetailVM();
                 assDetail.ExamId = Convert.ToInt32(ExamId);
                 assDetail.Filter = Convert.ToString(Filter);
                 assDetail.NoOfRecords = NoOfRecords;
                 assDetail.PageNo = PageNo;
                 assDetail.UserId = user._userId;
                 string examPostDataJson = JsonConvert.SerializeObject(assDetail);
                 var data = System.Text.Encoding.UTF8.GetBytes(examPostDataJson);
                 string url = AAOGlobalConstants.SiteWebAPIUrl + "ViewAssisment/FilterByQuestions";
                 string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");

                 List<QuestionDetails> filterByQuestions = new List<QuestionDetails>();
                 filterByQuestions = JsonConvert.DeserializeObject<List<QuestionDetails>>(result);
                 if (filterByQuestions.Count > 0)
                {
                    ViewBag.FirstSerialNum = filterByQuestions[0].serialNumber;
                    ViewBag.LastSerialNum = filterByQuestions[filterByQuestions.Count - 1].serialNumber;
                }

                 ViewBag.ExamId = ExamId;
                 ViewBag.Filter = Filter;
                 ViewBag.NoOfRecords = NoOfRecords;
                 ViewBag.PageNo = PageNo;
                 ViewBag.viewCurrentRecordcount = filterByQuestions.Count;
                 ViewBag.viewAssessmentdetailscount = filterByQuestions.Count > 0 ? filterByQuestions[0].QuestionCount : 0;
                 ViewBag.viewAssessmentdetails = filterByQuestions;
                 return View("ViewAssessmentDetails");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}