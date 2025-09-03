namespace AAO.Apps.BCSCSelfAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using Newtonsoft.Json;
    using System.Configuration;
    using Serilog;
    using HtmlAgilityPack;
    using iTextSharp.text;
    using iTextSharp.tool.xml;
    using iTextSharp.tool.xml.pipeline.css;
    using System.Data.SqlClient;
    using System.IO;

    using static AAO.DTO.BCSCSelfAssessment.ExamDTO;
    using System.Web;

    public class ExamManagerController : BaseResidentController
    {
        private string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private Serilog.Core.Logger log;

        // GET: ExamManager
        public ExamManagerController()
        {
            log = new LoggerConfiguration().WriteTo.RollingFile(this._sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
        }

        public ActionResult Index()
        {
            try
            {
                ModelState.Clear();
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userid:{2}", "ExamManagerController", "Index", user._userId);
                var enumExamTypeData = from ExamManagerEnum.ExamType e in Enum.GetValues(typeof(ExamManagerEnum.ExamType))
                                       select new
                                       {
                                           ID = (int)e,
                                           Name = e.ToString(),
                                       };
                var enumExamModeData = from ExamManagerEnum.ExamMode e in Enum.GetValues(typeof(ExamManagerEnum.ExamMode))
                                       select new
                                       {
                                           ID = (int)e,
                                           Name = e.ToString(),
                                       };
                var enumExamSelectionType = from ExamManagerEnum.ExamQuestionSelectionType e in Enum.GetValues(typeof(ExamManagerEnum.ExamQuestionSelectionType))
                                            select new
                                            {
                                                ID = (int)e,
                                                Name = e.ToString(),
                                            };

                // Exam Type List
                List<SelectListItem> examType = new List<SelectListItem>();
                foreach (var enumExamSelectionTypeView in enumExamTypeData)
                {
                    var res = (ExamManagerEnum.ExamType)Enum.Parse(typeof(ExamManagerEnum.ExamType), enumExamSelectionTypeView.Name.ToString());
                    string value = GetEnumDescription(res);
                    examType.Add(new SelectListItem() { Text = value, Value = enumExamSelectionTypeView.ID.ToString(), Selected = false });
                }

                examType.RemoveAll(x => x.Value == "3");

                // Create ExamDTO and populate with TempData values if available
                ExamDTO exam = new ExamDTO();
                exam.UserId = user._userId;

                bool preserveFormData = TempData["PreserveFormData"] as bool? ?? false;
                bool hasTempData = TempData["ExamType"] != null || !string.IsNullOrEmpty(TempData["ExamName"]?.ToString());

                log.Information("Debug - PreserveFormData: {0}, HasTempData: {1}", preserveFormData, hasTempData);

                int? selectedExamType = null;
                if (TempData.ContainsKey("SelectedExamType"))
                {
                    selectedExamType = TempData["SelectedExamType"] as int?;
                    log.Information("Debug - SelectedExamType from TempData: {0}", selectedExamType);
                }

                // Use selectedExamType if available, otherwise use TempData or default
                if (selectedExamType.HasValue)
                {
                    exam.ExamType = selectedExamType.Value;
                }
                else
                {
                    exam.ExamType = TempData["ExamType"] != null ? (int)TempData["ExamType"] : 0;
                }

                // Only populate from TempData if PreserveFormData is true
                if (preserveFormData && hasTempData)
                {
                    log.Information("Debug - Populating form from TempData");

                    // Populate all form fields from TempData
                    exam.SearchTerm = TempData["SearchTerm"]?.ToString() ?? "";
                    exam.ExamName = TempData["ExamName"]?.ToString() ?? "";
                    exam.NoofQuestions = TempData["NoofQuestions"] != null ? (int)TempData["NoofQuestions"] : 0;
                    exam.NumberOfCustomisedQuestions = TempData["NumberOfCustomisedQuestions"] != null ? (int)TempData["NumberOfCustomisedQuestions"] : 0;
                    exam.EmailId = TempData["EmailId"]?.ToString() ?? "";
                    exam.ExamMode = TempData["ExamMode"] != null ? (bool)TempData["ExamMode"] : false;
                    exam.ExamAnswerToShow = TempData["ExamAnswerToShow"] != null ? (bool)TempData["ExamAnswerToShow"] : false;
                    exam.ExamTimeType = TempData["ExamTimeType"] != null ? (bool)TempData["ExamTimeType"] : false;
                    exam.ShowDetailedAnswers = TempData["ShowDetailedAnswers"] != null ? (bool)TempData["ShowDetailedAnswers"] : false;
                    exam.ShowAnsAfterExamCompletion = TempData["ShowAnsAfterExamCompletion"] != null ? (bool)TempData["ShowAnsAfterExamCompletion"] : false;
                    exam.DateRangeEnabled = TempData["DateRangeEnabled"] != null ? (bool)TempData["DateRangeEnabled"] : false;
                    exam.StartDate = TempData["StartDate"] as DateTime?;
                    exam.EndDate = TempData["EndDate"] as DateTime?;
                    string selectedQuestionsString = TempData["SelectedQuestions"]?.ToString() ?? "";
                    if (!string.IsNullOrEmpty(selectedQuestionsString))
                    {
                        try
                        {
                            // Parse the comma-separated string into List<int>
                            exam.SelectedQuestionIds = selectedQuestionsString.Split(',')
                                                                            .Where(x => !string.IsNullOrEmpty(x.Trim()))
                                                                            .Select(x => int.Parse(x.Trim()))
                                                                            .ToList();

                            log.Information("Debug - Restored {0} selected questions from TempData: {1}", exam.SelectedQuestionIds.Count, selectedQuestionsString);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error parsing SelectedQuestions from TempData: {0}", ex.Message);
                            exam.SelectedQuestionIds = new List<int>();
                        }
                    }
                    else
                    {
                        exam.SelectedQuestionIds = new List<int>();
                    }

                    exam.SelectedQuestions = selectedQuestionsString;

                    // Handle array fields - TypeofCategoryList (SelectedSections)
                    string[] selectedSections = null;
                    if (TempData["SelectedSections"] != null)
                    {
                        selectedSections = TempData["SelectedSections"] as string[];
                    }
                    else if (TempData["TypeofCategoryList"] != null)
                    {
                        selectedSections = TempData["TypeofCategoryList"] as string[];
                    }

                    exam.TypeofCategoryList = selectedSections?.ToList() ?? new List<string>();

                    // Handle array fields - TypeofQuestionList (SelectedQuestionTypes)
                    string[] selectedQuestionTypes = null;
                    if (TempData["SelectedQuestionTypes"] != null)
                    {
                        selectedQuestionTypes = TempData["SelectedQuestionTypes"] as string[];
                    }
                    else if (TempData["TypeofQuestionList"] != null)
                    {
                        selectedQuestionTypes = TempData["TypeofQuestionList"] as string[];
                    }

                    exam.TypeofQuestionList = selectedQuestionTypes ?? new string[0];
                }
                else
                {
                    log.Information("Debug - Using default values for new form");

                    // Set appropriate default values based on exam type
                    exam.SearchTerm = "";
                    exam.ExamName = "";
                    exam.NoofQuestions = 0;
                    exam.NumberOfCustomisedQuestions = 0;
                    exam.EmailId = "";
                    exam.ExamMode = false;
                    exam.ExamTimeType = false;
                    exam.ShowDetailedAnswers = false;
                    exam.ShowAnsAfterExamCompletion = false;
                    exam.DateRangeEnabled = false;
                    exam.StartDate = null;
                    exam.EndDate = null;
                    exam.ExamAnswerToShow = true;
                    exam.TypeofCategoryList = new List<string>();
                    exam.TypeofQuestionList = new string[0];
                    exam.SelectedQuestionIds = new List<int>();
                    exam.SelectedQuestions = "";
                    exam.QuestionListData = "";

                    log.Information("Debug - Default ExamAnswerToShow for ExamType {0}: {1}", exam.ExamType, exam.ExamAnswerToShow);
                }

                // Get Question Type Count List
                string urlGetQuestionTypeCount = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/GetQuestionTypeCount";
                string questuionTypeDataJson = JsonConvert.SerializeObject(exam);
                string resultGetQuestionTypeCount = HttpProxy.HttpPost(urlGetQuestionTypeCount, questuionTypeDataJson, "application/json; charset=utf-8", "POST");
                List<QuestionTypeCountDTO> questionTypeListCount = new List<QuestionTypeCountDTO>();
                questionTypeListCount = JsonConvert.DeserializeObject<List<QuestionTypeCountDTO>>(resultGetQuestionTypeCount);
                string[] questionCount = new string[4];
                foreach (var item in questionTypeListCount)
                {
                    questionCount[0] = item.TotalCount.ToString();
                    questionCount[1] = item.ExamSkipQuestionCount.ToString();
                    questionCount[2] = item.IncorrectAnswerCount.ToString();
                    questionCount[3] = item.MarkQuestionCount.ToString();
                }

                List<SelectListItem> questionType = new List<SelectListItem>();
                foreach (var enumExamSelectionTypeView in enumExamSelectionType)
                {
                    var res = (ExamManagerEnum.ExamQuestionSelectionType)Enum.Parse(typeof(ExamManagerEnum.ExamQuestionSelectionType), enumExamSelectionTypeView.Name.ToString());
                    string value = GetEnumDescription(res);
                    questionType.Add(new SelectListItem() { Text = value, Value = enumExamSelectionTypeView.ID.ToString(), Selected = false });
                }

                // Get SubSpeciality
                string url = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/GetSpecialityList";
                string result = HttpProxy.HttpPost(url, questuionTypeDataJson, "application/json; charset=utf-8", "POST");
                List<SubSpecialityDetailVM> specialityType = new List<SubSpecialityDetailVM>();
                specialityType = JsonConvert.DeserializeObject<List<SubSpecialityDetailVM>>(result);
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

                ViewBag.ExamType = examType;
                ViewBag.EnumExamModeList = new SelectList(enumExamModeData, "ID", "Name");
                ViewBag.QuestionTypeListCount = questionCount;
                ViewBag.QuestionTypelist = questionType;
                ViewBag.CategoryTypelist = categoryType;
                ViewBag.EnumExamList = new SelectList(examType, "Value", "Text", exam.ExamType); // Set selected exam type
                ViewBag.SelectedExamType = selectedExamType ?? exam.ExamType;
                ViewBag.ModelExamType = exam.ExamType;
                ViewBag.SelectedSections = exam.TypeofCategoryList;
                ViewBag.SelectedQuestionTypes = exam.TypeofQuestionList;
                ViewBag.PreserveFormData = preserveFormData;
                ViewBag.SelectedQuestionsString = exam.SelectedQuestions ?? "";

                log.Information("Debug - Final ViewBag.PreserveFormData: {0}, ExamAnswerToShow: {1}", ViewBag.PreserveFormData, exam.ExamAnswerToShow);
                log.Information("end Controller :{0} method :{1} userid:{2}", "ExamManagerController", "Index", user._userId);
                // Return the populated exam model to the view
                return View(exam);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + " Controller :{0} method :{1}", "ExamManagerController", "Index");
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitSelectedQuestions(string selectedQuestions, int numberOfQuestions, string examName, int examType = 4, string emailId = "", bool showDetailedAnswers = false, bool showAnsAfterExamCompletion = false, bool dateRangeEnabled = false, DateTime? startDate = null, DateTime? endDate = null, int noofQuestions = 0, bool examAnswerToShow = true, string typeofCategoryList = "", string typeofQuestionList = "", string searchTerm = "")
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userid:{2}", "ExamManagerController", "SubmitSelectedQuestions", user._userId);

                // Parse the selected questions
                var questionIds = new List<int>();
                if (!string.IsNullOrEmpty(selectedQuestions))
                {
                    questionIds = selectedQuestions.Split(',')
                                                 .Where(x => !string.IsNullOrEmpty(x))
                                                 .Select(x => int.Parse(x.Trim()))
                                                 .ToList();
                }

                if (questionIds.Count == 0)
                {
                    TempData["ErrorMessage"] = "Please select at least one question.";
                    return RedirectToAction("Index");
                }

                // Store all the data in TempData to persist across the redirect
                TempData["SearchTerm"] = searchTerm ?? "";
                TempData["ExamName"] = examName;
                TempData["ExamType"] = examType;
                TempData["NumberOfCustomisedQuestions"] = numberOfQuestions;
                TempData["EmailId"] = emailId;
                TempData["ShowDetailedAnswers"] = showDetailedAnswers;
                TempData["ShowAnsAfterExamCompletion"] = showAnsAfterExamCompletion;
                TempData["DateRangeEnabled"] = dateRangeEnabled;
                TempData["StartDate"] = startDate;
                TempData["EndDate"] = endDate;
                TempData["NoofQuestions"] = noofQuestions;
                TempData["ExamAnswerToShow"] = examAnswerToShow;
                TempData["SelectedQuestions"] = selectedQuestions;

                string[] selectedSections = !string.IsNullOrEmpty(typeofCategoryList) ? typeofCategoryList.Split(',')
                    .Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray() : new string[0];

                string[] selectedQuestionTypes = !string.IsNullOrEmpty(typeofQuestionList)
                    ? typeofQuestionList.Split(',').Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray() : new string[0];

                // Store selected sections and question types
                TempData["SelectedSections"] = selectedSections ?? new string[0];
                TempData["SelectedQuestionTypes"] = selectedQuestionTypes ?? new string[0];

                // Also store with the original names for backward compatibility
                TempData["TypeofCategoryList"] = selectedSections ?? new string[0];
                TempData["TypeofQuestionList"] = selectedQuestionTypes ?? new string[0];

                // Create QuestionListData structure for API
                var questionItems = questionIds.Select(id => new QuestionItem
                {
                    QuestionId = id,
                    ExamId = 0,
                    ExamAttemptId = 0,
                    QuestionText = "",
                }).ToList();
                TempData["QuestionListData"] = JsonConvert.SerializeObject(questionItems);

                // Set flag to preserve form data
                TempData["PreserveFormData"] = true;
                TempData["MsgSuccess"] = $"Successfully added {numberOfQuestions} questions to the exam.";

                log.Information("Stored search term in TempData: {0}", searchTerm);
                log.Information("Stored selected questions in TempData: {0}", selectedQuestions);
                log.Information("Stored selected sections: {0}", selectedSections != null ? string.Join(",", selectedSections) : "none");
                log.Information("Stored selected question types: {0}", selectedQuestionTypes != null ? string.Join(",", selectedQuestionTypes) : "none");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + " Controller :{0} method :{1}", "ExamManagerController", "SubmitSelectedQuestions");
                TempData["ErrorMessage"] = "An error occurred while processing your request. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateExam(ExamDTO exam)
        {
            try
            {
                if (!string.IsNullOrEmpty(exam.TypeofCategoryListstring))
                {
                    exam.TypeofCategoryList = exam.TypeofCategoryListstring.Split(',').ToList();
                }

                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2} ExamType:{3}", "ExamManagerController", "CreateExam", user._userId, exam.ExamType);

                // Log the original parameters
                log.Information(
                    "ExamName:{0}, ExamId:{1}, ExamAttemptId:{2}, ExamAnswerToShow:{3}, ExamCreateDate:{4}, ExamMode:{5}, NoofQuestions:{6}, TypeofQuestion:{7}, TypeofCategory:{8}, ExamType:{9}, TypeofCategoryListstring:{10}, userId:{11}, StartDate:{12}, EndDate:{13}, EmailId:{14}",
                    exam.ExamName, exam.ExamId, exam.ExamAttemptId, exam.ExamAnswerToShow, exam.ExamCreateDate, exam.ExamMode, exam.NoofQuestions, exam.TypeofQuestion, exam.TypeofCategory, exam.ExamType, exam.TypeofCategoryListstring, user._userId, exam.StartDate, exam.EndDate, exam.EmailId);

                log.Information("CreateExam received - QuestionListData: {0}", exam.QuestionListData ?? "null");

                // ADD THIS: Process selected questions and ensure they're properly formatted for API
                List<int> selectedQuestionIds = new List<int>();

                // Process Question List Data if provided
                if (!string.IsNullOrEmpty(exam.QuestionListData))
                {
                    try
                    {
                        var questionList = JsonConvert.DeserializeObject<List<QuestionItem>>(exam.QuestionListData);
                        log.Information("Question List Data received - Count: {0}, UserId: {1}", questionList.Count, user._userId);

                        foreach (var questionItem in questionList)
                        {
                            log.Information("Selected Question - QuestionId: {0}, ExamId: {1}, ExamAttemptId: {2}, Text: {3}",
                                questionItem.QuestionId, questionItem.ExamId, questionItem.ExamAttemptId, questionItem.QuestionText);
                        }

                        if (questionList.Count > 0)
                        {
                            selectedQuestionIds = questionList.Select(q => q.QuestionId).ToList();
                            log.Information("Custom question selection detected with {0} questions", questionList.Count);

                            // ADD THIS: Update exam properties for selected questions
                            if (exam.ExamType == 4) // Shared exam
                            {
                                exam.NumberOfCustomisedQuestions = questionList.Count;
                                log.Information("Updated exam question counts - NoofQuestions: {0}, NumberOfCustomisedQuestions: {1}",
                                    exam.NoofQuestions, exam.NumberOfCustomisedQuestions);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error processing Question List Data: {0}, UserId: {1}", ex.Message, user._userId);
                        // Continue with normal exam creation if question list processing fails
                    }
                }

                // ADD THIS: Fallback to SelectedQuestions string if QuestionListData is empty
                else if (!string.IsNullOrEmpty(exam.SelectedQuestions))
                {
                    try
                    {
                        selectedQuestionIds = exam.SelectedQuestions.Split(',')
                                            .Where(x => !string.IsNullOrEmpty(x.Trim()))
                                            .Select(x => int.Parse(x.Trim()))
                                            .ToList();

                        log.Information("Parsed {0} questions from SelectedQuestions string: {1}", selectedQuestionIds.Count, exam.SelectedQuestions);

                        // Create QuestionListData for API if not already present
                        var questionItems = selectedQuestionIds.Select(id => new QuestionItem
                        {
                            QuestionId = id,
                            ExamId = 0,
                            ExamAttemptId = 0,
                            QuestionText = ""
                        }).ToList();

                        exam.QuestionListData = JsonConvert.SerializeObject(questionItems);

                        // Update exam properties for selected questions
                        if (exam.ExamType == 4) // Shared exam
                        {
                            exam.NumberOfCustomisedQuestions = selectedQuestionIds.Count;
                            log.Information("Updated exam question counts from SelectedQuestions - NoofQuestions: {0}", exam.NoofQuestions);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error parsing SelectedQuestions string: {0}, UserId: {1}", ex.Message, user._userId);
                    }
                }

                exam.UserId = user._userId;
                log.Information("UserId :{0}", user._userId);
                string url = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/CreateExam";
                if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    //exam.TypeofCategoryList = new List<string>() { "12", "13", "9", "2", "6", "8", "7", "10", "1", "3", "4", "5", "11" };
                    exam.TypeofQuestionList = new List<string>() { "0" };
                    exam.ExamAnswerToShow = true;
                }

                bool isSharedExam = exam.ExamType == 4;

                if (isSharedExam)
                {
                    // Validate date range
                    if (exam.StartDate >= exam.EndDate)
                    {
                        TempData["MsgError"] = "End Date must be after Start Date.";
                        log.Information("Shared Exam validation failed - invalid date range. UserId:{0}", user._userId);
                        return RedirectToAction("Index", "ExamManager");
                    }

                    // Validate start date is not in the past
                    if (exam.StartDate < DateTime.Today)
                    {
                        TempData["MsgError"] = "Start Date cannot be in the past.";
                        log.Information("Shared Exam validation failed - start date in past. UserId:{0}", user._userId);
                        return RedirectToAction("Index", "ExamManager");
                    }

                    log.Information("Creating Shared Exam - StartDate:{0}, EndDate:{1}, EmailId:{2}", exam.StartDate, exam.EndDate, exam.EmailId);
                }

                log.Information("ExamType :{0}", exam.ExamType);

                // ADD THIS: Log what's being sent to API
                if (selectedQuestionIds.Count > 0)
                {
                    log.Information("Sending exam to API with {0} selected questions: [{1}]", selectedQuestionIds.Count, string.Join(", ", selectedQuestionIds));
                }

                string examPostDataJson = JsonConvert.SerializeObject(exam);

                // ADD THIS: Log the JSON being sent (optional - for debugging)
                log.Information("API Request JSON contains QuestionListData: {0}", !string.IsNullOrEmpty(exam.QuestionListData) ? "Yes" : "No");

                string result = HttpProxy.HttpPost(url, examPostDataJson, "application/json; charset=utf-8", "POST");
                ResponseStatusVM examResponse = new ResponseStatusVM();
                examResponse = JsonConvert.DeserializeObject<ResponseStatusVM>(result);

                // Handle response based on exam type
                if (examResponse.ErrorCode == 1)
                {
                    if (examResponse.SystemGeneratedQuestionCount < examResponse.UserEnteredQuestionCount)
                    {
                        log.Information("ErrorCode: {0}", examResponse.ErrorCode + " Controller:ExamManagerController" + " method:CreateExam" + " UserId:" + user._userId);
                        TempData["MsgError"] = examResponse.Errormessage + ", with " + examResponse.SystemGeneratedQuestionCount + " questions , based on the availability of questions for your selected options.";
                    }

                    if (isSharedExam)
                    {
                        // Send email only for shared exams and only after successful creation
                        if (!string.IsNullOrEmpty(exam.EmailId))
                        {
                            SendSharedExamEmail(exam.EmailId, exam.ExamName, exam.StartDate, exam.EndDate);
                            log.Information("Shared exam email sent to: {0}", exam.EmailId);
                        }
                        else
                        {
                            log.Warning("Shared exam created but no email address provided. ExamId:{0}, UserId:{1}", examResponse.ExamId, user._userId);
                        }

                        string successMessage = "Shared exam has been created successfully and will be shared with the specified email.";

                        TempData["MsgSuccess"] = successMessage;
                        log.Information("Shared Exam created successfully. ExamId:{0}, UserId:{1}, EmailId:{2}, SelectedQuestions:{3}", examResponse.ExamId, user._userId, exam.EmailId, selectedQuestionIds.Count);

                        return RedirectToAction("Index", "ExamManager");
                    }
                    else
                    {
                        log.Information("Redirect to Controller: Controller:{0}, Method:{1}, ExamId:{2}, SelectedQuestions:{3}", "Assessment", "Index", examResponse.ExamId, selectedQuestionIds.Count);
                        return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Assessment", action = "Index", ExamId = examResponse.ExamId }));
                    }
                }
                else
                {
                    TempData["MsgError"] = examResponse.Errormessage;
                    log.Information("end Controller :{0} method :{1} userId:{2} ExamType:{3} ExamName:{4}, ExamId:{5}", "ExamManagerController", "CreateExam", user._userId, exam.ExamType, exam.ExamName, exam.ExamId);
                    log.Information("Redirect to Controller :{0} method :{1}", "Dashboard", "Index");
                    return RedirectToAction("Index", "ExamManager");
                }
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:ExamManagerController" + " method:CreateExam" + "UserId:" + user._userId + " ExamId:" + exam.ExamId + " ExamName:" + exam.ExamName);
                throw;
            }
        }

        public static string GetEnumDescription(Enum value)
        {
            try
            {
                FieldInfo fieldinfo = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[])fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return attributes[0].Description;
                }
                else
                {
                    return value.ToString();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult CheckExamNameAvailable(string ExamName)
        {
            try
            {
                log.Information("start Controller :{0} method :{1} ExamName:{2}", "ExamManagerController", "CheckExamNameAvailable", ExamName);
                log.Information("ExamName :{0}", ExamName);
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                ExamNameVM examName = new ExamNameVM
                {
                    ExamName = ExamName,
                    UserId = user._userId,
                };
                log.Information("ExamName :{0} UserId :{1}", examName.ExamName, examName.UserId);
                string checkAvailableExamJson = JsonConvert.SerializeObject(examName);

                // Calling ViewProgress Service
                string urlCheckExamName = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/CheckExamNameAvailable";
                string checkExamNameResult = HttpProxy.HttpPost(urlCheckExamName, checkAvailableExamJson, "application/json; charset=utf-8", "POST");
                string checkExamName = JsonConvert.DeserializeObject<string>(checkExamNameResult);
                log.Information("end Controller :{0} method :{1} ExamName:{2}", "ExamManagerController", "CheckExamNameAvailable", ExamName);
                return Json(checkExamName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "ExamName:" + ExamName);
                throw;
            }
        }

        public ActionResult GetExamCountOnExamType()
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} userId:{2}", "ExamManagerController", "GetExamCountOnExamType", user._userId);
                ExamCountOnExamTypeVM examCount = new ExamCountOnExamTypeVM
                {
                    UserId = user._userId,
                };

                // Get ExamId By UserId
                string countOnExamTypeDataJson = JsonConvert.SerializeObject(examCount);
                string urlGetCountOnExamType = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/GetExamCountOnExamType";
                string resultGetExamIdBYUserId = HttpProxy.HttpPost(urlGetCountOnExamType, countOnExamTypeDataJson, "application/json; charset=utf-8", "POST");
                ExamCountOnExamTypeVM count = JsonConvert.DeserializeObject<ExamCountOnExamTypeVM>(resultGetExamIdBYUserId);
                ViewBag.QuestionTypeCount = count;
                log.Information("QuestionTypeCount :{0}", count);
                log.Information("end Controller :{0} method :{1} userId:{2}", "ExamManagerController", "GetExamCountOnExamType", user._userId);
                return Json(count, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId);
                throw;
            }
        }

        public ActionResult GetQuestionTypeCountBySection(string sectionValue)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} sectionValue{2} userId:{3} ", "ExamManagerController", "GetQuestionTypeCountBySection", sectionValue, user._userId);

                QuestionCountOnSection sectoinValue = new QuestionCountOnSection
                {
                    UserId = user._userId,
                    SectionId = sectionValue,
                };

                // Get ExamId By UserId
                string sectoinValueDataJson = JsonConvert.SerializeObject(sectoinValue);
                string urlGetCountOnsectoinValue = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/GetQuestionTypeCountBySection";
                string resultGetExamIdBYUserId = HttpProxy.HttpPost(urlGetCountOnsectoinValue, sectoinValueDataJson, "application/json; charset=utf-8", "POST");
                QuestionCountOnSection sectionWiseCount = JsonConvert.DeserializeObject<QuestionCountOnSection>(resultGetExamIdBYUserId);
                ViewBag.QuestionTypeCount = sectionWiseCount;
                log.Information("QuestionTypeCount :{0}", sectionWiseCount);
                log.Information("end Controller :{0} method :{1} sectionValue{2} userId:{3} ", "ExamManagerController", "GetQuestionTypeCountBySection", sectionValue, user._userId);
                return Json(sectionWiseCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + "UserId:" + user._userId + "sectionValue:" + sectionValue);
                throw;
            }
        }

        // This Method Will Call the Web Service for getting Spaced Repetition Data
        public ActionResult SpacedRepetition(ExamDTO srExamData)
        {
            try
            {
                log.Information("start Controller:{0} method:{1} userId:{2}", "ExamManagerController", "SpacedRepetition", srExamData.UserId);
                ExamDTO spacedRepititionExam = new ExamDTO
                {
                    NoofQuestions = -1,
                    ExamCreateDate = DateTime.Now,
                    ExamType = 3,
                    ExamAnswerToShow = true,
                };
                List<string> listofCategory = srExamData.TypeofCategoryList;
                spacedRepititionExam.TypeofCategoryListstring = string.Join(",", listofCategory);

                log.Information("end Controller :{0} method :{1} ExamType:{2} userId:{3}", "ExamManagerController", "SpacedRepetition", spacedRepititionExam.ExamType, srExamData.UserId);
                log.Information("Redirect to Controller :{0} method :{1}", "ExamManagerController", "CreateExam");
                return RedirectToAction("CreateExam", spacedRepititionExam);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:ExamManagerController" + " method:SpacedRepetition" + "UserId:" + user._userId);
                throw;
            }

        }

        public ActionResult SearchEmails(string term)
        {
            try
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Information("start Controller :{0} method :{1} term:{2} userId:{3}", "ExamManagerController", "SearchEmails", term, user._userId);

                if (string.IsNullOrEmpty(term) || term.Length < 2)
                {
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);
                }

                // Create search request DTO
                EmailSearchDTO searchRequest = new EmailSearchDTO
                {
                    SearchTerm = term,
                    UserId = user._userId
                };

                string searchRequestJson = JsonConvert.SerializeObject(searchRequest);
                string urlSearchEmails = AAOGlobalConstants.SiteWebAPIUrl + "ExamManager/SearchEmails";
                string searchResult = HttpProxy.HttpPost(urlSearchEmails, searchRequestJson, "application/json; charset=utf-8", "POST");

                List<EmailSuggestionDTO> emailSuggestions = JsonConvert.DeserializeObject<List<EmailSuggestionDTO>>(searchResult);

                // Format for autocomplete (jQuery UI format)
                var formattedResults = emailSuggestions.Select(x => new
                {
                    label = $" ({x.Email})", // Display name with email
                    email = x.Email,
                }).ToList();

                log.Information("end Controller :{0} method :{1} results count:{2} userId:{3}", "ExamManagerController", "SearchEmails", formattedResults.Count, user._userId);
                return Json(formattedResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
                log.Error(ex.Message + " Controller:ExamManagerController method:SearchEmails UserId:" + user._userId + " SearchTerm:" + term);
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SendSharedExamEmail(string emailId, string examName, DateTime? startDate, DateTime? endDate)
        {
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            try
            {
                log.Information("start Controller :{0} method :{1} emaild:{2} userId:{3} examName:{4}", "ExamManagerController", "ShareExam", emailId, user._userId, examName);

                string userName = user._userName;

                ExamQuestionDTO examque = new ExamQuestionDTO();
                examque.UserId = user._userId;
                examque.Emailids = Convert.ToString(emailId);
                string examPostDataJson = JsonConvert.SerializeObject(examque);

                string emailids = AAOGlobalConstants.SiteWebAPIUrl + "ExamHistory/InsertorAddEmail_GetOptOutDetails";
                string emailidsResult = HttpProxy.HttpPost(emailids, examPostDataJson, "application/json; charset=utf-8", "POST");
                List<ExamHistoryDTO> emailDetails = new List<ExamHistoryDTO>();
                emailDetails = JsonConvert.DeserializeObject<List<ExamHistoryDTO>>(emailidsResult);

                var filePath = @"~\ShareResultEmailTemplete\SharedExamEmailTemplate.html";
                DirectoryInfo directory = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(filePath));

                var doc = new HtmlDocument();
                doc.Load(directory.ToString());
                string shareResultBody = doc.Text;

                shareResultBody = shareResultBody.Replace("UserName", userName);

                // Format startDate and endDate if they have values
                //string formattedStartDate = startDate.HasValue ? startDate.Value.ToString("MM-dd-yyyy") : "N/A";
                //string formattedEndDate = endDate.HasValue ? endDate.Value.ToString("MM-dd-yyyy") : "N/A";
                //string formattedEndTime = endDate.HasValue ? endDate.Value.ToString("HH:mm") : "N/A";

                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                string formattedStartDate = startDate.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(startDate.Value.ToUniversalTime(), easternZone).ToString("MM-dd-yyyy") : "N/A";
                string formattedEndDate = endDate.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(endDate.Value.ToUniversalTime(), easternZone).ToString("MM-dd-yyyy") : "N/A";

                //string formattedEndTime = endDate.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(endDate.Value.ToUniversalTime(), easternZone).ToString("HH:mm") : "N/A";

                string formattedEndTime = endDate.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(
                                                                endDate.Value.TimeOfDay == TimeSpan.Zero ?
                                                                endDate.Value.Date.AddHours(23).AddMinutes(59).ToUniversalTime() :
                                                                endDate.Value.ToUniversalTime(),
                                                                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToString("HH:mm") :
                                                                "N/A";

                // Check if both dates are null
                if (!startDate.HasValue && !endDate.HasValue)
                {
                    shareResultBody = shareResultBody.Replace("StartDateAndExpires", "The exam has no expiration.");
                }
                else
                {
                    shareResultBody = shareResultBody.Replace(
                        "StartDateAndExpires",
                        $"The exam can be started on '{formattedStartDate}' and expires on '{formattedEndDate}' at '{formattedEndTime}' Eastern Time.");
                }

                bool flag = SendEmailToRecipients(shareResultBody, emailId, emailDetails);

                log.Information("end Controller :{0} method :{1} emaild:{2} userId:{3} examName:{4} result:{5}", "ExamManagerController", "ShareExam", emailId, user._userId, examName, flag);

                if (flag)
                {
                    return Json(new { success = true, message = "Exam shared successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Failed to share exam" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + " Controller:{0}, Method:{1}, UserId:{2}", "ExamManagerController", "ShareExam", user._userId);
                return Json(new { success = false, message = "An error occurred while sharing the exam: " + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool SendEmailToRecipients(string shareResultBody, string emailid, List<ExamHistoryDTO> emailDetails)
        {
            string returnUrl = ConfigurationManager.AppSettings["RETURN_SITE_URL"];
            string examHistoryurl = ConfigurationManager.AppSettings["EXAM_HISTORY_LINK"];
            UserIdentity user = new UserIdentity(System.Web.HttpContext.Current.User.Identity.Name);
            var sendFeedback = new AssessmentController();
            try
            {
                bool allEmailsSent = true;
                string[] emailArray = emailid.Split(',');

                foreach (string email in emailArray)
                {
                    var emailDetail = emailDetails.FirstOrDefault(e => e.Emailids == email && e.OptOut == 0);
                    if (emailDetail != null)
                    {
                        string trimmedEmail = email.Trim();
                        shareResultBody = shareResultBody.Replace("ExamHistoryLink", "<a href='" + examHistoryurl + "'>Click here to view the exam.</a>");
                        string emailSpecificBody = shareResultBody;

                        string unsubscribe_link = "<a href='" + returnUrl + "/EmailOptOut/OptOut?optOut=" + emailDetail.Id + "' style='color: #3e87cb;'>Unsubscribe</a>";
                        emailSpecificBody = shareResultBody.Replace("Unsubscribe", unsubscribe_link);

                        int result = sendFeedback.SendFeedbackSharedExam(emailSpecificBody, trimmedEmail);
                        if (result != 1)
                        {
                            allEmailsSent = false;
                        }
                    }
                }

                return allEmailsSent;
            }
            catch (Exception e)
            {
                log.Error(e.Message + " Controller:{0}, Method:{1}, UserId:{2}", "ExamManagerController", "SendEmailToRecipients", user._userId);
                return false;
            }
        }
    }
}