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

    public class ResidentChapterReportController : BaseResidentController
    {
        // GET: ResidentChapterReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResidentChapterReportDetails(int? subspecialtyId, int? sectionNumber, DateTime? examStartDate = null, DateTime? examCompletedDate = null, string sectionTitle = null)
        {
            //UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            //JsonResult dataresult = new JsonResult();
            try
            {
                //ResidentChapterReportDetailsDTO reportChapterDetails = new ResidentChapterReportDetailsDTO();
                //reportChapterDetails.UserId = user._userId;
                //reportChapterDetails.SubspecialtyId = Convert.ToInt32(subspecialtyId);
                //reportChapterDetails.ExamStartDate = examStartDate;
                //reportChapterDetails.ExamCompletedDate = examCompletedDate;
                //string examPostDataJson = JsonConvert.SerializeObject(reportChapterDetails);
                //string url = AAOGlobalConstants.SiteWebAPIUrl + "ResidentChapterReport/ResidentChapterReportDetails";
                ////string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                //List<ResidentChapterReportDetailsDTO> report = new List<ResidentChapterReportDetailsDTO>();
               string report = null;// JsonConvert.DeserializeObject<List<ResidentChapterReportDetailsDTO>>(result);
                ViewBag.SubSpecialityId = subspecialtyId;
                ViewBag.StartDate = examStartDate;
                ViewBag.CompletedDate = examCompletedDate;
                //ViewBag.Report = report;
                ViewBag.Section = sectionTitle;// report[0].Subspecialty;
                ViewBag.SectionNumber = sectionNumber;// report[0].BCSCSectionNumber;
                //ViewBag.ReportCount = report.Count;
                return View(report);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public ActionResult ResidentChartDetail(int? subspecialtyId, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ResidentChapterReportDetailsDTO reportChapter = new ResidentChapterReportDetailsDTO();
            reportChapter.UserId = user._userId;
            reportChapter.SubspecialtyId = Convert.ToInt32(subspecialtyId);
            reportChapter.ExamStartDate = examStartDate;
            reportChapter.ExamCompletedDate = examCompletedDate;
            string serviceJson = JsonConvert.SerializeObject(reportChapter);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "ResidentChapterReport/ResidentChartDetail";
            string examPostDataJson = JsonConvert.SerializeObject(reportChapter);
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<ResidentChapterReportDetailsDTO> report = new List<ResidentChapterReportDetailsDTO>();
            report = JsonConvert.DeserializeObject<List<ResidentChapterReportDetailsDTO>>(result);
            return Json(report);
        }

        public ActionResult ExportToExcelGraph(int? subspecialtyId, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            ResidentChapterReportDetailsDTO reportChapter = new ResidentChapterReportDetailsDTO()
            {
                UserId = user._userId,
                SubspecialtyId = Convert.ToInt32(subspecialtyId),
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
            };

            string serviceJson = JsonConvert.SerializeObject(reportChapter);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "ResidentChapterReport/ResidentChartDetail";
            string examPostDataJson = JsonConvert.SerializeObject(reportChapter);
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<ResidentChapterReportDetailsDTO> report = new List<ResidentChapterReportDetailsDTO>();
            report = JsonConvert.DeserializeObject<List<ResidentChapterReportDetailsDTO>>(result);

            var table = new System.Data.DataTable("report");
            table.Columns.Add("Section", typeof(string));
            table.Columns.Add("Chapter Name", typeof(string));
            table.Columns.Add("Correct", typeof(int));
            table.Columns.Add("InCorrect", typeof(int));
            table.Columns.Add("RemainingQuestion", typeof(int));

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
            Response.AddHeader("content-disposition", "attachment; filename=Resident Section Chapter.xls");
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