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

    public class AssessmentController : BaseResidentController
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        // GET: Assesment
        private Serilog.Core.Logger log;

        // GET: ExamManager
        public AssessmentController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        public ActionResult Index(int? ExamId, int? Questionid, int? ChoiceSelected, int? ExamattemptId, int? CurrentQuestionid, bool? isRight)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                log.Information("start Controller :{0} method :{1} examId :{2} userId:{3} ", "AssessmentController", "Index", ExamId, user._userId);
                if (ExamId == null)
                {
                    TempData["Msg"] = "ExamId not Found ";
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    QuestionDetails question = new QuestionDetails();
                    question.examId = Convert.ToInt32(ExamId);
                    question.questionId = Convert.ToInt32(Questionid);
                    question.userId = user._userId;
                    string examPostDataJson = JsonConvert.SerializeObject(question);
                    log.Information("examId:{} questionId:{1} userId;{2}", question.examId, question.questionId, question.userId);
                    // Get Exam Question
                    string urlGetExamQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExamQuestion";
                    string resultGetExamQuestion = HttpProxy.HttpPost(urlGetExamQuestion, examPostDataJson, "application/json; charset=utf-8", "POST");
                    QuestionDetails questionDetails = new QuestionDetails();
                    questionDetails = JsonConvert.DeserializeObject<QuestionDetails>(resultGetExamQuestion);
                    if (questionDetails.userId != user._userId)
                    {
                        TempData["MsgError"] = "You can see your exam only.";
                        return RedirectToAction("Index", "Dashboard");
                    }

                    log.Information(" end Controller :{0} method :{1} examId :{2} userId:{3} ", "AssessmentController", "Index", ExamId, user._userId);
                    // Update If Exam Completed
                    // if (questionDetails.examStatus != 2)
                    // {
                    //    questionDetails.userId = user._userId;
                    //    string examAttemptDataJson = JsonConvert.SerializeObject(questionDetails);
                    //    var dataExamAttempt = Encoding.UTF8.GetBytes(examAttemptDataJson);
                    //    string urlUpdateExamAttempt = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/UpdateExamAttempt";
                    //    string resultUpdateExamAttempt = HttpProxy.HttpPost(urlUpdateExamAttempt, examAttemptDataJson, "application/json; charset=utf-8", "POST");
                    // }
                    if (questionDetails.choiceList.Count > 0)
                    {
                        return View(questionDetails);
                    }
                    else
                    {
                        TempData["Msg"] = "Question not found. Please try another action";
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Msg"] = "Exam not found. Please try another action";
                log.Information(" end Controller :{0} method :{1} Errormessage :{2} ", "AssessmentController", "Index", TempData["Msg"]);
                Console.Write(e.Message);
                log.Error(e.Message);
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [OutputCache(NoStore = true, Location = System.Web.UI.OutputCacheLocation.Client, Duration = 2)]
        public ActionResult QuestionListIndex(int? ExamId, int? Questionid, int? ChoiceSelected, int? ExamattemptId, int? CurrentQuestionid, bool? isRight, bool isExit = false, int?IsYourAnswer = 0, int? ButtonType = 0)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            log.Information("start Controller :{0} method :{1} examId:{2} userId:{2} QuestionId:{3}", "AssessmentController", "QuestionListIndex", ExamId, user._userId, Questionid);
            try
            {
                UpdateSkipAnswered updateAnswered = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    questionId = Convert.ToInt32(Questionid),
                    selectedChoiceId = Convert.ToInt32(ChoiceSelected),
                    examAttemptId = Convert.ToInt32(ExamattemptId),
                    isRight = Convert.ToBoolean(isRight),
                    userId = user._userId,
                    currentQuestionId = Convert.ToInt32(CurrentQuestionid),
                    isYourAnswer = Convert.ToInt32(IsYourAnswer),
                    ButtonType = (int)ButtonType,
                };
                ExamDTO examDetail = new ExamDTO();
                examDetail = GetExamDetailForTimer(updateAnswered);

                var time = Convert.ToDateTime(System.Web.HttpContext.Current.Session["ExamStartTime"]);
                log.Information("Time:{0}", time.ToString());
                //// Update Exam ConsumedTime
                // if (Convert.ToInt32(examDetail.ExamTimeType) != Convert.ToInt32(ExamManagerEnum.ExamTimeType.Time) && Convert.ToInt32(examDetail.ExamStatus) != Convert.ToInt32(ExamManagerEnum.ExamStatus.Completed))
                // {
                //    UpdateSkipAnswered updateTimer = new UpdateSkipAnswered
                //    {
                //        examAttemptId = Convert.ToInt32(ExamattemptId),
                //        consumedTimeDifference = Convert.ToDouble((DateTime.Now - Convert.ToDateTime(System.Web.HttpContext.Current.Session["ExamStartTime"])).TotalSeconds),
                //    };
                //    UpdateConsumedTime(updateTimer);
                // }

                // Update SkipAnsweredQuestion
                string questionListJson = JsonConvert.SerializeObject(updateAnswered);

                //if (examDetail.ExamType != Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition)
                //    || (examDetail.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) && ButtonType != 1 && ButtonType != 2))
                if (examDetail.ExamType != Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    string urlUpdateSkipQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/UpdateSkipAnswerQuestion";
                    HttpProxy.HttpPost(urlUpdateSkipQuestion, questionListJson, "application/json; charset=utf-8", "POST");

                }

                if (isExit)
                {
                    System.Web.HttpContext.Current.Session.Remove("RemTime");
                    System.Web.HttpContext.Current.Session.Remove("ExamId");
                    System.Web.HttpContext.Current.Session.Remove("ExamStartTime");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    // GetQuestionDetail
                    string urlGetExamQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExamQuestion";
                    string resultGetExamQuestion = HttpProxy.HttpPost(urlGetExamQuestion, questionListJson, "application/json; charset=utf-8", "POST");
                    QuestionDetails questionDetails = new QuestionDetails();
                    questionDetails = JsonConvert.DeserializeObject<QuestionDetails>(resultGetExamQuestion);
                    questionDetails.LastButtonClicked = (int)ButtonType;
                    if (examDetail.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) && (ButtonType == 3 || ButtonType == 4))
                    {

                        log.Information("end Controller :{0} method :{1} examId:{2} userId:{2}", "AssessmentController", "QuestionListIndex", ExamId, user._userId);
                        return new JsonResult();
                    }
                    else
                    {
                        log.Information("end Controller :{0} method :{1}", "AssessmentController", "QuestionListIndex");
                        return PartialView("QuestionListIndex", questionDetails);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + "UserId:" + user._userId+"ExamId:"+ ExamId + "Controller :{0}, method :{1}", "AssessmentController", "QuestionListIndex");
                Console.Write(e.Message);
                return null;
            }
        }

        public ActionResult ExamAttemptByuser(int? ExamId, int? Questionid, int? ChoiceSelected, int? ExamattemptId, bool? isRight, int? IsYourAnswer = 0, string userStatechoiceIDList = null, int? ButtonType = 0)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                log.Information("start Controller:{0}, method:{1}, userId:{2}, examId:{3}, QuestionId:{4}", "AssessmentController", "ExamAttemptByuser", user._userId, ExamId, Questionid);

                UpdateSkipAnswered updateAnswered = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    currentQuestionId = Convert.ToInt32(Questionid),
                    questionId = Convert.ToInt32(Questionid),
                    selectedChoiceId = Convert.ToInt32(ChoiceSelected),
                    examAttemptId = Convert.ToInt32(ExamattemptId),
                    isRight = Convert.ToBoolean(isRight),
                    userId = user._userId,
                    isYourAnswer = Convert.ToInt32(IsYourAnswer),
                    UserExamState = userStatechoiceIDList,
                    ButtonType = (int)ButtonType,
                };

                string urlUpdateSkipQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/UpdateSkipAnswerQuestion";
                string examAttemptJson = JsonConvert.SerializeObject(updateAnswered);
                string anseredQuestionIdResult = HttpProxy.HttpPost(urlUpdateSkipQuestion, examAttemptJson, "application/json; charset=utf-8", "POST");
                int answeredQuestionId = JsonConvert.DeserializeObject<int>(anseredQuestionIdResult);
                ExamDTO examDetail = new ExamDTO();
                examDetail = GetExamDetailForTimer(updateAnswered);
                var time = Convert.ToDateTime(System.Web.HttpContext.Current.Session["ExamStartTime"]);

                //// Update Exam ConsumedTime
                // if (Convert.ToInt32(examDetail.ExamTimeType) != Convert.ToInt32(ExamManagerEnum.ExamTimeType.Time) && Convert.ToInt32(examDetail.ExamStatus) != Convert.ToInt32(ExamManagerEnum.ExamStatus.Completed))
                // {
                //    UpdateSkipAnswered updateTimer = new UpdateSkipAnswered
                //    {
                //        examAttemptId = Convert.ToInt32(ExamattemptId),
                //        consumedTimeDifference = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(System.Web.HttpContext.Current.Session["ExamStartTime"])).TotalSeconds),
                //    };
                //    UpdateConsumedTime(updateTimer);
                // }

                if (answeredQuestionId == 0)
                {
                    return Json(answeredQuestionId, JsonRequestBehavior.AllowGet);
                }

                string urlGetExamQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExamQuestion";
                string resultGetExamQuestion = HttpProxy.HttpPost(urlGetExamQuestion, examAttemptJson, "application/json; charset=utf-8", "POST");
                QuestionDetails questionDetails = new QuestionDetails();
                questionDetails = JsonConvert.DeserializeObject<QuestionDetails>(resultGetExamQuestion);

                log.Information("end Controller:{0}, method:{1}, userId:{2}, examId:{3}", "AssessmentController", "ExamAttemptByuser", user._userId, ExamId);

                return Json(questionDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:{0}, method:{1}, UserId:{2}, ExamId:{3}, QuestionId:{4}", "AssessmentController", "ExamAttemptByuser", user._userId, ExamId, Questionid);
                throw;
            }
        }

        public ActionResult AddNotesAnswerText(int? ExamId, int? CurrentQuestionid, int? ExamattemptId, string ExamAttemptText, char TextType)
        {
            try
            {

                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} :exammId:{3}", "AssessmentController", "AddNotesAnswerText", user._userId, ExamId);
                UpdateSkipAnswered notesAnswer = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    currentQuestionId = Convert.ToInt32(CurrentQuestionid),
                    questionId = Convert.ToInt32(CurrentQuestionid),
                    examAttemptId = Convert.ToInt32(ExamattemptId),
                    userId = user._userId,
                    examAttemptText = Convert.ToString(ExamAttemptText),
                    textType = Convert.ToChar(TextType),
                };
                log.Information("examId:{0} questionId:{1}  examAttemptId:{2}  userId:{3} examAttemptText:{4} textType:{5}",
                    notesAnswer.examId, notesAnswer.questionId, notesAnswer.examAttemptId, notesAnswer.userId, notesAnswer.examAttemptText, notesAnswer.textType);
                string notesAnswerJson = JsonConvert.SerializeObject(notesAnswer);
                string urlAddNotesAnswerText = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/AddNotesAnswerText";
                HttpProxy.HttpPost(urlAddNotesAnswerText, notesAnswerJson, "application/json; charset=utf-8", "POST");
                string urlGetExamQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExamQuestion";
                string resultGetExamQuestion = HttpProxy.HttpPost(urlGetExamQuestion, notesAnswerJson, "application/json; charset=utf-8", "POST");
                QuestionDetails questionDetails = new QuestionDetails();
                questionDetails = JsonConvert.DeserializeObject<QuestionDetails>(resultGetExamQuestion);
                log.Information("end Controller :{0} method :{1} userId:{2} :exammId:{3}", "AssessmentController", "AddNotesAnswerText", user._userId, ExamId);
                return PartialView("QuestionListIndexContent", questionDetails);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + ExamId);
                throw;
            }
        }

        public ActionResult SaveIsYourAnswer(int? ExamId, int? Questionid, int? ExamattemptId, int? IsYourAnswer=0)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} ExamId:{2}", "AssessmentController", "SaveIsYourAnswer", ExamId);

                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                UpdateSkipAnswered updateAnswered = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    currentQuestionId = Convert.ToInt32(Questionid),
                    examAttemptId = Convert.ToInt32(ExamattemptId),
                    userId = user._userId,
                    isYourAnswer = Convert.ToInt32(IsYourAnswer),
                };
                log.Information("userId {0}", updateAnswered.userId);
                string saveIsYourAnswerJson = JsonConvert.SerializeObject(updateAnswered);
                string urlSaveIsYourAnswer = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/SaveIsYourAnswer";
                string resultSaveIsYourAnswer = HttpProxy.HttpPost(urlSaveIsYourAnswer, saveIsYourAnswerJson, "application/json; charset=utf-8", "POST");
                log.Information("end Controller :{0} method :{1} ExamId:{2}", "AssessmentController", "SaveIsYourAnswer", ExamId);
                return null;
            }
            catch (Exception ex )
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + ExamId);
                throw;
            }
        }

        public ActionResult MarkQuestionCheck(int? ExamId, int? CurrentQuestionid, int? ExamattemptId, bool? MarkedStatus)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} :examId:{3}", "AssessmentController", "MarkQuestionCheck", user._userId, ExamId);

                UpdateSkipAnswered updateMark = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    currentQuestionId = Convert.ToInt32(CurrentQuestionid),
                    examAttemptId = Convert.ToInt32(ExamattemptId),
                    userId = user._userId,
                    markedStatus = Convert.ToBoolean(MarkedStatus),
                };
                string questionMarkJson = JsonConvert.SerializeObject(updateMark);

                // string QuestionListJson = JsonConvert.SerializeObject(updateAnswered);
                string urlMarkQuestionCheck = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/MarkQuestionCheck";
                string resultMarkQuestionCheck = HttpProxy.HttpPost(urlMarkQuestionCheck, questionMarkJson, "application/json; charset=utf-8", "POST");
                bool result = JsonConvert.DeserializeObject<bool>(resultMarkQuestionCheck);
                log.Information("end Controller :{0} method :{1} userId:{2} :examId:{3}", "AssessmentController", "MarkQuestionCheck", user._userId, ExamId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + ExamId);
                throw;
            }
        }

        public void UpdateConsumedTime(UpdateSkipAnswered updateTime)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} examId:{2}", "AssessmentController", "UpdateConsumedTime", updateTime.examId);
                string examUpdateTimeJson = JsonConvert.SerializeObject(updateTime);
                string urlUpdateTime = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/UpdateConsumedTime";
                HttpProxy.HttpPost(urlUpdateTime, examUpdateTimeJson, "application/json; charset=utf-8", "POST");
                log.Information("end Controller :{0} method :{1} examId:{2}", "AssessmentController", "UpdateConsumedTime", updateTime.examId);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + updateTime.examId);
                throw;
            }
        }

        public ActionResult SubmitExam(int? ExamId, int? ExamattemptId)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId;{2} examId:{3} ExamattemptId:{4}", "AssessmentController", "SubmitExam", user._userId, ExamId, ExamattemptId);

                UpdateSkipAnswered updateAnswered = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    userId = user._userId,
                    examAttemptId = Convert.ToInt32(System.Web.HttpContext.Current.Session["ExamattemptId"]),
                };
                log.Information("examId:{0} userId:{1} examAttemptId:{2}", updateAnswered.examId, updateAnswered.userId, updateAnswered.examAttemptId);
                string questionListJson = JsonConvert.SerializeObject(updateAnswered);
                ExamDTO examDetail = new ExamDTO();
                examDetail = GetExamDetailForTimer(updateAnswered);

                //// Update Exam ConsumedTime
                // if (Convert.ToInt32(examDetail.ExamTimeType) != Convert.ToInt32(ExamManagerEnum.ExamTimeType.Time) && Convert.ToInt32(examDetail.ExamStatus) != Convert.ToInt32(ExamManagerEnum.ExamStatus.Completed))
                // {
                //    UpdateSkipAnswered updateTimer = new UpdateSkipAnswered
                //    {
                //        examAttemptId = Convert.ToInt32(ExamattemptId),
                //        consumedTimeDifference = Convert.ToInt32((DateTime.Now - Convert.ToDateTime(System.Web.HttpContext.Current.Session["ExamStartTime"])).TotalSeconds),
                //    };
                //    System.Web.HttpContext.Current.Session["ExamStartTime"] = DateTime.Now;
                //    UpdateConsumedTime(updateTimer);
                // }

                // Submit Exam
                System.Web.HttpContext.Current.Session["RemTime"] = null;
                string urlUpdateSkipQuestion = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/SubmitExam";
                string submitResult = HttpProxy.HttpPost(urlUpdateSkipQuestion, questionListJson, "application/json; charset=utf-8", "POST");
                int result = JsonConvert.DeserializeObject<int>(submitResult);
                log.Information("end Controller :{0} method :{1} userId;{2} examId:{3} ExamattemptId:{4}", "AssessmentController", "SubmitExam", user._userId, ExamId, ExamattemptId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + ExamId);
                throw;
            }
        }

        public ActionResult ViewProgress(int? ExamId, int? ExamattemptId)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId;{2} ExamattemptId:{3} ", "AssessmentController", "ViewProgress", user._userId, ExamattemptId);

                UpdateSkipAnswered updateAnswered = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(ExamId),
                    userId = user._userId,
                };
                log.Information("examId:{0} userId:{1} ", updateAnswered.examId, updateAnswered.userId);
                string questionListJson = JsonConvert.SerializeObject(updateAnswered);

                // Calling ViewProgress Service
                string urlViewProgress = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/ViewProgress";
                string viewProgressResult = HttpProxy.HttpPost(urlViewProgress, questionListJson, "application/json; charset=utf-8", "POST");
                List<ViewProgressVM> viewProgressVMList = new List<ViewProgressVM>();
                viewProgressVMList = JsonConvert.DeserializeObject<List<ViewProgressVM>>(viewProgressResult);
                log.Information("end Controller :{0} method :{1} userId;{2} ExamattemptId:{3} ", "AssessmentController", "ViewProgress", user._userId, ExamattemptId);
                return Json(viewProgressVMList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + ExamId);
                throw;
            }
        }

        public ExamDTO GetExamDetailForTimer(UpdateSkipAnswered examTimer)
        {
            try
            {
                log.Information("start Controller :{0} method :{1}", "AssessmentController examid:{2}", "GetExamDetailForTimer", examTimer.examId);
                string examPostDataJson = JsonConvert.SerializeObject(examTimer);
                string urlGetExam = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExamDetail";
                string resultGetExamdetail = HttpProxy.HttpPost(urlGetExam, examPostDataJson, "application/json; charset=utf-8", "POST");
                ExamDTO examDetail = new ExamDTO();
                examDetail = JsonConvert.DeserializeObject<ExamDTO>(resultGetExamdetail);
                return examDetail;
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + examTimer.examId + "Method: GetExamDetailForTimer");
                throw;
            }
        }

        public ActionResult GetExamTime(int examId)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} examId:{2} ", "AssessmentController", "GetExamTime", examId);
                int time = 0;
                UpdateSkipAnswered examTimer = new UpdateSkipAnswered();
                examTimer.examId = examId;
                // Get Exam Detail for Timer
                ExamDTO examDetail = new ExamDTO();
                examDetail = GetExamDetailForTimer(examTimer);
                if (Convert.ToInt32(examDetail.ExamTimeType) != Convert.ToInt32(ExamManagerEnum.ExamTimeType.Time) && ((Convert.ToInt32(examDetail.ExamStatus) != 2)))
                {
                    string urlGetTime = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/SetTimeForQuestion";
                    string examPostDataJson1 = JsonConvert.SerializeObject(examDetail);
                    string resultGetTime = HttpProxy.HttpPost(urlGetTime, examPostDataJson1, "application/json; charset=utf-8", "POST");
                    time = JsonConvert.DeserializeObject<int>(resultGetTime);
                    System.Web.HttpContext.Current.Session["AllocatedTime"] = time;
                    System.Web.HttpContext.Current.Session["ExamStartTime"] = DateTime.Now;
                    if (System.Web.HttpContext.Current.Session["RemTime"] == null)
                    {
                        System.Web.HttpContext.Current.Session["RemTime"] = DateTime.Now.AddSeconds(Convert.ToInt32(time)).ToString("dd-MM-yyyy h:mm:ss tt");
                        System.Web.HttpContext.Current.Session["ExamId"] = examDetail.ExamId;
                        System.Web.HttpContext.Current.Session["ExamStartTime"] = DateTime.Now;
                    }
                    else if (Session["RemTime"] != null && Convert.ToInt32(System.Web.HttpContext.Current.Session["ExamId"]) != examDetail.ExamId)
                    {
                        System.Web.HttpContext.Current.Session.Remove("RemTime");
                        System.Web.HttpContext.Current.Session.Remove("ExamId");
                        System.Web.HttpContext.Current.Session.Remove("ExamStartTime");
                        System.Web.HttpContext.Current.Session["RemTime"] = DateTime.Now.AddSeconds(Convert.ToInt32(time)).ToString("dd-MM-yyyy h:mm:ss tt");
                        System.Web.HttpContext.Current.Session["ExamId"] = examDetail.ExamId;
                        System.Web.HttpContext.Current.Session["ExamStartTime"] = DateTime.Now;
                    }

                    // ViewBag.RemTime = Session["RemTime"];
                }

                // return Json(System.Web.HttpContext.Current.Session["RemTime"], JsonRequestBehavior.AllowGet);
                log.Information("end Controller :{0} method :{1} examId:{2} ", "AssessmentController", "GetExamTime", examId);
                return Json(time, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamId:" + examId);
                throw;
            }
        }

        public ActionResult GetRightChoiceId(int? questionId)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                log.Information("start Controller :{0}, method :{1}, userId:{2}, questionId:{3}", "AssessmentController", "GetRightChoiceId", user._userId, questionId);
                RightQuestionIdVM rightQuestionId = new RightQuestionIdVM();
                rightQuestionId.RightQuestionId = Convert.ToInt32(questionId);
                log.Information("RightQuestionId:[0]", Convert.ToInt32(questionId));
                string questionPostDataJson = JsonConvert.SerializeObject(rightQuestionId);
                string urlGetRightChoiceId = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetRightChoiceId";
                string resultGetRightChoiceIl = HttpProxy.HttpPost(urlGetRightChoiceId, questionPostDataJson, "application/json; charset=utf-8", "POST");
                int rightChoiceId = JsonConvert.DeserializeObject<int>(resultGetRightChoiceIl);
                log.Information("end Controller :{0}, method :{1}, userId:{2}, questionId:{3}", "AssessmentController", "GetRightChoiceId", user._userId, questionId);
                return Json(rightChoiceId);
            }
            catch (Exception ex)
            {
                //UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller :{0}, method :{1}, userId:{2}, questionId:{3}", "AssessmentController", "GetRightChoiceId", user._userId, questionId);
                throw;
            }
        }

        // feedback mail Method
        public int SendFeedback(string feedbackBody, int id, string strFileName, string userName, string emailId)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} emailId:{3}", "AssessmentController", "SendFeedback", user._userId, emailId);
                Attachment attachment;
                string email = user._userEmail;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email);
                mail.IsBodyHtml = true;
                if (strFileName != string.Empty)
                {
                    mail.Subject = WebConfigurationManager.AppSettings["ShareResultSubject"] + userName;
                    attachment = new Attachment(strFileName);
                    mail.Attachments.Add(attachment);
                    mail.Body = feedbackBody;
                    if (emailId != string.Empty)
                    {
                        string[] emails = emailId.Split(',');
                        for (int i = 0; i < emails.Length; i++)
                        {
                            emails[i] = emails[i].Trim();
                            mail.To.Add(emails[i]);
                        }
                    }
                }
                else
                {
                    mail.To.Add(WebConfigurationManager.AppSettings["To"]);
                    mail.Subject = WebConfigurationManager.AppSettings["Subject"];
                    mail.Subject += id;
                    mail.Body = Server.UrlDecode(feedbackBody);
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = WebConfigurationManager.AppSettings["host"];
                smtp.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
                smtp.UseDefaultCredentials = false;

                // smtp.Credentials = new System.Net.NetworkCredential(WebConfigurationManager.AppSettings["userMailId"], WebConfigurationManager.AppSettings["Password"]);
                // smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Send(mail);
                mail.Attachments.Dispose();
                smtp.Dispose();
                log.Information("end Controller :{0} method :{1} userId:{2} emailId:{3}", "AssessmentController", "SendFeedback", user._userId, emailId);
                return 1;
            }
            catch (Exception e)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(e.Message + "UserId:" + user._userId );
                return 0;
            }
        }

        public int SendFeedbackSharedExam(string feedbackBody, string emailId)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} emailId:{3}", "AssessmentController", "SendFeedback", user._userId, emailId);
                string email = user._userEmail;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(email);
                mail.IsBodyHtml = true;
                //mail.Subject = WebConfigurationManager.AppSettings["SharedExamSubject"] + userName;
                mail.Subject = WebConfigurationManager.AppSettings["SharedExamSubject"];

                mail.Body = feedbackBody;
                if (emailId != string.Empty)
                {
                    string[] emails = emailId.Split(',');
                    for (int i = 0; i < emails.Length; i++)
                    {
                        emails[i] = emails[i].Trim();
                        mail.To.Add(emails[i]);
                    }
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = WebConfigurationManager.AppSettings["host"];
                smtp.Port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Send(mail);
                smtp.Dispose();
                log.Information("end Controller :{0} method :{1} userId:{2} emailId:{3}", "AssessmentController", "SendFeedback", user._userId, emailId);
                return 1;
            }
            catch (Exception e)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(e.Message + "UserId:" + user._userId);
                return 0;
            }
        }

        public void UpdateTime(int examattemptId)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} examattemptId:{2}", "AssessmentController", "UpdateTime", examattemptId);
                UpdateSkipAnswered updateTimer = new UpdateSkipAnswered
                {
                    examAttemptId = Convert.ToInt32(examattemptId),
                    consumedTimeDifference = 5,
                };
                log.Information("examAttemptId:{0} consumedTimeDifference:{1} ", updateTimer.examAttemptId, updateTimer.consumedTimeDifference);
                string examUpdateTimeJson = JsonConvert.SerializeObject(updateTimer);
                string urlUpdateTime = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/UpdateTime";
                HttpProxy.HttpPost(urlUpdateTime, examUpdateTimeJson, "application/json; charset=utf-8", "POST");

                log.Information("end Controller :{0} method :{1} examattemptId:{2}", "AssessmentController", "UpdateTime", examattemptId);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "examattemptId:" + examattemptId);
                throw;
            }
        }

        public ActionResult GetNotes(int examId, int questionId)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} examId:{2} questionId:{3} ", "AssessmentController", "GetNotes", examId, questionId);
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                UpdateSkipAnswered getNotes = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(examId),
                    currentQuestionId = Convert.ToInt32(questionId),
                    userId = user._userId,
                };
                log.Information("examId:{0} currentQuestionId:{1} userId:{2} ", getNotes.examId, getNotes.currentQuestionId, getNotes.userId);
                string notesListJson = JsonConvert.SerializeObject(getNotes);

                // Calling ViewProgress Service
                string urlGetNotes = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetNotes";
                string getNotesResult = HttpProxy.HttpPost(urlGetNotes, notesListJson, "application/json; charset=utf-8", "POST");
                List<string> notesList = new List<string>();
                notesList = JsonConvert.DeserializeObject<List<string>>(getNotesResult);
                log.Information("end Controller :{0} method :{1} examId:{2} questionId:{3} ", "AssessmentController", "GetNotes", examId, questionId);
                return Json(notesList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "examId:"+ examId);
                throw;
            }
        }

        public ActionResult GetExcerpt(int questionId, int examId, int userid)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} examId:{2} questionId:{3} userid:{4}", "AssessmentController", "GetExcerpt", examId, questionId, userid);

                // Assuming you have proper error handling/logging for HttpProxy.HttpPost method
                UpdateSkipAnswered getExcerpt = new UpdateSkipAnswered
                {
                    examId = Convert.ToInt32(examId),
                    questionId = Convert.ToInt32(questionId),
                    userId = userid // user._userId,
                };
                string getExcerptJson = JsonConvert.SerializeObject(getExcerpt);

                string urlGetExcerpts = AAOGlobalConstants.SiteWebAPIUrl + "Assessment/GetExcerpt";
                string getExcerptResult = HttpProxy.HttpPost(urlGetExcerpts, getExcerptJson, "application/json; charset=utf-8", "POST");

                // Deserialize the JSON response
                var excerptData = JsonConvert.DeserializeObject<QuestionDetails>(getExcerptResult);

                // Logging or any other operations on excerptData if needed

                // Return the excerpt data as JSON
                return Json(new { data = excerptData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + " examId:" + examId + " userid:" + userid);

                // Handle the exception (return error response, etc.)
                return Json(new { error = "An error occurred while processing the request." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}