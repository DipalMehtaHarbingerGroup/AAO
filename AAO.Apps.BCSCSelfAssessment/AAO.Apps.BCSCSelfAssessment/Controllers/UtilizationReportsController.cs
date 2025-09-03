namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Common.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using Proxy;

    public class UtilizationReportsController : BaseAdminController
    {
        // GET: UtilizationReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UtilizationReports(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            JsonResult dataresult = new JsonResult();
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                UtilizationReportsVM reportDetails = new UtilizationReportsVM()
                {
                    ExamStartDate = examStartDate,
                    ExamCompletedDate = examCompletedDate,
                    Year = Convert.ToInt32(year),
                    OrderBy = sortColumn,
                    Seq = sortColumnDir,
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "UtilizationReports/UtilizationReports";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<UtilizationReportsVM> report = new List<UtilizationReportsVM>();
                report = JsonConvert.DeserializeObject<List<UtilizationReportsVM>>(result);
                int totalRecords = report.Count;
                int recFilter = report.Count;
                dataresult = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = report }, JsonRequestBehavior.AllowGet);
                return dataresult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public ActionResult ExportToExcel(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            UtilizationReportsVM reportDetails = new UtilizationReportsVM()
            {
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
                Year = Convert.ToInt32(year),
            };

            string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "UtilizationReports/UtilizationReports";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<UtilizationReportsVM> report = new List<UtilizationReportsVM>();
            report = JsonConvert.DeserializeObject<List<UtilizationReportsVM>>(result);
            var table = new System.Data.DataTable("report");
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("Customer ID", typeof(string));
            table.Columns.Add("#Question Answered", typeof(int));
            table.Columns.Add("#Question Custom", typeof(int));
            table.Columns.Add("#Question Simulated", typeof(int));
            table.Columns.Add("#Question Quick", typeof(int));
            table.Columns.Add("#Question Challenged", typeof(int));
            table.Columns.Add("#Questions Spaced Repetition", typeof(int));

            foreach (var pro in report)
            {
                string userName = pro.UserName.ToString();
                string customerId = pro.CustomerId.ToString();
                int questionsAnswered = pro.QuestionAnswered;
                int questionsCustom = pro.QuestionsCustom;
                int questionsSimulated = pro.QuestionsSimulated;
                int questionsQuick = pro.QuestionsQuick;
                int questionsChallenged = pro.QuestionsChallenged;
                int questionsSpacedRepetition = pro.QuestionsSpacedRepetition;

                table.Rows.Add(new object[] { userName, customerId, questionsAnswered, questionsCustom, questionsSimulated, questionsQuick, questionsChallenged, questionsSpacedRepetition });
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Utilization Reports.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = string.Empty;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return null;
        }

        // GET: AggregateUserPerformance
        public ActionResult GetPYGYear()
        {
            AdminReportVM reportYearDetails = new AdminReportVM();
            string pgy = JsonConvert.SerializeObject(reportYearDetails);
            string urlpgy = AAOGlobalConstants.SiteWebAPIUrl + "AggregateUserPerformance/GetPYGYear";
            string result_GetListOfChapter = HttpProxy.HttpPost(urlpgy, pgy, "application/json; charset=utf-8", "POST");
            List<AdminReportVM> pgyYear = new List<AdminReportVM>();
            pgyYear = JsonConvert.DeserializeObject<List<AdminReportVM>>(result_GetListOfChapter);
            return Json(pgyYear, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AtAGlance(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            JsonResult dataresult = new JsonResult();
            try
            {
                UtilizationReportsVM reportDetails = new UtilizationReportsVM()
                {
                    ExamStartDate = examStartDate,
                    ExamCompletedDate = examCompletedDate,
                    Year = Convert.ToInt32(year),
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "UtilizationReports/AtAGlance";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                UtilizationReportsVM report = new UtilizationReportsVM();
                report = JsonConvert.DeserializeObject<UtilizationReportsVM>(result);
                int totalRecords = report.TotalUserCount;
                return Json(totalRecords, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}