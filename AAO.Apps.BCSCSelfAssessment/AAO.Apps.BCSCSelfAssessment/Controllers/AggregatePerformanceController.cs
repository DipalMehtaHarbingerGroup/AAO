namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using Newtonsoft.Json;

    public class AggregatePerformanceController : BaseResidentController
    {
        // POST: AggregatePerformance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReportDetails(DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            JsonResult dataresult = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                try
                {
                    ReportsDetailsVM reportDetails = new ReportsDetailsVM()
                    {
                        UserId = user._userId,
                        ExamStartDate = examStartDate,
                        ExamCompletedDate = examCompletedDate,
                    };
                    string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                    string url = AAOGlobalConstants.SiteWebAPIUrl + "AggregatePerformance/ReportDetails";
                    string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                    List<ReportsDetailsVM> report = new List<ReportsDetailsVM>();
                    report = JsonConvert.DeserializeObject<List<ReportsDetailsVM>>(result);
                    int totalRecords = report.Count;
                    int recFilter = report.Count;
                    dataresult = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = report }, JsonRequestBehavior.AllowGet);
                    return dataresult;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // POST: AggregatePerformance
        public ActionResult ExportToExcel(DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ReportsDetailsVM reportDetails = new ReportsDetailsVM()
            {
                UserId = user._userId,
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
            };

            string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "AggregatePerformance/ReportDetails";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<ReportsDetailsVM> report = new List<ReportsDetailsVM>();
            report = JsonConvert.DeserializeObject<List<ReportsDetailsVM>>(result);
            var table = new System.Data.DataTable("report");
            table.Columns.Add("Section", typeof(string));
            table.Columns.Add("Correct", typeof(int));
            table.Columns.Add("InCorrect", typeof(int));
            table.Columns.Add("%Correct", typeof(int));
            foreach (var pro in report)
            {
                int bCSCSectionNumber = pro.BCSCSectionNumber;
                string subspecialtyName = pro.SubspecialtyName.ToString();
                int correct = pro.Correct;
                int inCorrect = pro.InCorrect;
                int score = pro.Score;

                table.Rows.Add(new object[] { "Section " + bCSCSectionNumber + ": " + subspecialtyName, correct, inCorrect, score });
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Aggregate Performance.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = " ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return null;
        }
    }
}