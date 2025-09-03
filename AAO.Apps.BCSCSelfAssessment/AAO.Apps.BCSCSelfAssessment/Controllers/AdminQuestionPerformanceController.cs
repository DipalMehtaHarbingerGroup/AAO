namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using ClosedXML.Excel;
    using Newtonsoft.Json;
    using OfficeOpenXml;
    using Serilog;

    public class AdminQuestionPerformanceController : BaseAdminController
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private Serilog.Core.Logger log;

        public AdminQuestionPerformanceController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        // POST: AdminQuestionPerformance
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminQuestionPerformanceDetails(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            AdminQuestionPerformanceVM countDetails = new AdminQuestionPerformanceVM()
            {
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
                Year = Convert.ToInt32(2),
            };
            string questionCountJson = JsonConvert.SerializeObject(countDetails);
            string counturl = AAOGlobalConstants.SiteWebAPIUrl + "AdminQuestionPerformance/GetAllQuestionCount";
            string countResult = HttpProxy.HttpPost(counturl, questionCountJson, "application/json; charset=utf-8", "POST");
            int queCount = JsonConvert.DeserializeObject<int>(countResult);
            JsonResult dataresult = new JsonResult();
            try
            {
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();

                // var sortColumn1 = Request.Form.GetValues("order[0][column]").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);

                try
                {
                    AdminQuestionPerformanceVM queDetails = new AdminQuestionPerformanceVM()
                    {
                        ExamStartDate = examStartDate,
                        ExamCompletedDate = examCompletedDate,
                        Year = Convert.ToInt32(year),
                        NoOfRecords = pageSize,
                        PageNo = Convert.ToInt32(draw),
                        OrderBy = sortColumn,
                        Seq = sortColumnDir,
                    };
                    string examPostDataJson = JsonConvert.SerializeObject(queDetails);
                    string url = AAOGlobalConstants.SiteWebAPIUrl + "AdminQuestionPerformance/AdminQuestionPerformanceDetails";
                    string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                    List<AdminQuestionPerformanceVM> report = new List<AdminQuestionPerformanceVM>();
                    report = JsonConvert.DeserializeObject<List<AdminQuestionPerformanceVM>>(result);
                    int totalRecords = queCount;
                    int recFilter = queCount;
                    // report = report.Skip(startRec).Take(pageSize).ToList();
                    dataresult = Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = report }, JsonRequestBehavior.AllowGet);

                    return (dataresult);
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

        public ActionResult ExportToExcel(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null)
        {
            AdminQuestionPerformanceVM queDetails = new AdminQuestionPerformanceVM()
            {
                ExamStartDate = examStartDate,
                ExamCompletedDate = examCompletedDate,
                Year = Convert.ToInt32(year),
            };

            string examPostDataJson = JsonConvert.SerializeObject(queDetails);
            string url = AAOGlobalConstants.SiteWebAPIUrl + "AdminQuestionPerformance/GetAllQuestionExport";
            string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<AdminQuestionPerformanceVM> quereport = new List<AdminQuestionPerformanceVM>();
            quereport = JsonConvert.DeserializeObject<List<AdminQuestionPerformanceVM>>(result);
            var table = new System.Data.DataTable("quereport");
            table.Columns.Add("Question Stem", typeof(string));
            table.Columns.Add("Question Id", typeof(int));
            table.Columns.Add("Section", typeof(string));
            table.Columns.Add("Chapter", typeof(string));
            table.Columns.Add("Topic", typeof(string));
            table.Columns.Add("# users answered correctly", typeof(int));
            table.Columns.Add("# users answered Incorrectly", typeof(int));
            table.Columns.Add("# users repeated at least once in SR mode", typeof(int));
            table.Columns.Add("% users answered correctly", typeof(string));
            table.Columns.Add("% users answered Incorrectly", typeof(string));
            table.Columns.Add("Correct answer", typeof(string));
            table.Columns.Add("Distractor 1", typeof(string));
            table.Columns.Add("# who chose Distractor 1", typeof(int));
            table.Columns.Add("Distractor 2", typeof(string));
            table.Columns.Add("# who chose Distractor 2", typeof(int));
            table.Columns.Add("Distractor 3", typeof(string));
            table.Columns.Add("# who chose Distractor 3", typeof(int));
            table.Columns.Add("Avg # times incorrect before correct", typeof(string));
            table.Columns.Add("# users correct first try", typeof(int));

            foreach (var pro in quereport)
            {
                string stem = pro.Stem.ToString();
                int questionId = pro.QuestionId;
                string section = pro.Subspecialty.ToString();
                string chapter = pro.Chapter.ToString();
                string topic = pro.Topic.ToString();
                int userAnsweredcorrectly = pro.UserAnsweredcorrectly;
                int userAnsweredincorrectly = pro.UserAnsweredincorrectly;
                int usersRepeatedSRMode = pro.UsersRepeatedSRMode;
                string percentagecorrectly = pro.Percentagecorrectly.ToString();
                string percentageIncorrectly = pro.PercentageIncorrectly.ToString();
                string correctAnswer = pro.CorrectAnswer.ToString();
                string distractor_1 = pro.Distractor_1.ToString();
                int user_chose_distractor_1 = pro.User_chose_distractor_1;
                string distractor_2 = pro.Distractor_2.ToString();
                int user_chose_distractor_2 = pro.User_chose_distractor_2;
                string distractor_3 = pro.Distractor_3.ToString();
                int user_chose_distractor_3 = pro.User_chose_distractor_3;
                string avg_times_incorrect_before_correct = pro.Avg_times_incorrect_before_correct;
                int users_correct_first_try = pro.Users_correct_first_try;

                table.Rows.Add(new object[] { stem, questionId, section, chapter, topic, userAnsweredcorrectly, userAnsweredincorrectly,
                                                usersRepeatedSRMode, percentagecorrectly + "%", percentageIncorrectly + "%", correctAnswer,
                                                distractor_1, user_chose_distractor_1,
                                                distractor_2, user_chose_distractor_2,
                                                distractor_3, user_chose_distractor_3,
                                                avg_times_incorrect_before_correct,
                                                users_correct_first_try,
                                            }
                                );
            }

            var grid = new GridView();
            grid.DataSource = table;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Question Performance.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = " ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            //Response.Flush();
            Response.End();
            return null;
        }

        public async Task<ActionResult> ExportAllQuestionToExcel(int year, DateTime? examStartDate = null, DateTime? examCompletedDate = null, bool? optOutValue = false)
        {
            log.Information(" Start of Controller :{0} method :{1} Year :{2} startDate:{3} completedDate:{4}", "AdminQuestionPerformanceController", "ExportAllQuestionToExcel", year, examStartDate, examCompletedDate);
            int recordPerSheet = Convert.ToInt32(ConfigurationManager.AppSettings["RecordPerSheet"]);

            try
            {
                AdminQuestionPerformanceVM queDetails = new AdminQuestionPerformanceVM()
                {
                    ExamStartDate = examStartDate,
                    ExamCompletedDate = examCompletedDate,
                    Year = Convert.ToInt32(year),
                    OptOutValue = Convert.ToBoolean(optOutValue),
                };

                string examPostDataJson = JsonConvert.SerializeObject(queDetails);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "AdminQuestionPerformance/GetTableCountExport";

                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                int tableCount = JsonConvert.DeserializeObject<int>(result);


                HttpAPI proxy = new HttpAPI();
                List<AdminExamQuestionReport> quereport = new List<AdminExamQuestionReport>();
                quereport = proxy.GetData(tableCount);
                if (quereport != null)
                {
                    //string rootFolder = ConfigurationManager.AppSettings["ExcelPath"];
                    //string fileName = @"ExportQuestionPerformance.xlsx";
                    //FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

                    // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    // using (ExcelPackage package = new ExcelPackage(file))
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        int counter = 0;
                        int sheetNumber = 0;

                        ExcelWorksheet worksheet = GetWorkSheet(package, sheetNumber);
                        int totalRows = quereport.Count();
                        int row = 2;
                        foreach (var pro in quereport)
                        {
                            counter++;
                            worksheet.Cells[row, 1].Value = pro.Email;
                            worksheet.Cells[row, 2].Value = pro.MasterCustomerID;
                            worksheet.Cells[row, 3].Value = pro.UserName;
                            worksheet.Cells[row, 4].Value = pro.OptInStatus;
                            worksheet.Cells[row, 5].Value = pro.ExamId;
                            worksheet.Cells[row, 6].Value = pro.ExamStartDate;
                            worksheet.Cells[row, 7].Value = pro.ExamType;
                            worksheet.Cells[row, 8].Value = pro.SelectedSubspeciality;
                            worksheet.Cells[row, 9].Value = pro.NoOfQuestions;
                            worksheet.Cells[row, 10].Value = pro.TypeOfQuestion;
                            worksheet.Cells[row, 11].Value = pro.ExamMode;
                            worksheet.Cells[row, 12].Value = pro.ExamAnswerToShow;
                            worksheet.Cells[row, 13].Value = pro.ExamTimeType;
                            worksheet.Cells[row, 14].Value = pro.Stem;
                            worksheet.Cells[row, 15].Value = pro.QuestionId;
                            worksheet.Cells[row, 16].Value = pro.BCSCSectionTitle;
                            worksheet.Cells[row, 17].Value = pro.ChapterName;
                            worksheet.Cells[row, 18].Value = pro.TopicName;
                            worksheet.Cells[row, 19].Value = pro.ChallengeModeTypedAnswer;
                            worksheet.Cells[row, 20].Value = pro.ChallengeModeAnswerText;
                            worksheet.Cells[row, 21].Value = pro.UserSelectedChoice;
                            worksheet.Cells[row, 22].Value = pro.UserAnsweredcorrectly;
                            worksheet.Cells[row, 23].Value = pro.UserAnsweredincorrectly;
                            worksheet.Cells[row, 24].Value = pro.UsersRepeatedSRMode;
                            worksheet.Cells[row, 25].Value = pro.Percentagecorrectly;
                            worksheet.Cells[row, 26].Value = pro.PercentageIncorrectly;
                            worksheet.Cells[row, 27].Value = pro.CorrectAnswer;
                            worksheet.Cells[row, 28].Value = pro.Distractor_1;
                            worksheet.Cells[row, 29].Value = pro.User_chose_distractor_1;
                            worksheet.Cells[row, 30].Value = pro.Distractor_2;
                            worksheet.Cells[row, 31].Value = pro.User_chose_distractor_2;
                            worksheet.Cells[row, 32].Value = pro.Distractor_3;
                            worksheet.Cells[row, 33].Value = pro.User_chose_distractor_3;
                            if (counter > recordPerSheet)
                            {
                                log.Information(" Excel Export added sheet no {0}", sheetNumber);
                                sheetNumber++;
                                counter = 0;
                                row = 2;
                                worksheet = GetWorkSheet(package, sheetNumber);
                            }

                            row++;
                        }

                        package.Save();

                        using (MemoryStream stream = new MemoryStream())
                        {
                            package.SaveAs(stream);
                            //log.Information(" Excel Generation completed for startDate:{0} completedDate:{1}", examStartDate, examCompletedDate);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ALLQuestionPerformance.xlsx");
                        }

                        /*.xlsx export ends */
                    }
                }
            }
            catch (Exception ex)
            {
                log.Information(" Excel Generation failed for startDate:{0} completedDate:{1} and error message: {2}", examStartDate, examCompletedDate, ex.Message);
            }

            //{
            //    log.Information(" end of Controller :{0} method :{1} Error :{2} ", "AdminQuestionPerformanceController", "ExportAllQuestionToExcel", ex.Message);
            //}

            return null;
        }

        public DataTable GetTable(int count)
        {
            var table = new System.Data.DataTable("quereport" + count);
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Master Customer ID", typeof(string));
            table.Columns.Add("User Name", typeof(string));
            table.Columns.Add("Opt In Status", typeof(string));

            table.Columns.Add("Exam Id", typeof(int));
            table.Columns.Add("Exam StartDate", typeof(string));
            table.Columns.Add("Exam Type", typeof(string));
            table.Columns.Add("Selected Subspeciality", typeof(string));
            table.Columns.Add("No Of Questions", typeof(int));
            table.Columns.Add("Type Of Question", typeof(string));
            table.Columns.Add("Exam Mode", typeof(string));
            table.Columns.Add("Show Correct Answers", typeof(string));
            table.Columns.Add("Exam Time Type", typeof(string));
            table.Columns.Add("Question Stem", typeof(string));
            table.Columns.Add("Question Id", typeof(int));
            table.Columns.Add("BCSC Section Title", typeof(string));
            table.Columns.Add("Chapter Name", typeof(string));
            table.Columns.Add("Topic Name", typeof(string));
            table.Columns.Add("Challenged Mode Typed Answer", typeof(string));
            table.Columns.Add("Challenged Mode Answer Text", typeof(string));
            table.Columns.Add("User Selected Choice", typeof(string));

            table.Columns.Add("# users answered correctly", typeof(int));
            table.Columns.Add("# users answered Incorrectly", typeof(int));
            table.Columns.Add("# users repeated at least once in SR mode", typeof(int));
            table.Columns.Add("% users answered correctly", typeof(string));
            table.Columns.Add("% users answered Incorrectly", typeof(string));
            table.Columns.Add("Correct answer", typeof(string));
            table.Columns.Add("Distractor 1", typeof(string));
            table.Columns.Add("# who chose Distractor 1", typeof(int));
            table.Columns.Add("Distractor 2", typeof(string));
            table.Columns.Add("# who chose Distractor 2", typeof(int));
            table.Columns.Add("Distractor 3", typeof(string));
            table.Columns.Add("# who chose Distractor 3", typeof(int));

            return table;
        }

        public ActionResult ExcelGeneration(List<AdminExamQuestionReport> quereport)
        {
            try
            {
                string rootFolder = ConfigurationManager.AppSettings["ExcelPath"];
                string fileName = @"ExportQuestionPerformance.xlsx";

                FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    int counter = 0;
                    int sheetNumber = 0;

                    ExcelWorksheet worksheet = GetWorkSheet(package, sheetNumber);//package.Workbook.Worksheets.Add("Customer");
                    int totalRows = quereport.Count();
                    int row = 2;
                    foreach (var pro in quereport)
                    {
                        counter++;
                        worksheet.Cells[row, 1].Value = pro.Email;
                        worksheet.Cells[row, 2].Value = pro.MasterCustomerID;
                        worksheet.Cells[row, 3].Value = pro.UserName;
                        worksheet.Cells[row, 4].Value = pro.OptInStatus;
                        worksheet.Cells[row, 5].Value = pro.ExamId;
                        worksheet.Cells[row, 6].Value = pro.ExamStartDate;
                        worksheet.Cells[row, 7].Value = pro.ExamType;
                        worksheet.Cells[row, 8].Value = pro.SelectedSubspeciality;
                        worksheet.Cells[row, 9].Value = pro.NoOfQuestions;
                        worksheet.Cells[row, 10].Value = pro.TypeOfQuestion;
                        worksheet.Cells[row, 11].Value = pro.ExamMode;
                        worksheet.Cells[row, 12].Value = pro.ExamAnswerToShow;
                        worksheet.Cells[row, 13].Value = pro.ExamTimeType;
                        worksheet.Cells[row, 14].Value = pro.Stem;
                        worksheet.Cells[row, 15].Value = pro.QuestionId;
                        worksheet.Cells[row, 16].Value = pro.BCSCSectionTitle;
                        worksheet.Cells[row, 17].Value = pro.ChapterName;
                        worksheet.Cells[row, 18].Value = pro.TopicName;
                        worksheet.Cells[row, 19].Value = pro.ChallengeModeTypedAnswer;
                        worksheet.Cells[row, 20].Value = pro.ChallengeModeAnswerText;
                        worksheet.Cells[row, 21].Value = pro.UserSelectedChoice;
                        worksheet.Cells[row, 22].Value = pro.UserAnsweredcorrectly;
                        worksheet.Cells[row, 23].Value = pro.UserAnsweredincorrectly;
                        worksheet.Cells[row, 24].Value = pro.UsersRepeatedSRMode;
                        worksheet.Cells[row, 25].Value = pro.Percentagecorrectly;
                        worksheet.Cells[row, 26].Value = pro.PercentageIncorrectly;
                        worksheet.Cells[row, 27].Value = pro.CorrectAnswer;
                        worksheet.Cells[row, 28].Value = pro.Distractor_1;
                        worksheet.Cells[row, 39].Value = pro.User_chose_distractor_1;
                        worksheet.Cells[row, 30].Value = pro.Distractor_2;
                        worksheet.Cells[row, 31].Value = pro.User_chose_distractor_2;
                        worksheet.Cells[row, 32].Value = pro.Distractor_3;
                        worksheet.Cells[row, 33].Value = pro.User_chose_distractor_3;
                        if (counter > 300000)
                        {
                            log.Information(" Excel Export added sheet no {0}", sheetNumber);
                            sheetNumber++;
                            counter = 0;
                            row = 2;
                            worksheet = GetWorkSheet(package, sheetNumber);
                        }

                        row++;
                    }

                    package.Save();

                    using (MemoryStream stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        //log.Information(" Excel Generation completed for startDate:{0} completedDate:{1}", examStartDate, examCompletedDate);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ALLQuestionPerformance.xlsx");
                    }
                }
                // return " Customer list has been exported successfully";

                /*.xlsx export ends */
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public ExcelWorksheet GetWorkSheet(ExcelPackage package, int sheetNumber)
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("QuestionReport"+ sheetNumber);

            worksheet.Cells[1, 1].Value = "Email";
            worksheet.Cells[1, 2].Value = "Master Customer ID";
            worksheet.Cells[1, 3].Value = "User Name";
            worksheet.Cells[1, 4].Value = "Opt In Status";

            worksheet.Cells[1, 5].Value = "Exam Id";
            worksheet.Cells[1, 6].Value = "Exam StartDate";
            worksheet.Cells[1, 7].Value = "Exam Type";
            worksheet.Cells[1, 8].Value = "Selected Subspeciality";
            worksheet.Cells[1, 9].Value = "No Of Questions";
            worksheet.Cells[1, 10].Value = "Type Of Question";
            worksheet.Cells[1, 11].Value = "Exam Mode";
            worksheet.Cells[1, 12].Value = "Show Correct Answers";
            worksheet.Cells[1, 13].Value = "Exam Time Type";
            worksheet.Cells[1, 14].Value = "Question Stem";
            worksheet.Cells[1, 15].Value = "Question Id";
            worksheet.Cells[1, 16].Value = "BCSC Section Title";
            worksheet.Cells[1, 17].Value = "Chapter Name";
            worksheet.Cells[1, 18].Value = "Topic Name";
            worksheet.Cells[1, 19].Value = "Challenged Mode Typed Answer";
            worksheet.Cells[1, 20].Value = "Challenged Mode Answer Text";
            worksheet.Cells[1, 21].Value = "User Selected Choice";

            worksheet.Cells[1, 22].Value = "# users answered correctly ";
            worksheet.Cells[1, 23].Value = "# users answered Incorrectly ";
            worksheet.Cells[1, 24].Value = "# users repeated at least once in SR mode ";
            worksheet.Cells[1, 25].Value = " % users answered correctly";
            worksheet.Cells[1, 26].Value = "% users answered Incorrectly";
            worksheet.Cells[1, 27].Value = "Correct answer";
            worksheet.Cells[1, 28].Value = "Distractor 1";
            worksheet.Cells[1, 29].Value = "# who chose Distractor 1";
            worksheet.Cells[1, 30].Value = "Distractor 2";
            worksheet.Cells[1, 31].Value = "# who chose Distractor 2 ";
            worksheet.Cells[1, 32].Value = "Distractor 3";
            worksheet.Cells[1, 33].Value = "# who chose Distractor 3";
            return worksheet;
        }
    }
}