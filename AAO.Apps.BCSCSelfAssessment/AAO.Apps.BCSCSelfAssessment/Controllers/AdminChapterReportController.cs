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
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;

    public class AdminChapterReportController : Controller
    {
        // GET: AdminChapterReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminChapterReportDetails(int year, int? subspecialtyId, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            JsonResult dataresult = new JsonResult();
            try
            {
                ResidentChapterReportDetailsDTO reportChapterDetails = new ResidentChapterReportDetailsDTO();

                reportChapterDetails.SubspecialtyId = Convert.ToInt32(subspecialtyId);
                reportChapterDetails.ExamStartDate = examStartDate;
                reportChapterDetails.ExamCompletedDate = examCompletedDate;
                reportChapterDetails.Year = Convert.ToInt32(year);
                string examPostDataJson = JsonConvert.SerializeObject(reportChapterDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "/AdminChapterReport/AdminChapterReportDetails";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<ResidentChapterReportDetailsDTO> report = new List<ResidentChapterReportDetailsDTO>();
                report = JsonConvert.DeserializeObject<List<ResidentChapterReportDetailsDTO>>(result);
                ViewBag.SubSpecialityId = subspecialtyId;
                ViewBag.StartDate = examStartDate;
                ViewBag.CompletedDate = examCompletedDate;
                ViewBag.Report = report;
                ViewBag.ReportCount = report.Count;
                ViewBag.Section = report[0].Subspecialty;
                ViewBag.SectionNumber = report[0].BCSCSectionNumber;
                ViewBag.Year = year;
                return View();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult AdminChartDetail(int year, int? subspecialtyId, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            ResidentChapterReportDetailsDTO reportChapter = new ResidentChapterReportDetailsDTO();

            reportChapter.SubspecialtyId = Convert.ToInt32(subspecialtyId);
            reportChapter.ExamStartDate = examStartDate;
            reportChapter.ExamCompletedDate = examCompletedDate;
            reportChapter.Year = Convert.ToInt32(year);
            string serviceJson = JsonConvert.SerializeObject(reportChapter);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "AdminChapterReport/AdminChartDetail";
            string examPostDataJson = JsonConvert.SerializeObject(reportChapter);
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<ResidentChapterReportDetailsDTO> report = new List<ResidentChapterReportDetailsDTO>();
            report = JsonConvert.DeserializeObject<List<ResidentChapterReportDetailsDTO>>(result);
            return Json(report);
        }

        public ActionResult ExportToExcelGraph(int year, int? subspecialtyId, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            ResidentChapterReportDetailsDTO reportChapter = new ResidentChapterReportDetailsDTO()
            {
                SubspecialtyId = Convert.ToInt32(subspecialtyId),
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
                Year = Convert.ToInt32(year),
            };
            string serviceJson = JsonConvert.SerializeObject(reportChapter);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "AdminChapterReport/AdminChartDetail";
            string examPostDataJson = JsonConvert.SerializeObject(reportChapter);
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<ResidentChapterReportDetailsDTO> report = new List<ResidentChapterReportDetailsDTO>();
            report = JsonConvert.DeserializeObject<List<ResidentChapterReportDetailsDTO>>(result);
            var table = new System.Data.DataTable("report");
            table.Columns.Add("Subspecialty Name", typeof(string));
            table.Columns.Add("Chapter Name", typeof(string));
            table.Columns.Add("Correct", typeof(int));
            table.Columns.Add("InCorrect", typeof(int));
            table.Columns.Add("Remaining Question", typeof(int));
            foreach (var pro in report)
            {
                string subspecialtyName = pro.Subspecialty.ToString();
                string chapterName = pro.ChapterName.ToString();
                int correct = pro.Correct;
                int inCorrect = pro.InCorrect;
                int remainingQuestion = pro.RemainingQuestion;

                table.Rows.Add(new object[] { subspecialtyName, chapterName, correct, inCorrect, remainingQuestion });
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=AdminSectionChapter.xls");
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