namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.Mvc;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using System.Configuration;
    using Serilog;
    using System.Linq;

    public class SpacedRepetitionController : BaseResidentController
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        public Serilog.Core.Logger log;

        // GET: ExamManager

        public SpacedRepetitionController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        // This Method Will Call the Web Service for getting the Exam Status Count For Spaced Repetition
        public ActionResult GetExamStatusCountSR()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "GetExamStatusCountSR", user._userId);
                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                };

                log.Information("UserId:{0}", user._userId);
                string serviceJson = JsonConvert.SerializeObject(serviceCall);
                string urlGetExamStatusCountSR = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/GetExamStatusCountSR";
                string resultExamStatusCount = HttpProxy.HttpPost(urlGetExamStatusCountSR, serviceJson, "application/json; charset=utf-8", "POST");
                ExamStatusCountSRVM examStatusCountSRVM = new ExamStatusCountSRVM();
                examStatusCountSRVM = JsonConvert.DeserializeObject<ExamStatusCountSRVM>(resultExamStatusCount);
                log.Information("AnsweredCorrectly:{0} AnsweredInCorrectly:{1}  TotalAnsweredQuestions:{2}", examStatusCountSRVM.AnsweredCorrectly, examStatusCountSRVM.AnsweredInCorrectly, examStatusCountSRVM.TotalAnsweredQuestions);
                log.Information("end Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "GetExamStatusCountSR", user._userId);
                return Json(resultExamStatusCount);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:SpacedRepetitionController" + " Method:GetExamStatusCountSR" + " UserId:" + user._userId);
                throw;
            }

        }

        // This method will call the Web Service For SR Performance Overview
        public ActionResult SRPerformanceOverview()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "SRPerformanceOverview", user._userId);
                log.Information("---SR performence preview  data ----");
                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                };
                log.Information("UserId:{0}", serviceCall.userId);
                string serviceJson = JsonConvert.SerializeObject(serviceCall);
                string urlGetExamStatusCountSR = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRPerformanceOverview";
                string resultExamStatusCount = HttpProxy.HttpPost(urlGetExamStatusCountSR, serviceJson, "application/json; charset=utf-8", "POST");
                List<SRPerformanceOverviewVM> sRPerformanceOverviewVM = new List<SRPerformanceOverviewVM>();
                sRPerformanceOverviewVM = JsonConvert.DeserializeObject<List<SRPerformanceOverviewVM>>(resultExamStatusCount);
                log.Information("end Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "SRPerformanceOverview", user._userId);
                return Json(resultExamStatusCount);

                //return Json(sRPerformanceOverviewVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId);
                throw;
            }
        }

        // This method will call the Web Service For SR Performance Overview Chapters
        public ActionResult SRPerformanceOverviewChapter(string subspecialityId)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} subspecialityId:{3}", "SpacedRepetitionController", "SRPerformanceOverview", user._userId, subspecialityId);
                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                    SubpecialityId = Convert.ToInt32(subspecialityId),
                };
                log.Information("userId:{0} SubpecialityId:{1}", serviceCall.userId, serviceCall.SubpecialityId);
                string serviceJson = JsonConvert.SerializeObject(serviceCall);
                string urlGetExamStatusCountSR = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRPerformanceOverviewChapter";
                string resultExamStatusCount = HttpProxy.HttpPost(urlGetExamStatusCountSR, serviceJson, "application/json; charset=utf-8", "POST");
                List<SRPerformanceOverviewChapterVM> sRPerformanceOverviewChapterVM = new List<SRPerformanceOverviewChapterVM>();
                sRPerformanceOverviewChapterVM = JsonConvert.DeserializeObject<List<SRPerformanceOverviewChapterVM>>(resultExamStatusCount);
                log.Information("end Controller :{0} method :{1} userId:{2} subspecialityId:{3}", "SpacedRepetitionController", "SRPerformanceOverview", user._userId, subspecialityId);
                return Json(resultExamStatusCount);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:SpacedRepetitionController" + " Method:SRPerformanceOverviewChapter" + " UserId:" + user._userId + "subspecialityId:" + subspecialityId);
                throw;
            }
        }

        // This method will call the Web Service For SR Performance Overview Topics
        public ActionResult SRPerformanceOverviewTopics(string chapterNumber, int subspecialityId, string ChapterName)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} chapterNumber:{3} subspecialityId:{4}", "SpacedRepetitionController", "SRPerformanceOverviewTopics", user._userId, chapterNumber, subspecialityId);
                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                    ChapterNumber = Convert.ToString(chapterNumber),
                    SubpecialityId = Convert.ToInt32(subspecialityId),
                    ChapterName = Convert.ToString(ChapterName),
                };
                string serviceJson = JsonConvert.SerializeObject(serviceCall);
                string urlGetExamStatusCountSR = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRPerformanceOverviewTopics";
                string resultExamStatusCount = HttpProxy.HttpPost(urlGetExamStatusCountSR, serviceJson, "application/json; charset=utf-8", "POST");
                List<SRPerformanceOverviewTopicsVM> sRPerformanceOverviewTopicsVM = new List<SRPerformanceOverviewTopicsVM>();
                sRPerformanceOverviewTopicsVM = JsonConvert.DeserializeObject<List<SRPerformanceOverviewTopicsVM>>(resultExamStatusCount);
                log.Information("end Controller :{0} method :{1} userId:{2} chapterNumber:{3} subspecialityId:{4}", "SpacedRepetitionController", "SRPerformanceOverviewTopics", user._userId, chapterNumber, subspecialityId);
                return Json(resultExamStatusCount);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "subspecialityId:" + subspecialityId);
                throw;
            }

        }

        // This method will call the Web Service For SR Recommended Topics
        public ActionResult SRRecommendedTopics()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "SRRecommendedTopics", user._userId);

                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                };
                log.Information("userId:{0}", serviceCall.userId);
                string serviceJson = JsonConvert.SerializeObject(serviceCall);
                string urlGetExamStatusCountSR = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRRecommendedTopics";
                string resultExamStatusCount = HttpProxy.HttpPost(urlGetExamStatusCountSR, serviceJson, "application/json; charset=utf-8", "POST");
                List<SRRecommendedTopicsVM> sRRecommendedTopicsVM = new List<SRRecommendedTopicsVM>();
                sRRecommendedTopicsVM = JsonConvert.DeserializeObject<List<SRRecommendedTopicsVM>>(resultExamStatusCount);
                log.Information("end Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "SRRecommendedTopics", user._userId);
                return Json(resultExamStatusCount);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:SpacedRepetitionController" + " method:SRRecommendedTopics" + " UserId:" + user._userId);
                throw;
            }
        }


        /*public ActionResult SRRecommendedTopics()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "SRRecommendedTopics", user._userId);

                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                };
                log.Information("userId:{0}", serviceCall.userId);
                string serviceJson = JsonConvert.SerializeObject(serviceCall);
                string urlGetExamStatusCountSR = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRRecommendedTopics";
                string resultExamStatusCount = HttpProxy.HttpPost(urlGetExamStatusCountSR, serviceJson, "application/json; charset=utf-8", "POST");

                List<SRRecommendedTopicsVM> sRRecommendedTopicsVM = JsonConvert.DeserializeObject<List<SRRecommendedTopicsVM>>(resultExamStatusCount);
                List<SRRecommendedTopicsVM> filteredTopics = new List<SRRecommendedTopicsVM>();
                HashSet<string> topicNames = new HashSet<string>();

                foreach (var topic in sRRecommendedTopicsVM)
                {
                    if (!topicNames.Contains(topic.TopicName))
                    {
                        topicNames.Add(topic.TopicName);
                        filteredTopics.Add(topic);
                    }
                }

                log.Information("end Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "SRRecommendedTopics", user._userId);
                return Json(filteredTopics);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:SpacedRepetitionController" + " method:SRRecommendedTopics" + " UserId:" + user._userId);
                throw;
            }
        }*/



        // This method will call the Web Service For SR Recommended Topics Export
        public ActionResult ExportToExcel()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "ExportToExcel", user._userId);
                log.Information("Export to excel.");

                ServiceCallVM serviceCall = new ServiceCallVM
                {
                    userId = user._userId,
                };
                log.Information("Export to excel for userid:{0}", serviceCall.userId);
                List<SRRecommendedTopicsVM> sRRecommendedTopicsVM = new List<SRRecommendedTopicsVM>();
                string examPostDataJson = JsonConvert.SerializeObject(serviceCall);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRRecommendedTopics";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                sRRecommendedTopicsVM = JsonConvert.DeserializeObject<List<SRRecommendedTopicsVM>>(result);
                var table = new System.Data.DataTable("RecommendedTopicsReport");
                table.Columns.Add("SectionName", typeof(string));
                table.Columns.Add("ChapterName", typeof(string));
                table.Columns.Add("TopicName", typeof(string));
                foreach (var pro in sRRecommendedTopicsVM)
                {
                    string sectionName = pro.SectionName.ToString();
                    string chapterName = pro.ChapterName.ToString();
                    string topicName = pro.TopicName.ToString();

                    table.Rows.Add(new object[] { sectionName, chapterName, topicName });
                }

                var grid = new GridView();
                grid.DataSource = table;
                grid.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Recommended Topics.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = string.Empty;
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                log.Information("end Controller :{0} method :{1} userId:{2}", "SpacedRepetitionController", "ExportToExcel", user._userId);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:SpacedRepetitionController" + " method:ExportToExcel" + " UserId:" + user._userId);
                throw;
            }
            return null;
        }

        // This Method Will Call the Web Service for getting Spaced Repetition Landing Page
        public ActionResult SRLanding()
        {
            //Added section for HG-459
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("Start Controller:{0} method:{1} userid:{2}", "SpacedRepetitionController", "SRLanding", user._userId);

                ExamDTO srExam = new ExamDTO();
                srExam.UserId = user._userId;
                string questionTypeDataJson = JsonConvert.SerializeObject(srExam);

                // Get SubSpeciality
                string url = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/GetSpecialityList";
                string result = HttpProxy.HttpPost(url, questionTypeDataJson, "application/json; charset=utf-8", "POST");
                List<SubSpecialityDetailVM> specialityType = JsonConvert.DeserializeObject<List<SubSpecialityDetailVM>>(result);
                List<SelectListItem> categoryType = new List<SelectListItem>();
                foreach (var item in specialityType)
                {
                    categoryType.Add(new SelectListItem()
                    {
                        Text = item.SpecialityName,
                        Value = item.SpecialityId.ToString() + ":" + item.TotalCount.ToString() + ":" + item.MarkQuestionCount.ToString() + ":" + item.IncorrectAsnwerCount.ToString() + ":" + item.ExamSkipQuestionCount.ToString(),
                        Selected = false,
                    });
                }

                ViewBag.CategoryTypelist = categoryType;
                log.Information("End Controller:{0} method:{1} userid:{2}", "SpacedRepetitionController", "SRLanding", user._userId);
                return View();
            }
            catch (Exception exc)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(exc.Message + " Controller:SpacedRepetitionController" + " method:SRLanding" + " userid:" + user._userId);
                throw;
            }
        }

        public ActionResult DeleteTopic(SRRecommendedTopicsDeletedVM delRecommendedTopic)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                delRecommendedTopic.UserId = user._userId;
                //examhistory.ExamId = Convert.ToInt32(ExamId);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "spacedRepetition/SRRecommendedDelete";
                var delRecommendedTopics = JsonConvert.SerializeObject(delRecommendedTopic);
                string result = HttpProxy.HttpPost(url, delRecommendedTopics, "application/json; charset=utf-8", "POST");
                return RedirectToAction("SRLanding", "SpacedRepetition");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}