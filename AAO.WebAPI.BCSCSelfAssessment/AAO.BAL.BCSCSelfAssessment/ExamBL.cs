namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;
    using Serilog;
    using System.Data;
    using System.Data.SqlClient;

    public static class ExamBL
    {
        private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private static Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

        public static List<ExamHistoryVM> ExamMyProgressView(int? userId)
        {
            return ExamDAL.GetExamListView(userId);
        }

        public static ResponseStatusVM CreateExam(ExamDTO objCreateExam)
        {
            ResponseStatusVM status = new ResponseStatusVM();

            // Set Exam Title If not given
            if (objCreateExam.ExamName == null)
            {
                if (Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode) == objCreateExam.ExamType)
                {
                    objCreateExam.ExamName = "CustomExam_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + (objCreateExam.UserId.ToString());
                }
                else if (Convert.ToInt32(ExamManagerEnum.ExamType.SimulatedMode) == objCreateExam.ExamType)
                {
                    objCreateExam.ExamName = "SimulatedExam_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + (objCreateExam.UserId.ToString());
                }
                else if (Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode) == objCreateExam.ExamType)
                {
                    objCreateExam.ExamName = "QuickExam_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + (objCreateExam.UserId.ToString());
                }
                else if (Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) == objCreateExam.ExamType)
                {
                    objCreateExam.ExamName = "SpacedRepetition_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + (objCreateExam.UserId.ToString());
                }
                else if (Convert.ToInt32(ExamManagerEnum.ExamType.SharedMode) == objCreateExam.ExamType)
                {
                    objCreateExam.ExamName = "SharedExam_" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + (objCreateExam.UserId.ToString());
                }
                log.Information("start method :{0} userId:{1} ExamName:{2}", "CreateExam", objCreateExam.UserId, objCreateExam.ExamName);
            }

            if (Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode) == objCreateExam.ExamType || Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) == objCreateExam.ExamType)
            {
                objCreateExam.TypeofQuestion = (string.Join(",", objCreateExam.TypeofQuestionList.Select(x => x.ToString())));
                objCreateExam.TypeofCategory = (string.Join(",", objCreateExam.TypeofCategoryList.Select(x => x.ToString())));
                status = CreateExamAndAddExamQuestion(objCreateExam);
            }
            else if (Convert.ToInt32(ExamManagerEnum.ExamType.SimulatedMode) == objCreateExam.ExamType)
            {
                objCreateExam.NoofQuestions = Convert.ToInt32(ConfigurationManager.AppSettings["SimulatedQuestionCount"]);
                objCreateExam.ExamTimeType = true;
                status = CreateExamAndAddExamQuestion(objCreateExam);
            }
            else if (Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode) == objCreateExam.ExamType)
            {
                // objCreateExam.NoofQuestions = Convert.ToInt32(ConfigurationManager.AppSettings["QuickQuestionCount"]);
                // objCreateExam.ExamTimeType = true;
                objCreateExam.TypeofQuestion = (string.Join(",", objCreateExam.TypeofQuestionList.Select(x => x.ToString())));
                status = CreateExamAndAddExamQuestion(objCreateExam);
            }
            else if (Convert.ToInt32(ExamManagerEnum.ExamType.SharedMode) == objCreateExam.ExamType)
            {
                objCreateExam.TypeofQuestion = (string.Join(",", objCreateExam.TypeofQuestionList.Select(x => x.ToString())));
                objCreateExam.TypeofCategory = (string.Join(",", objCreateExam.TypeofCategoryList.Select(x => x.ToString())));
                status = CreateExamAndAddExamQuestion(objCreateExam);
            }
            log.Information("end method :{0}  userId:{1} ExamName:{2} ExamType:{3} ", "CreateExam", objCreateExam.UserId, objCreateExam.ExamName, objCreateExam.ExamType);
            return status;
        }

        public static ExamDTO GetExamBYId(int? examId)
        {
            return ExamDAL.GetExamBYId(examId);
        }

        // public static ExamAttemptDTO GetExamAttempt(ExamDTO examObj)
        // {
        //    ExamAttemptDTO examAttempt = new ExamAttemptDTO();
        //    examAttempt.ExamCreatedDate = examObj.ExamCreateDate;
        //    examAttempt.ExamLastAttemptDate = DateTime.Now;
        //    examAttempt.ExamId = examObj.ExamId;
        //    examAttempt.UserId = examObj.UserId;
        //    examAttempt.ExamStatus = false;
        //    return examAttempt;
        // }

        // public static int AddExamAttempt(ExamAttemptDTO examAttempt)
        // {
        //    return ExamAttemptDAL.AddExamAttempt(examAttempt);
        // }
        public static List<QuestionTypeCountDTO> GetQuestionTypeCount(int? userId)
        {
            return ExamDAL.GetQuestionTypeCount(userId);
        }

        public static List<int> GetQuestionList(ExamDTO exam)
        {
            List<int> examQuestionList = new List<int>();
            List<SelectedQuestionTypeId> selectedQuestionTypeIdList = new List<SelectedQuestionTypeId>();
            List<SpecialityId> specialityIdList = new List<SpecialityId>();
            int totalinputquestion;
            log.Information("start method :{0} examId:{1} userId:{2} totalinputquestion:{3}", "GetQuestionList", exam.ExamId, exam.UserId, exam.NoofQuestions);
            // Get QuestionList for Quick Exam Mode
            if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SharedMode))
            {
                if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SharedMode))
                {
                    if (exam.SelectedQuestions == null) { exam.SelectedQuestions = ""; }
                    int countSelectedQuestions = exam.SelectedQuestions.Split(',').Length;
                    totalinputquestion = exam.NoofQuestions - countSelectedQuestions;
                }
                else
                {
                    totalinputquestion = exam.NoofQuestions;
                }

                var globaladjustment = (IEnumerable<decimal>)null;

                // Get the default speciality for Quick mode Exam
                if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode))
                {
                    List<SubSpecialityDetailVM> speciality = new List<SubSpecialityDetailVM>();
                    speciality = SpecialityDAL.GetSpecialityList(exam.UserId);

                    // Fill the default speciality for Quick mode Exam
                    foreach (var specialityId in speciality)
                    {
                        SpecialityId specilityId = new SpecialityId();
                        specilityId.specialityId = Convert.ToInt32(specialityId.SpecialityId);
                        specialityIdList.Add(specilityId);
                    }

                    globaladjustment = SplitQuestionNumber(totalinputquestion, speciality.Count);
                    log.Information(" method :{0} examId:{1} userId:{2} totalinputquestion:{3}", "GetQuestionList", exam.ExamId, exam.UserId, exam.NoofQuestions);
                }
                else if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SharedMode))
                {
                    foreach (var id in exam.TypeofCategoryList)
                    {
                        SpecialityId specilityId = new SpecialityId();
                        specilityId.specialityId = Convert.ToInt32(id);
                        specialityIdList.Add(specilityId);
                    }

                    globaladjustment = SplitQuestionNumber(totalinputquestion, specialityIdList.Count);
                }

                // Fill Question type list Selected by user
                foreach (var id in exam.TypeofQuestionList)
                {
                    SelectedQuestionTypeId questionType = new SelectedQuestionTypeId();
                    questionType.questionTypeId = Convert.ToInt32(id);
                    selectedQuestionTypeIdList.Add(questionType);
                }

                var globaladjustmentList = globaladjustment.ToList();
                int globaladjustmentSeq = 0;
                foreach (var item in specialityIdList)
                {
                    List<int> questionList = new List<int>();
                    double topCount = Math.Ceiling(Convert.ToDouble(globaladjustmentList[globaladjustmentSeq]));
                    SpecialityQuestion listSpecialityQuestion = new SpecialityQuestion();
                    questionList = ExamDAL.GetExamQuestionIdList(item.specialityId, exam.UserId, selectedQuestionTypeIdList, Convert.ToInt32(topCount));

                    // specialityQuestionList.Add(listSpecialityQuestion);
                    examQuestionList.AddRange(questionList);
                    globaladjustmentSeq++;
                }
            }

            // Get QuestionList for Simulated Exam Mode
            else if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SimulatedMode))
            {
                totalinputquestion = Convert.ToInt32(ConfigurationManager.AppSettings["SimulatedQuestionCount"]);
                examQuestionList = ExamDAL.GetSimulatedExamQuestionList(totalinputquestion);
            }

            // Get QuestionList for Custom Exam Mode
            else if (exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) || exam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SharedMode))
            {
                totalinputquestion = exam.NoofQuestions;

                // Fill Type of Category List
                foreach (var id in exam.TypeofCategoryList)
                {
                    SpecialityId specilityId = new SpecialityId();
                    specilityId.specialityId = Convert.ToInt32(id);
                    specialityIdList.Add(specilityId);
                }

                // Fill Type of Question List
                foreach (var id in exam.TypeofQuestionList)
                {
                    SelectedQuestionTypeId questionType = new SelectedQuestionTypeId();
                    questionType.questionTypeId = Convert.ToInt32(id);
                    selectedQuestionTypeIdList.Add(questionType);
                }
            }

            // List<int> ExamQuestion = new List<int>();
            examQuestionList = examQuestionList.Distinct().ToList();
            log.Information("end method :{0} examId:{1} userId:{2} ExamName:{3} totalinputquestion:{4} ExamType:{5}", "GetQuestionList", exam.ExamId, exam.UserId, exam.ExamName, exam.NoofQuestions, exam.ExamType);
            return examQuestionList;

        }

        public static IEnumerable<decimal> SplitQuestionNumber(decimal value, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("count must be greater than zero.", "count");
            }
            log.Information("start method :{0} value:{1}  count:{2}", "SplitQuestionNumber", value, count);
            var result = new decimal[count];
            decimal runningTotal = 0M;
            for (int i = 0; i < count; i++)
            {
                var remainder = value - runningTotal;
                var share = remainder > 0M ? Math.Max(Math.Round(remainder / ((decimal)(count - i)), 2), .01M) : 0M;
                result[i] = share;
                runningTotal += share;
            }

            if (runningTotal < value)
            {
                result[count - 1] += value - runningTotal;
            }
            log.Information("end method :{0} value:{1} count:{2}", "SplitQuestionNumber", value, count);
            return result;
        }

        // Filling Question list With Question Id And Exam Id
        public static List<ExamQuestionDTO> FillQuestionList(ExamDTO examObj, List<int> selectedQuestionList, int noOfQuestion)
        {
            List<ExamQuestionDTO> questionList = new List<ExamQuestionDTO>();
            List<ExamQuestionDTO> questionShuffleList = new List<ExamQuestionDTO>();
            List<ExamQuestionDTO> questionShuffleListAll = new List<ExamQuestionDTO>();
            log.Information("start method :{0} examId:{1} userId:{2}  totalinputquestion:{3}", "FillQuestionList", examObj.ExamId, examObj.UserId, examObj.NoofQuestions);
            foreach (int questionId in selectedQuestionList)
            {
                ExamQuestionDTO questionDTOListitem = new ExamQuestionDTO();
                questionDTOListitem.ExamId = Convert.ToInt32(examObj.ExamId);
                questionDTOListitem.QuestionId = Convert.ToInt32(questionId);
                questionList.Add(questionDTOListitem);
            }

            try
            {
                if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    string selectedQuestionList_string = string.Join(",", selectedQuestionList); //Added for HG-459 to fetch only selected sec ques.
                    List<ExamQuestionDTO> attemptedQuestionDetails = GetAttemptedQuestionDetails_SR(examObj, selectedQuestionList_string);
                    questionShuffleList.AddRange(attemptedQuestionDetails);
                }

                if (questionList.Count > 0)
                {

                    if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) && questionList.Count > 0)
                    {
                        questionList = ShuffleList(questionList);
                        int count = 0;
                        foreach (ExamQuestionDTO questionObj in questionList)
                        {
                            if (count < questionShuffleList.Count)
                            {
                                questionShuffleListAll.Add(questionShuffleList[count]);
                                count++;
                            }
                            questionShuffleListAll.Add(questionObj);
                        }
                        questionShuffleList.Clear();
                        questionShuffleList = questionShuffleListAll;
                    }
                    else
                    {
                        questionShuffleList.AddRange(ShuffleList(questionList));
                    }
                    questionList.Clear();
                    if (noOfQuestion == -1)
                    {
                        questionList = questionShuffleList;
                    }
                    else
                    {
                        int questionCount = 0;
                        foreach (ExamQuestionDTO question in questionShuffleList)
                        {
                            if (questionCount < noOfQuestion)
                            {
                                questionList.Add(question);
                                questionCount++;
                                continue;
                            }

                            if (questionCount >= noOfQuestion)
                            {
                                break;
                            }

                            questionCount++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + "ExamId: "+examObj.ExamId.ToString()+"UserId: "+ examObj.UserId.ToString());
                Console.WriteLine(e.Message);
            }
            log.Information("end method :{0} examId:{1} userId:{2}  totalinputquestion:{3}", "FillQuestionList", examObj.ExamId, examObj.UserId, examObj.NoofQuestions);
            return questionList;
        }

        // Filling Question list With Question Id And Exam Id for Shared Exam
        public static List<ExamQuestionDTO> FillQuestionListSharedExam(ExamDTO examObj, List<int> selectedQuestionList, int noOfQuestion, string selectedQuestions)
        {
            List<ExamQuestionDTO> questionList = new List<ExamQuestionDTO>();
            List<ExamQuestionDTO> questionShuffleList = new List<ExamQuestionDTO>();
            List<ExamQuestionDTO> questionShuffleListAll = new List<ExamQuestionDTO>();
            List<ExamQuestionDTO> priorityQuestions = new List<ExamQuestionDTO>();

            log.Information("start method :{0} examId:{1} userId:{2}  totalinputquestion:{3}", "FillQuestionList", examObj.ExamId, examObj.UserId, examObj.NoofQuestions);

            // Parse selectedQuestions string to get priority question IDs
            List<int> priorityQuestionIds = new List<int>();
            if (!string.IsNullOrEmpty(selectedQuestions))
            {
                try
                {
                    priorityQuestionIds = selectedQuestions.Split(',')
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => Convert.ToInt32(x.Trim()))
                        .ToList();
                }
                catch (Exception ex)
                {
                    log.Warning("Error parsing selectedQuestions: {0}", ex.Message);
                }
            }

            // Create priority questions first (these will be the first questions in the exam)
            foreach (int questionId in priorityQuestionIds)
            {
                ExamQuestionDTO priorityQuestionItem = new ExamQuestionDTO();
                priorityQuestionItem.ExamId = Convert.ToInt32(examObj.ExamId);
                priorityQuestionItem.QuestionId = Convert.ToInt32(questionId);
                priorityQuestions.Add(priorityQuestionItem);
            }

            // Create remaining questions (excluding priority questions to avoid duplicates)
            foreach (int questionId in selectedQuestionList.Where(x => !priorityQuestionIds.Contains(x)))
            {
                ExamQuestionDTO questionDTOListitem = new ExamQuestionDTO();
                questionDTOListitem.ExamId = Convert.ToInt32(examObj.ExamId);
                questionDTOListitem.QuestionId = Convert.ToInt32(questionId);
                questionList.Add(questionDTOListitem);
            }

            try
            {
                if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    string selectedQuestionList_string = string.Join(",", selectedQuestionList);
                    List<ExamQuestionDTO> attemptedQuestionDetails = GetAttemptedQuestionDetails_SR(examObj, selectedQuestionList_string);
                    questionShuffleList.AddRange(attemptedQuestionDetails);
                }

                if (questionList.Count > 0)
                {
                    if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition) && questionList.Count > 0)
                    {
                        questionList = ShuffleList(questionList);
                        int count = 0;
                        foreach (ExamQuestionDTO questionObj in questionList)
                        {
                            if (count < questionShuffleList.Count)
                            {
                                questionShuffleListAll.Add(questionShuffleList[count]);
                                count++;
                            }
                            questionShuffleListAll.Add(questionObj);
                        }
                        questionShuffleList.Clear();
                        questionShuffleList = questionShuffleListAll;
                    }
                    else
                    {
                        questionShuffleList.AddRange(ShuffleList(questionList));
                    }

                    questionList.Clear();

                    // Start with priority questions (these will be first)
                    questionList.AddRange(priorityQuestions);

                    if (noOfQuestion == -1)
                    {
                        // Add all remaining shuffled questions
                        questionList.AddRange(questionShuffleList);
                    }
                    else
                    {
                        // Calculate how many more questions we need after priority questions
                        int remainingQuestionsNeeded = Math.Max(0, noOfQuestion - priorityQuestions.Count);
                        int questionCount = 0;

                        foreach (ExamQuestionDTO question in questionShuffleList)
                        {
                            if (questionCount < remainingQuestionsNeeded)
                            {
                                questionList.Add(question);
                                questionCount++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // If no other questions, at least add priority questions
                    questionList.AddRange(priorityQuestions);
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message + "ExamId: " + examObj.ExamId.ToString() + "UserId: " + examObj.UserId.ToString());
                Console.WriteLine(e.Message);
            }

            log.Information("end method :{0} examId:{1} userId:{2}  totalinputquestion:{3} finalQuestionCount:{4}",
                "FillQuestionList", examObj.ExamId, examObj.UserId, examObj.NoofQuestions, questionList.Count);

            return questionList;
        }

        public static List<ExamQuestionDTO> GetAttemptedQuestionDetails_SR(ExamDTO examObj, string selectedQuestionList_string)
        {
            return ExamDAL.GetAttemptedQuestionDetails_SR(examObj, selectedQuestionList_string);
        }

        public static int GetExamIdBYUserId(int? userId)
        {
            return ExamDAL.GetExamIdBYUserId(Convert.ToInt32(userId));
        }

        // This Method Used to Get Exam Status Count On Dashboard
        public static ExamStatusCountVM GetExamStatusCount(ServiceCallVM examService)
        {
            return ExamDAL.GetExamStatusCount(examService);
        }

        // This method will Get Exam Score On Dashboard
        public static List<ExamScoreDetailVM> GetExamScore(ServiceCallVM examService)
        {
            log.Information("start method :{0} userId:{1}", "GetExamScore", examService.userId);
            List<ExamScoreDetailVM> examScoreList = new List<ExamScoreDetailVM>();
            ExamScoreDetailVM examScore = new ExamScoreDetailVM();

            // Get Custom Exam Score
            examService.examType = Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode);
            examScore = ExamDAL.GetExamScore(examService);
            examScore.examType = Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode);
            examScoreList.Add(examScore);

            // Get Simulated Exam Score
            examService.examType = Convert.ToInt32(ExamManagerEnum.ExamType.SimulatedMode);
            examScore = ExamDAL.GetExamScore(examService);
            examScore.examType = Convert.ToInt32(ExamManagerEnum.ExamType.SimulatedMode);
            examScoreList.Add(examScore);

            // Get Quick Exam Score
            examService.examType = Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode);
            examScore = ExamDAL.GetExamScore(examService);
            examScore.examType = Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode);
            examScoreList.Add(examScore);
            log.Information("end method :{0} userId:{2}", "GetExamScore", examService.userId);
            return examScoreList;
        }

        // This Method will Get  Dashboard Chart Details
        public static List<DashboardChartVM> GetChartDetail(ChartParameterVM chartParameters)
        {
            if (chartParameters.ScoreCompWithPeers == false)
            {
                return ExamDAL.GetCumulativeChartDetail(chartParameters);
            }
            else
            {
                return ExamDAL.GetPeerChartDetail(chartParameters);
            }
        }

        // Get Available Exam Name
        public static string CheckExamNameAvailable(ExamNameVM examName)
        {
            return ExamDAL.CheckExamNameAvailable(examName);
        }

        // Get  ExamType Count on base of ExamType and userid
        public static ExamCountOnExamTypeVM GetExamCountOnExamType(int userId)
        {
            return ExamDAL.GetExamCountOnExamType(userId);
        }

        // Adding exam question and updating exam
        public static ResponseStatusVM CreateExamAndAddExamQuestion(ExamDTO objCreateExam)
        {
            List<ExamQuestionDTO> questionList = new List<ExamQuestionDTO>();
            List<int> listOfquestionCount = new List<int>();
            ExamAttemptDTO examAttempt = new ExamAttemptDTO();
            ResponseStatusVM status = new ResponseStatusVM();
            ExamDTO examObj = new ExamDTO();
            int noofQuestion = 0;
            int createdexamId = 0;
            int simulatedTime = Convert.ToInt32(ConfigurationManager.AppSettings["SimulatedQuestionTime"]);
            int time = Convert.ToInt32(ConfigurationManager.AppSettings["IndividualQuestionTime"]);
            log.Information("start method :{0}  userId:{1} ExamMode:{2} ExamName:{3} ExamType:{4}", "CreateExamAndAddExamQuestion", objCreateExam.UserId, objCreateExam.ExamMode, objCreateExam.ExamName, objCreateExam.ExamType);
            // Creating  Exam
            if (objCreateExam.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
            {
                examObj = SpacedRepetitionBL.GetSpacedRepitionExam(objCreateExam.UserId, objCreateExam.TypeofCategory); //'TypeofCategory' added for HG-459 
                if (examObj.ExamId > 0)
                {
                    createdexamId = examObj.ExamId;
                }
                else
                {
                    objCreateExam.RoundNumber = 1;
                    objCreateExam.SessionId = 1;
                    createdexamId = ExamDAL.CreateExam(objCreateExam);
                }
            }
            else
            {
                createdexamId = ExamDAL.CreateExam(objCreateExam);
            }
            log.Information("method :{0} userId:{1} ExamMode:{2} ExamId:{3} ExamType:{4} RoundNo {5}", "CreateExamAndAddExamQuestion",  objCreateExam.UserId, objCreateExam.ExamMode, createdexamId, objCreateExam.ExamType, objCreateExam.RoundNumber);
            //createdexamId = ExamDAL.CreateExam(objCreateExam);
            if (createdexamId > 0)
            {
                examObj = GetExamBYId(createdexamId);
                // Getting Question List Of Created Exam
                listOfquestionCount = GetQuestionList(objCreateExam);

                // Filling QuestionList With Exam Id
                if (objCreateExam.ExamType == 4)
                {
                    questionList = FillQuestionListSharedExam(examObj, listOfquestionCount, objCreateExam.NoofQuestions, objCreateExam.SelectedQuestions);
                }
                else
                {
                    questionList = FillQuestionList(examObj, listOfquestionCount, objCreateExam.NoofQuestions);
                }
                if (questionList.Count > 0)
                {
                    noofQuestion = questionList.Count;

                    // examAttempt = GetExamAttempt(examObj);
                    if ((Convert.ToInt32(ExamManagerEnum.ExamType.CustomMode) == objCreateExam.ExamType && Convert.ToInt32(examObj.ExamTimeType) != Convert.ToInt32(ExamManagerEnum.ExamTimeType.Time)))
                    {
                        examAttempt.TotalAllocatedTime = Convert.ToInt32(noofQuestion * time);
                    }
                    else if (Convert.ToInt32(ExamManagerEnum.ExamType.SimulatedMode) == objCreateExam.ExamType)
                    {
                        examAttempt.TotalAllocatedTime = Convert.ToInt32(simulatedTime);
                    }

                    // else if (Convert.ToInt32(ExamManagerEnum.ExamType.QuickMode) == objCreateExam.ExamType)
                    // {
                    //    examAttempt.TotalAllocatedTime = Convert.ToInt32(noofQuestion * time);
                    // }
                    else
                    {
                        examAttempt.TotalAllocatedTime = 0;
                    }

                    examAttempt.TotalConsumedTime = 0;

                    // AddExamAttempt(examAttempt);
                    // Adding Exam Question in ExamQuestion Table
                    ExamQuestionDAL.AddExamQuestion(questionList, examObj);

                    // Update Exam in Exam Table With Final No. of Question
                    ExamDAL.UpdateExam(createdexamId, objCreateExam.UserId, noofQuestion, "NoofQuestions");
                    status.UserEnteredQuestionCount = objCreateExam.NoofQuestions;
                    status.SystemGeneratedQuestionCount = noofQuestion;
                    status.ErrorCode = 1;
                    status.ExamId = createdexamId;
                    status.Errormessage = "Exam created";
                    log.Information(status.Errormessage + "ExamId:" + status.ExamId);
                }
                else
                {
                    // Delete Exam from Exam Table if  Final No. of Question is ZERO
                    ExamDAL.DeleteExam(createdexamId, objCreateExam.UserId);
                    status.ErrorCode = 2;
                    status.ExamId = createdexamId;
                    status.Errormessage = "An issue was encountered while building this exam. Please try again.";
                    log.Information(status.Errormessage+"ExamId:"+ status.ExamId);
                }
            }
            else
            {
                status.ErrorCode = 2;
                status.Errormessage = "An issue was encountered while building this exam. Please try again.";
                log.Information(status.Errormessage + "ExamId:" + status.ExamId);
            }
            log.Information("end method :{0} examId:{1} userId:{2}  RoundNo {3}", "CreateExamAndAddExamQuestion", createdexamId, objCreateExam.UserId,  objCreateExam.RoundNumber);

            return status;
        }

        // Get  UserStatus IsFirstUser
        public static int GetUserStatusIsFirst(ServiceCallVM userService)
        {
            return ExamDAL.GetUserStatusIsFirst(userService);
        }

        // Update  UserStatus IsFirstUser
        public static void UpdateUserStatusIsFirst(ServiceCallVM userService)
        {
            ExamDAL.UpdateUserStatusIsFirst(userService);
        }

        // Get  QuestionType Count on Section
        public static QuestionCountOnSection GetQuestionTypeCountBySection(QuestionCountOnSection sectionValue)
        {
            return ExamDAL.GetQuestionTypeCountBySection(sectionValue);
        }

        public static string GetUserOptInStatus(ServiceCallVM userService)
        {
            return ExamDAL.GetUserOptInStatus(userService);
        }

        // Update UserOptIn
        public static int UpdateUserOptIn(ServiceCallVM userService)
        {
            return ExamDAL.UpdateUserOptIn(userService);
        }

        // Random the Question List
        private static List<ExamQuestionDTO> ShuffleList<ExamQuestionDTO>(List<ExamQuestionDTO> inputList)
        {
            List<ExamQuestionDTO> randomList = new List<ExamQuestionDTO>();
            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); // Choose a random object in the list
                randomList.Add(inputList[randomIndex]); // add it to the new, random list
                inputList.RemoveAt(randomIndex); // remove to avoid duplicates
            }
            log.Information("method :{0} randomListCount:{1}", "ShuffleList", randomList.Count);

            return randomList; // return the new random list
        }

        public static List<EmailSuggestionDTO> SearchEmails(EmailSearchDTO searchRequest)
        {
            try
            {
                if (searchRequest == null || string.IsNullOrEmpty(searchRequest.SearchTerm))
                {
                    return new List<EmailSuggestionDTO>();
                }

                string searchTerm = searchRequest.SearchTerm.Trim();
                if (searchTerm.Length < 2)
                {
                    return new List<EmailSuggestionDTO>();
                }

                // Create SQL parameters array
                SqlParameter[] arrSqlParameter = new SqlParameter[]
                {
            new SqlParameter("@SearchTerm", searchTerm),
            new SqlParameter("@UserId", searchRequest.UserId)
                };

                var emailSuggestions = new List<EmailSuggestionDTO>();

                using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                    SqlConnectionProvider.GetConnectionString(DataAccessType.Read),
                    CommandType.StoredProcedure,
                    "AAO_SearchUserEmails",
                    arrSqlParameter))
                {
                    while (objSqlDataReader.Read())
                    {
                        var emailSuggestion = new EmailSuggestionDTO
                        {
                            UserId = objSqlDataReader["UserId"] != DBNull.Value ? Convert.ToInt32(objSqlDataReader["UserId"]) : 0,
                            Email = objSqlDataReader["Email"]?.ToString(),
                        };
                        emailSuggestions.Add(emailSuggestion);
                    }
                }

                return emailSuggestions;
            }
            catch (Exception ex)
            {
                //Logger.Error($"Error in SearchEmailsStoredProc: {ex.Message}", ex);
                return new List<EmailSuggestionDTO>();
            }
        }
    }
}
