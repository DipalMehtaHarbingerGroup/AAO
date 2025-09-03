namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using Newtonsoft.Json;

    public class AggregateUserPerformanceController : BaseAdminController
    {
        // POST: AggregateUserPerformance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminReportDetails(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
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
                AdminReportVM reportDetails = new AdminReportVM()
                {
                    ExamStartDate = examStartDate,
                    ExamCompletedDate = examCompletedDate,
                    Year = Convert.ToInt32(year),
                    OrderBy = sortColumn,
                    Seq = sortColumnDir,
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "AggregateUserPerformance/AdminReportDetails";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<AdminReportVM> report = new List<AdminReportVM>();
                report = JsonConvert.DeserializeObject<List<AdminReportVM>>(result);
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
            AdminReportVM reportDetails = new AdminReportVM()
            {
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
                Year = Convert.ToInt32(year),
            };

            string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "AggregateUserPerformance/AdminReportDetailsExport";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<ReportsDetailsVM> report = new List<ReportsDetailsVM>();
            report = JsonConvert.DeserializeObject<List<ReportsDetailsVM>>(result);
            var table = new System.Data.DataTable("report");
            table.Columns.Add("Section", typeof(string));
            table.Columns.Add("Correct", typeof(int));
            table.Columns.Add("InCorrect", typeof(int));
            table.Columns.Add("%Correct", typeof(string));
            table.Columns.Add("No of Resident attempted this section", typeof(int));
            foreach (var pro in report)
            {
                int subspecialtyId = pro.BCSCSectionNumber;
                string subspecialtyName = pro.SubspecialtyName.ToString();
                int correct = pro.Correct;
                int inCorrect = pro.InCorrect;
                string score = Convert.ToString(pro.Score);
                int noOfUser = pro.UserId;
                table.Rows.Add(new object[] { "Section " + subspecialtyId + ": " + subspecialtyName, correct, inCorrect, score + "%", noOfUser });
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Aggregate_User_Performance.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
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
    }
}