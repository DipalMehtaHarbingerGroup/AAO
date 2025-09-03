namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using HtmlAgilityPack;
    using System.Web.Configuration;
    using System.Web;

    public class BCSCMapperController : BaseController
    {
        // GET: BCSCMapper
        public ActionResult Index(int pageIndex = 1, int noOfRecords = 25)
        {
            QuestionContentDTO quesDTO = new QuestionContentDTO();
            quesDTO.PageIndex = pageIndex;
            quesDTO.NoOfRecords = noOfRecords;
            string examPostDataJson = JsonConvert.SerializeObject(quesDTO);
            var data = System.Text.Encoding.UTF8.GetBytes(examPostDataJson);
            string urlGetExamIdBYUserId = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetQuestions";
            string result_GetListOfQuestions = HttpProxy.HttpPost(urlGetExamIdBYUserId, examPostDataJson, "application/json; charset=utf-8", "POST");
            List<QuestionContentDTO> questions = new List<QuestionContentDTO>();
            questions = JsonConvert.DeserializeObject<List<QuestionContentDTO>>(result_GetListOfQuestions);
            ViewBag.questionCount = questions.Count > 0 ? questions[0].RecordCount : 0;
            ViewBag.viewCurrentRecordcount = questions.Count;
            ViewBag.pageIndex = pageIndex;
            ViewBag.noOfRecords = noOfRecords;
            ViewBag.Questions = questions;
            return View();
        }

        public ActionResult GetQuestionContent(int? questionId)
        {
            QuestionContentDTO questionDTO = new QuestionContentDTO();
            questionDTO.QuestionId = Convert.ToInt32(questionId);
            string questionIdJson = JsonConvert.SerializeObject(questionDTO);
            var data = Encoding.UTF8.GetBytes(questionIdJson);
            string urlGetContent = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetQuestionContent";
            string result_GetContent = HttpProxy.HttpPost(urlGetContent, questionIdJson, "application/json; charset=utf-8", "POST");
            List<QuestionContentDTO> content = new List<QuestionContentDTO>();
            content = JsonConvert.DeserializeObject<List<QuestionContentDTO>>(result_GetContent);
            ViewBag.Content = content;
            return Json(content, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetChapter(int? subSpecialityId)
        {
            SubSpecialityDTO specialityDTO = new SubSpecialityDTO();
            specialityDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            string subSpecialityIDJson = JsonConvert.SerializeObject(specialityDTO);
            var data = Encoding.UTF8.GetBytes(subSpecialityIDJson);
            string urlGetChapterId = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetChapter";
            string result_GetListOfChapter = HttpProxy.HttpPost(urlGetChapterId, subSpecialityIDJson, "application/json; charset=utf-8", "POST");
            List<ChapterDTO> chapter = new List<ChapterDTO>();
            chapter = JsonConvert.DeserializeObject<List<ChapterDTO>>(result_GetListOfChapter);
            return Json(chapter, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetButtonStatus(int? questionId, int? subSpecialityId, int? topicId)
        {
            QuestionContentDTO questionDTO = new QuestionContentDTO();
            questionDTO.QuestionId = Convert.ToInt32(questionId);
            questionDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            questionDTO.TopicId = Convert.ToInt32(topicId);
            string questionIdJson = JsonConvert.SerializeObject(questionDTO);
            var data = Encoding.UTF8.GetBytes(questionIdJson);
            string urlGetContent = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetButtonStatus";
            string result_GetContent = HttpProxy.HttpPost(urlGetContent, questionIdJson, "application/json; charset=utf-8", "POST");
            bool buttonStatus;
            buttonStatus = JsonConvert.DeserializeObject<bool>(result_GetContent);
            return Json(buttonStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUpdateContent(int? questionId, int? subSpecialityId, int? topicId)
        {
            TopicContentDTO questionDTO = new TopicContentDTO();
            questionDTO.QuestionId = Convert.ToInt32(questionId);
            questionDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            questionDTO.TopicId = Convert.ToInt32(topicId);
            string updateContentJson = JsonConvert.SerializeObject(questionDTO);
            var data = Encoding.UTF8.GetBytes(updateContentJson);
            string urlGetContent = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetUpdateContent";
            string result_GetListOfTopic = HttpProxy.HttpPost(urlGetContent, updateContentJson, "application/json; charset=utf-8", "POST");
            List<TopicContentDTO> content = new List<TopicContentDTO>();
            content = JsonConvert.DeserializeObject<List<TopicContentDTO>>(result_GetListOfTopic);

            var htmlDoc = new HtmlDocument();

            string html = content[0].Content.ToString();
            htmlDoc.LoadHtml(html);

            /*if (WebConfigurationManager.AppSettings["IsDebugMode"] == "Y")
            {
                if (html != null && html != "")
                {
                    foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                    {
                        var src = node.Attributes[@"src"].Value;
                        if (src.StartsWith("/BCSC"))
                        {
                            node.SetAttributeValue("src", WebConfigurationManager.AppSettings["IISHOSTURL"] + src);
                        }
                    }

                    var newHtml = htmlDoc.DocumentNode.WriteTo();
                    content[0].Content = newHtml;
                }
            }*/

            return Json(content, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertContent(int? questionId, int? subSpecialityId, string chapterName, int? topicId, string contentData)
        {
            TopicContentDTO topicContentDTO = new TopicContentDTO();
            topicContentDTO.QuestionId = Convert.ToInt32(questionId);
            topicContentDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            topicContentDTO.Chapter = Convert.ToString(chapterName);
            topicContentDTO.TopicId = Convert.ToInt32(topicId);
            topicContentDTO.Content = Convert.ToString(WebUtility.UrlDecode(contentData));
            string topicContentIdJson = JsonConvert.SerializeObject(topicContentDTO);
            var data = Encoding.UTF8.GetBytes(topicContentIdJson);
            string urlGetContent = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/InsertContent";
            string result_AddTopicContent = HttpProxy.HttpPost(urlGetContent, topicContentIdJson, "application/json; charset=utf-8", "POST");
            int saveStatus;
            saveStatus = JsonConvert.DeserializeObject<int>(result_AddTopicContent);
            return Json(saveStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuestionById(int? questionId)
        {
            QuestionContentDTO questionDTO = new QuestionContentDTO();
            questionDTO.QuestionId = Convert.ToInt32(questionId);
            string questionIdJson = JsonConvert.SerializeObject(questionDTO);
            var data = Encoding.UTF8.GetBytes(questionIdJson);
            string urlGetQuestions = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetQuestionById";
            string result_GetQuestions = HttpProxy.HttpPost(urlGetQuestions, questionIdJson, "application/json; charset=utf-8", "POST");
            List<QuestionContentDTO> questions = new List<QuestionContentDTO>();
            questions = JsonConvert.DeserializeObject<List<QuestionContentDTO>>(result_GetQuestions);
            ViewBag.questionId = questionId;
            ViewBag.Questions = questions;
            return View("Index");
        }

        public ActionResult GetQuestionByTitle(string questionText, int pageIndex, int noOfRecords, string radioButtonState)
        {
            QuestionContentDTO questionContent = new QuestionContentDTO();
            questionContent.Stem = Convert.ToString(questionText);
            questionContent.PageIndex = Convert.ToInt32(pageIndex);
            questionContent.NoOfRecords = Convert.ToInt32(noOfRecords);
            questionContent.radioButtonState = Convert.ToString(radioButtonState);
            string questionTextJson = JsonConvert.SerializeObject(questionContent);
            var data = Encoding.UTF8.GetBytes(questionTextJson);
            string urlGetQuestions = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetQuestionByTitle";
            string result_GetQuestions = HttpProxy.HttpPost(urlGetQuestions, questionTextJson, "application/json; charset=utf-8", "POST");
            List<QuestionContentDTO> questions = new List<QuestionContentDTO>();
            questions = JsonConvert.DeserializeObject<List<QuestionContentDTO>>(result_GetQuestions);

            ViewBag.questionCount = questions.Count > 0 ? questions[0].RecordCount : 0;
            ViewBag.questionText = questionText;
            ViewBag.noOfRecords = noOfRecords;
            ViewBag.pageIndex = pageIndex;
            ViewBag.radioButtonState = radioButtonState;
            ViewBag.Questions = questions;
            return View("Index");
        }

        public ActionResult GetProgram()
        {
            string urlGetProgramId = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetProgram";
            string result_GetListOfProgram = HttpProxy.HttpGET(urlGetProgramId, "application/json; charset=utf-8", "GET");
            List<ProgramDTO> program = new List<ProgramDTO>();
            program = JsonConvert.DeserializeObject<List<ProgramDTO>>(result_GetListOfProgram);
            return Json(program, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubProgram(int? programId)
        {
            ProgramDTO programDTO = new ProgramDTO();
            programDTO.programId = Convert.ToInt32(programId);
            string programIdJson = JsonConvert.SerializeObject(programDTO);
            var data = Encoding.UTF8.GetBytes(programIdJson);
            string urlGetSubProgram = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetSubProgram";
            string result_GetSubProgram = HttpProxy.HttpPost(urlGetSubProgram, programIdJson, "application/json; charset=utf-8", "POST");
            List<ProgramDTO> subPrograms = new List<ProgramDTO>();
            subPrograms = JsonConvert.DeserializeObject<List<ProgramDTO>>(result_GetSubProgram);
            return Json(subPrograms, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubSpeciality(int? subProgramId)
        {
            ProgramDTO subProgramDTO = new ProgramDTO();
            subProgramDTO.subprogramId = Convert.ToInt32(subProgramId);
            string subSpecialityIdJson = JsonConvert.SerializeObject(subProgramDTO);
            var data = Encoding.UTF8.GetBytes(subSpecialityIdJson);
            string urlGetSubSpeciality = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetSubSpeciality";
            string result_GetSubSpeciality = HttpProxy.HttpPost(urlGetSubSpeciality, subSpecialityIdJson, "application/json; charset=utf-8", "POST");
            List<ProgramDTO> subSpeciality = new List<ProgramDTO>();
            subSpeciality = JsonConvert.DeserializeObject<List<ProgramDTO>>(result_GetSubSpeciality);
            return Json(subSpeciality, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopicBySubSpec(int? subSpecialityId)
        {
            ProgramDTO subSpecialityDTO = new ProgramDTO();
            subSpecialityDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            string subTopicIdJson = JsonConvert.SerializeObject(subSpecialityDTO);
            var data = Encoding.UTF8.GetBytes(subTopicIdJson);
            string urlGetTopic = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetTopicBySubSpec";
            string result_GetTopic = HttpProxy.HttpPost(urlGetTopic, subTopicIdJson, "application/json; charset=utf-8", "POST");
            List<ProgramDTO> subTopic = new List<ProgramDTO>();
            subTopic = JsonConvert.DeserializeObject<List<ProgramDTO>>(result_GetTopic);
            return Json(subTopic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetQuesByTopic(int? topicId, int? subSpecialtyId, int? programId, int? subProgramId, int pageIndex = 1, int noOfRecords = 25)
        {
            QuestionContentDTO questionDTO = new QuestionContentDTO();
            questionDTO.TopicId = Convert.ToInt32(topicId);
            questionDTO.SubspecialtyId = Convert.ToInt32(subSpecialtyId);
            questionDTO.programId = Convert.ToInt32(programId);
            questionDTO.SubprogramId = Convert.ToInt32(subProgramId);
            questionDTO.PageIndex = Convert.ToInt32(pageIndex);
            questionDTO.NoOfRecords = Convert.ToInt32(noOfRecords);
            string topicIdJson = JsonConvert.SerializeObject(questionDTO);
            var data = Encoding.UTF8.GetBytes(topicIdJson);
            string urlGetQuestions = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetQuesByTopic";
            string result_GetQuestions = HttpProxy.HttpPost(urlGetQuestions, topicIdJson, "application/json; charset=utf-8", "POST");
            List<QuestionContentDTO> questionsval = new List<QuestionContentDTO>();
            questionsval = JsonConvert.DeserializeObject<List<QuestionContentDTO>>(result_GetQuestions);
            ViewBag.ProgramId = programId;
            ViewBag.SubprogramId = subProgramId;
            ViewBag.TopicId = topicId;
            ViewBag.SubSpeciality = subSpecialtyId;
            ViewBag.questionCount = questionsval.Count > 0 ? questionsval[0].RecordCount : 0;
            ViewBag.pageIndex = pageIndex;
            ViewBag.noOfRecords = noOfRecords;
            ViewBag.Questions = questionsval;
            return View("Index");
        }

        public ActionResult GetBCSCSectionNumber(int subSpecialityId)
        {
            SubSpecialityDTO subSpecDTO = new SubSpecialityDTO();
            subSpecDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            string subTopicIdJson = JsonConvert.SerializeObject(subSpecDTO);
            var data = Encoding.UTF8.GetBytes(subTopicIdJson);
            string urlSerNum = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetBCSCSectionNumber";
            string result_GetTopic = HttpProxy.HttpPost(urlSerNum, subTopicIdJson, "application/json; charset=utf-8", "POST");
            List<SubSpecialityDTO> secSerialNum = new List<SubSpecialityDTO>();
            secSerialNum = JsonConvert.DeserializeObject<List<SubSpecialityDTO>>(result_GetTopic);
            return Json(secSerialNum, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHtmlPage(string chapterName, int bcscSectionNumber)
        {
            var fileName = string.Empty;
            var rData = string.Empty;
            DirectoryInfo directory;
            List<System.IO.FileInfo> files = new List<System.IO.FileInfo>();

            int fileCount;
            if (WebConfigurationManager.AppSettings["IsDebugMode"] == "Y")
            {
                directory = new DirectoryInfo(WebConfigurationManager.AppSettings["BCSCBooksDirectory"] + bcscSectionNumber + string.Empty);
                files = directory.GetFiles().ToList();
                fileCount = Directory.GetFiles(WebConfigurationManager.AppSettings["BCSCBooksDirectory"] + bcscSectionNumber + string.Empty, ".", SearchOption.TopDirectoryOnly).Length;

            }
            else
            {
                var path = @"/BCSC Book/" + bcscSectionNumber + "";
                directory = new DirectoryInfo(Server.MapPath(path));
                files = directory.GetFiles().ToList();
                fileCount = Directory.GetFiles(Server.MapPath(path), ".", SearchOption.TopDirectoryOnly).Length;
            }

            chapterName = chapterName.Trim();
            foreach (var fullPath in files)
            {
                fileName = Path.GetFileName(fullPath.FullName);
                string line;
                string chapterName1 = "class=\"chapter-title\"";
                string chapterName2 = "class=\"chapter-title";
                fileName = Path.GetFileName(fullPath.FullName);

                // Read the file and display it line by line.
                int line_Number = 0;
                System.IO.StreamReader file1 = new System.IO.StreamReader(directory + "\\" + fileName);
                while ((line = file1.ReadLine()) != null)
                {
                    line_Number++;
                    //if (line_Number >= 55)
                    //{
                    //    break;
                    //}

                    if ((line.Contains(chapterName1)) || (line.Contains(chapterName2)))
                    {
                        string bookLine = line;
                        bookLine = Regex.Replace(bookLine, @"&#160;|&#173;", "");
                        bookLine = Regex.Replace(bookLine, @"\t|\n|\r|\s|<br />", "");
                        chapterName = Regex.Replace(chapterName, @"\t|\n|\r|\s", "");
                        int pos1 = bookLine.IndexOf(">") + ">".Length;
                        int pos2 = bookLine.LastIndexOf("<");
                        bookLine = bookLine.Substring(pos1);
                        bookLine = bookLine.Replace("</p>", "");
                        if (bookLine == chapterName)
                        {
                            rData = System.IO.File.ReadAllText(directory + "\\" + fileName);
                            file1.Close();
                            string html = rData;
                            var htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(html);

                            /*if (WebConfigurationManager.AppSettings["IsDebugMode"] == "Y")
                            {
                                foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                {
                                    var src = node.Attributes[@"src"].Value;
                                    if (src.StartsWith("BCSC"))
                                    {
                                        node.SetAttributeValue("src", @"http://localhost/BCSC Book/" + bcscSectionNumber + "/" + src);
                                    }
                                }

                            }
                            else
                            {
                                foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                                {
                                    var src = node.Attributes[@"src"].Value;
                                    if (src.StartsWith("BCSC"))
                                    {
                                        node.SetAttributeValue("src", @"/BCSC Book/" + bcscSectionNumber + "/" + src);
                                    }
                                }
                            }
                            */
                            foreach (var node in htmlDoc.DocumentNode.SelectNodes("//img"))
                            {
                                var src = node.Attributes[@"src"].Value;
                                if (src.StartsWith("BCSC"))
                                {
                                    node.SetAttributeValue("src", @"/BCSC Book/" + bcscSectionNumber + "/" + src);
                                }
                            }

                            var newHtml = htmlDoc.DocumentNode.WriteTo();

                            //var htmlDocCSS = new HtmlDocument();
                            //htmlDocCSS.LoadHtml(newHtml);
                            //foreach (var node in htmlDocCSS.DocumentNode.SelectNodes("//link"))
                            // {
                            //    var src = node.Attributes[@"href"].Value;
                            //    if (src.StartsWith("BCSC"))
                            //    {
                            //        node.SetAttributeValue("href", "/BCSC Book/" + bcscSectionNumber + "/" + src);
                            //    }
                            //}

                            //var newHtml_Update = htmlDocCSS.DocumentNode.WriteTo();

                            return Json(newHtml, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                        }
                    }
                }

                file1.Close();
            }

            return Json(rData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBCSCQuestDetails(int? questionId, int? subSpecialityId)
        {
            QuestionData questionDTO = new QuestionData();
            questionDTO.QuestionId = Convert.ToInt32(questionId);
            questionDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            string questionIdJson = JsonConvert.SerializeObject(questionDTO);
            var data = Encoding.UTF8.GetBytes(questionIdJson);
            string urlGetContent = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetBCSCQuestDetails";
            string result_GetContent = HttpProxy.HttpPost(urlGetContent, questionIdJson, "application/json; charset=utf-8", "POST");
            List<QuestionData> questionDetails = new List<QuestionData>();
            questionDetails = JsonConvert.DeserializeObject<List<QuestionData>>(result_GetContent);
            ViewBag.Content = questionDetails;
            return Json(questionDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubSpecFromSpecialityId(int? subSpecialityId)
        {
            ProgramDTO programDTO = new ProgramDTO();
            programDTO.SubspecialtyId = Convert.ToInt32(subSpecialityId);
            string subTopicIdJson = JsonConvert.SerializeObject(programDTO);
            var data = Encoding.UTF8.GetBytes(subTopicIdJson);
            string urlGetTopic = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetSubSpecFromSpecialityId";
            string result_GetTopic = HttpProxy.HttpPost(urlGetTopic, subTopicIdJson, "application/json; charset=utf-8", "POST");
            List<ProgramDTO> subTopic = new List<ProgramDTO>();
            subTopic = JsonConvert.DeserializeObject<List<ProgramDTO>>(result_GetTopic);
            return Json(subTopic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTopicByTopicId(int? topicId)
        {
            ProgramDTO programDTO = new ProgramDTO();
            programDTO.TopicId = Convert.ToInt32(topicId);
            string topicIdJson = JsonConvert.SerializeObject(programDTO);
            var data = Encoding.UTF8.GetBytes(topicIdJson);
            string urlGetTopic = AAOGlobalConstants.SiteWebAPIUrl + "BCSCMapper/GetTopicByTopicId";
            string result_GetTopic = HttpProxy.HttpPost(urlGetTopic, topicIdJson, "application/json; charset=utf-8", "POST");
            List<ProgramDTO> topic = new List<ProgramDTO>();
            topic = JsonConvert.DeserializeObject<List<ProgramDTO>>(result_GetTopic);
            return Json(topic, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Upload(HttpPostedFileBase file)
        {
            string path;
            string saveloc = WebConfigurationManager.AppSettings["BCSCBooksDirectory"] + @"uploads\";
            string relativeloc = "/BCSC Book/uploads/";
            string fileName = Path.GetFileNameWithoutExtension(file.FileName) + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string extention = Path.GetExtension(file.FileName);
            string filename = fileName + extention;

            if (file != null && file.ContentLength > 0 && file.ContentType.StartsWith("image/"))
            {
                try
                {
                    path = Path.Combine(saveloc, Path.GetFileName(filename));
                    file.SaveAs(path);
                }
                catch (Exception e)
                {
                    return "<script>alert('Failed: " + e + "');</script>";
                }
            }
            else
            {
                return "<script>alert('Failed: Unkown Error. This form only accepts valid images.');</script>";
            }

            return "<script>top.$('.mce-btn.mce-open').parent().find('.mce-textbox').val('" + relativeloc + filename + "').closest('.mce-window').find('.mce-primary').click();</script>";
        }
    }
}