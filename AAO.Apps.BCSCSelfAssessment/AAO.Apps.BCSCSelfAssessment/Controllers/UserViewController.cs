namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using System.Configuration;
    using Serilog;

    public class UserViewController : BaseAdminController
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        // GET: Assesment
        private Serilog.Core.Logger log;

        // GET: ExamManager
        public UserViewController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        // GET: QuestionReport
        public ActionResult Index()
        {
            QuestionDetails questionDetails = new QuestionDetails();
            questionDetails = null;
            return View(questionDetails);
        }

        public ActionResult GetQuestionByFriendly(int? questionid)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                log.Information("start Controller :{0} method :{1} Questionid :{2} userId:{3} ", "UserViewController", "Index", questionid, user._userId);
                if (questionid == null)
                {
                    TempData["Msg"] = "Questionid not Found ";
                    return RedirectToAction("Index", "BCSCMapper");

                }
                else
                {
                    QuestionDetails question = new QuestionDetails();
                    question.questionId = Convert.ToInt32(questionid);
                    question.userId = user._userId;
                    string QuestionPostDataJson = JsonConvert.SerializeObject(question);
                    log.Information("questionId:{} userId;{1}",
                        question.questionId, question.userId);
                    // Get Question
                    string urlGetQuestionById = AAOGlobalConstants.SiteWebAPIUrl + "UserView/GetQuestionById";
                    string resultGetExamQuestion = HttpProxy.HttpPost(urlGetQuestionById, QuestionPostDataJson, "application/json; charset=utf-8", "POST");
                    QuestionDetails questionDetails = new QuestionDetails();
                    questionDetails = JsonConvert.DeserializeObject<QuestionDetails>(resultGetExamQuestion);
                    ViewBag.QuestionId = questionid;
                    if (questionDetails.choiceList.Count > 0)
                    {
                        return View("index", questionDetails);
                    }
                    else
                    {
                        TempData["Msg"] = "Question not found. Please try another action";
                        return RedirectToAction("Index", "Userview");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Msg"] = "QuestionId not found. Please try another action";
                log.Information(" end Controller :{0} method :{1} Errormessage :{2} ", "Userview", "Index", TempData["Msg"]);
                Console.Write(e.Message);
                log.Error(e.Message);
                return RedirectToAction("Index", "Userview");
            }
        }
    }
}