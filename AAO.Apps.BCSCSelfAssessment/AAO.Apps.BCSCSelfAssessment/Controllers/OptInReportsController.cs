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

    public class OptInReportsController : BaseAdminController
    {
        // GET: OptInReports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OptInReports(int year)
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
                OptInReports reportDetails = new OptInReports()
                {
                    Year = Convert.ToInt32(year),
                    OrderBy = sortColumn,
                    Seq = sortColumnDir,
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "OptInReports/OptInReports";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<OptInReports> report = new List<OptInReports>();
                report = JsonConvert.DeserializeObject<List<OptInReports>>(result);
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

        public ActionResult GetOptIn(int year)
        {
            JsonResult dataresult = new JsonResult();
            try
            {
                OptInReports reportDetails = new OptInReports()
                {
                   Year = Convert.ToInt32(year),
                };
                string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "OptInReports/GetOptIn";
                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<OptInReports> optInReports = new List<OptInReports>();
                optInReports = JsonConvert.DeserializeObject<List<OptInReports>>(result);
                OptInReports report = new OptInReports();
                report.TotalOptInAcceptCount = optInReports.Where(a => a.OptIn == "Y").Count();
                report.TotalOptInDeclineCount = optInReports.Where(a => a.OptIn == "N").Count();
                report.TotalUserCount = optInReports.Count;
                return Json(report, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public ActionResult ExportToExcel(int year)
        {
            OptInReports reportDetails = new OptInReports()
            {
                Year = Convert.ToInt32(year),
            };

            string examPostDataJson = JsonConvert.SerializeObject(reportDetails);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "OptInReports/OptInReports";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<OptInReports> report = new List<OptInReports>();
            report = JsonConvert.DeserializeObject<List<OptInReports>>(result);
            var table = new System.Data.DataTable("report");
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("Customer ID", typeof(string));
            table.Columns.Add("#Status", typeof(string));
            foreach (var pro in report)
            {
                string userName = pro.UserName.ToString();
                string customerId = pro.MasterCustomerID.ToString();
                string status = pro.OptIn.ToString();

                table.Rows.Add(new object[] { userName, customerId, status });
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=OptInReports.xls");
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
    }
}