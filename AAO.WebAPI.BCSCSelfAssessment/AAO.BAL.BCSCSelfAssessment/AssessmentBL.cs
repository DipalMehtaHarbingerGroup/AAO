namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using HtmlAgilityPack;
    using System.Configuration;
    using Serilog;
    using System.Data.SqlClient;
    using AAO.SQLHelper.BCSCSelfAssessment;

    public static class AssessmentBL
    {
        private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private static Serilog.Core.Logger log= new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

        // GET: ExamManager

        public static QuestionDetails GetExamQuestion(int? examId, int? questionId, int? userId)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            connection.Open();

            try
            {
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
                // Examination ExamQuestionList = new Examination();
                ExamDTO examObj = AssessmentBL.GetExamBYId(examId);
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                if (Convert.ToInt32(questionId) == 0)
                {
                    log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetFirstQuestion", examId, questionId, userId);
                    questionId = AssessmentDAL.GetFirstQuestion(examObj);
                    log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetFirstQuestion", examId, questionId, userId);
                }

                // get QuestionDetail with Next and Previous Id
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDetails", examId, questionId, userId);
                questionDetails = AssessmentDAL.GetQuestionDetails(Convert.ToInt32(examId), Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDetails", examId, questionId, userId);
                // Get Question Choice by Question Id
                // choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId));
                // questionDetails.choiceList = choiceList;

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId), connection);

                // questionDetails.questionText = StripHTML(questionDetails.questionText);
                questionDetails.examStatus = examObj.ExamStatus;
                questionDetails.examAttemptId = examObj.ExamAttemptId;
                questionDetails.examTimeType = examObj.ExamTimeType;
                questionDetails.examMode = examObj.ExamMode;
                questionDetails.examAnswerToShow = examObj.ExamAnswerToShow;
                questionDetails.noOfQuestion = examObj.NoofQuestions;
                questionDetails.ExamName = examObj.ExamName;
                questionDetails.ExamType = examObj.ExamType;
                questionDetails.userId = examObj.UserId;
                // Get Question Disscussion on Question Id
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDiscussion", examId, questionId, userId);
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId), connection);
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDiscussion", examId, questionId, userId);

                // Get References from Question Table on base on Question Id
                questionDetails.references = AssessmentDAL.GetReferences(Convert.ToInt32(questionId), connection);

                // Get Notes from Notes On Base on QuestionId and ExamId
                questionDetails.notes = AssessmentDAL.GetNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId), Convert.ToInt32(examId), connection);

                // Get RelatedContent from RelatedCOntent On Base on QuestionId
                questionDetails.RelatedContents = AssessmentDAL.GetRelatedContent(Convert.ToInt32(questionId), connection);

                // Get IsAttemped this Question in this Exam
                questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer(Convert.ToInt32(questionId), Convert.ToInt32(examObj.ExamAttemptId), connection);

                // GetExamAttemptText
                questionDetails.examAttemptText = AssessmentDAL.GetExamAttemptAnswerText(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Mark Status
                questionDetails.markCheck = AssessmentDAL.GetMarkStatus(Convert.ToInt32(examId), Convert.ToInt32(questionId), Convert.ToInt32(userId));
                questionDetails.examId = Convert.ToInt32(examId);
                questionDetails.CurrentExamAttemptTime = DateTime.Now;

                // Get Question Choice by Question Id
                choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptId, questionDetails.NotRandomizeAnswer, connection);
                questionDetails.choiceList = choiceList;

                // Get PeerDetails for this Question
                // questionDetails.PeerDetails = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetPeerDetails", examId, questionId, userId);
                var peerdata = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetPeerDetails", examId, questionId, userId);
                questionDetails.PeerDetails = peerdata.Where(s => s.ChoiceId != 0).OrderBy(s => s.ChoiceOption).ToList();

                // Question Count for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionCount", examId, questionId, userId);
                questionDetails.QuestionCount = AssessmentDAL.GetQuestionCount(Convert.ToInt32(examId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionCount", examId, questionId, userId);

                // Get BCSCExcerpt for this Question
                //log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);
                //questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
                //log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);

                //questionDetails.ExcerptHeadings = GetExcerptHeadings(questionDetails.BCSCExcerpt);

                // Get Topic for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userId:{3}", "GetTopicByQuestion", examId, questionId, userId);
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userId:{3}", "GetTopicByQuestion", examId, questionId, userId);
                connection.Close();
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
                return questionDetails;
            }
            catch (Exception e)
            {
                log.Error(e.Message + " method:GetExamQuestion" + " examId:" + examId + " UserId:" + userId);
                Console.Write(e.Message);
                return null;
            }
        }

        //HG-459 Impact here - Added for User Report if ExamType=3 then fetch Ques from there 
        public static QuestionDetails GetExamQuestionForSR(int? examId, int? questionId, int? userId)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            connection.Open();

            try
            {
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
                ExamDTO examObj = AssessmentBL.GetExamBYId(examId);
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                if (Convert.ToInt32(questionId) == 0)
                {
                    log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetFirstQuestion", examId, questionId, userId);
                    questionId = AssessmentDAL.GetFirstQuestion(examObj);
                    log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetFirstQuestion", examId, questionId, userId);
                }

                // get QuestionDetail with Next and Previous Id
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDetails", examId, questionId, userId);
                questionDetails = AssessmentDAL.GetQuestionDetails_SR(Convert.ToInt32(examId), Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDetails", examId, questionId, userId);

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId), connection);

                // questionDetails.questionText = StripHTML(questionDetails.questionText);
                questionDetails.examStatus = examObj.ExamStatus;
                questionDetails.examAttemptId = examObj.ExamAttemptId;
                questionDetails.examTimeType = examObj.ExamTimeType;
                questionDetails.examMode = examObj.ExamMode;
                questionDetails.examAnswerToShow = examObj.ExamAnswerToShow;
                questionDetails.noOfQuestion = examObj.NoofQuestions;
                questionDetails.ExamName = examObj.ExamName;
                questionDetails.ExamType = examObj.ExamType;
                questionDetails.userId = examObj.UserId;
                // Get Question Disscussion on Question Id
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDiscussion", examId, questionId, userId);
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId), connection);
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionDiscussion", examId, questionId, userId);

                // Get References from Question Table on base on Question Id
                questionDetails.references = AssessmentDAL.GetReferences(Convert.ToInt32(questionId), connection);

                // Get RelatedContent from RelatedCOntent On Base on QuestionId
                questionDetails.RelatedContents = AssessmentDAL.GetRelatedContent(Convert.ToInt32(questionId), connection);

                // Get Notes from Notes On Base on QuestionId and ExamId
                questionDetails.notes = AssessmentDAL.GetNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId), Convert.ToInt32(examId), connection);

                // Get IsAttemped this Question in this Exam
                questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer(Convert.ToInt32(questionId), Convert.ToInt32(examObj.ExamAttemptId), connection);

                // GetExamAttemptText
                questionDetails.examAttemptText = AssessmentDAL.GetExamAttemptAnswerText(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Mark Status
                questionDetails.markCheck = AssessmentDAL.GetMarkStatus(Convert.ToInt32(examId), Convert.ToInt32(questionId), Convert.ToInt32(userId));
                questionDetails.examId = Convert.ToInt32(examId);
                questionDetails.CurrentExamAttemptTime = DateTime.Now;

                // Get Question Choice by Question Id
                choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptId, questionDetails.NotRandomizeAnswer, connection);
                questionDetails.choiceList = choiceList;

                // Get PeerDetails for this Question
                // questionDetails.PeerDetails = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetPeerDetails", examId, questionId, userId);
                var peerdata = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetPeerDetails", examId, questionId, userId);
                questionDetails.PeerDetails = peerdata.Where(s => s.ChoiceId != 0).OrderBy(s => s.ChoiceOption).ToList();

                // Question Count for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionCount", examId, questionId, userId);
                questionDetails.QuestionCount = AssessmentDAL.GetQuestionCount(Convert.ToInt32(examId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetQuestionCount", examId, questionId, userId);

                // Get BCSCExcerpt for this Question
                //log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);
                //questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
                //log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);

                //questionDetails.ExcerptHeadings = GetExcerptHeadings(questionDetails.BCSCExcerpt);

                // Get Topic for this Question
                log.Information("start method :{0} examId:{1} questionId:{2} userId:{3}", "GetTopicByQuestion", examId, questionId, userId);
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userId:{3}", "GetTopicByQuestion", examId, questionId, userId);
                connection.Close();
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
                return questionDetails;
            }
            catch (Exception e)
            {
                log.Error(e.Message + " method:GetExamQuestion" + " examId:" + examId + " UserId:" + userId);
                Console.Write(e.Message);
                return null;
            }
        }

        public static string StripHTML(string input)
        {
            if (input != null)
            {
                return Regex.Replace(input, "<.*?>", string.Empty);
            }
            else
            {
                return null;
            }
        }

        // public static QuestionDetails UpdateExamAttempt(int examId, int userId, int examStatus)
        // {
        //    QuestionDetails questionDetail = new QuestionDetails();
        //    if (examStatus == 0)
        //    {
        //        examStatus = 1;
        //    }

        // int status = AssessmentDAL.UpdateExamAttempt(examId, userId, examStatus);
        //    questionDetail.examStatus = status;

        // // AssessmentDAL.DeleteQuestionType(ExamId, userId);
        //    return questionDetail;
        // }

        public static int UpdateSkipAnswerQuestion(UpdateSkipAnswered updateAnswer)
        {
            log.Information("start method :{0} examId:{1} userId:{2} QuestionId:{3}", "UpdateSkipAnswerQuestion", updateAnswer.examId, updateAnswer.userId, updateAnswer.questionId);
            ExamDTO examObj = ExamDAL.GetExamBYId(updateAnswer.examId);
            if (updateAnswer.selectedChoiceId == 0)
            {
                ExamSkipQuestionDTO skipQuestion = new ExamSkipQuestionDTO
                {
                    ExamId = updateAnswer.examId,
                    QuestionId = updateAnswer.currentQuestionId,
                    ExamAttemptId = Convert.ToInt32(updateAnswer.examAttemptId),
                    UserId = updateAnswer.userId,
                    ExamCreatedDate = examObj.ExamCreateDate,
                    ExamLastAttemptDate = DateTime.Now,
                };
                log.Information("end method :{0} examId:{1} userId:{2}QuestionId:{3}", "UpdateSkipAnswerQuestion", updateAnswer.examId, updateAnswer.userId, updateAnswer.questionId);
                return AssessmentDAL.UpdateSkipQuestion(skipQuestion);
            }
            else
            {
                IncorrectQuestionDTO incorrectQuestion = new IncorrectQuestionDTO
                {
                    QuestionId = updateAnswer.currentQuestionId,
                    UserId = updateAnswer.userId,
                    ExamAttemptId = Convert.ToInt32(updateAnswer.examAttemptId),
                    IsRight = Convert.ToBoolean(updateAnswer.isRight),
                };
                if (examObj.ExamType != Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    log.Information("start method :{0} examId:{1} userId:{2} questionId:{3} ", "UpdateOrAddIncorrectAnswer", updateAnswer.examId, updateAnswer.userId, updateAnswer.questionId);
                    AssessmentDAL.UpdateOrAddIncorrectAnswer(incorrectQuestion);
                    log.Information("end method :{0} examId:{1} userId:{2} questionId:{3}", "UpdateOrAddIncorrectAnswer", updateAnswer.examId, updateAnswer.userId, updateAnswer.questionId);
                }
                ExamAttemptAnswerDTO attemptAnswer = new ExamAttemptAnswerDTO
                {
                    ChoiceId = updateAnswer.selectedChoiceId,
                    IsRight = Convert.ToBoolean(updateAnswer.isRight),
                    ExamAttemptDate = DateTime.Now,
                    UserId = updateAnswer.userId,
                    QuestionId = updateAnswer.currentQuestionId,
                    ExamAttemptId = Convert.ToInt32(updateAnswer.examAttemptId),
                    IsYourAnswer = updateAnswer.isYourAnswer,
                    UserExamState = updateAnswer.UserExamState,
                };
                if (examObj.ExamType == Convert.ToInt32(ExamManagerEnum.ExamType.SpacedRepetition))
                {
                    if(Convert.ToBoolean(updateAnswer.isRight)== false)
                    {
                        SRRecommendedDeleteVM updatedeltopic = new SRRecommendedDeleteVM
                        {
                            ExamId= updateAnswer.examId,
                            UserId= updateAnswer.userId,
                            QuestionId= updateAnswer.questionId,
                            IsRight= Convert.ToBoolean(updateAnswer.isRight)
                        };
                        AssessmentDAL.UpdateDeleteTopic(updatedeltopic);
                    }
                    log.Information("start method :{0} examId:{1} userId:{2}", "UpdateSpacedRepetitionDetails", updateAnswer.examId, updateAnswer.userId);
                    UpdateSpacedRepetitionDetails(examObj, updateAnswer, ref attemptAnswer);
                    log.Information("end method :{0} examId:{1} userId:{2}", "UpdateSpacedRepetitionDetails", updateAnswer.examId, updateAnswer.userId);
                }
                log.Information("end method :{0} examId:{1} userId:{2} QuestionId:{3}", "UpdateSkipAnswerQuestion", updateAnswer.examId, updateAnswer.userId, updateAnswer.questionId);
                if (attemptAnswer.Quality != 0 && !string.IsNullOrEmpty(attemptAnswer.UserExamState))
                {
                    return AssessmentDAL.UpdateAttemptAnswer_SR_05Mar24(attemptAnswer);
                }

                return AssessmentDAL.UpdateAttemptAnswer(attemptAnswer);
            }
        }

        public static void AddNotesAnswerText(UpdateSkipAnswered notesAnswer)
        {
            log.Information("start method :{0} examId:{1} userId:{2} questionId:{3}", "AddNotesAnswerText", notesAnswer.examId, notesAnswer.userId, notesAnswer.questionId);
            if (notesAnswer.textType == 'C' && notesAnswer.examAttemptText != string.Empty)
            {
                log.Information("end method :{0} examId:{1} userId:{2} questionId:{3}", "AddNotesAnswerText", notesAnswer.examId, notesAnswer.userId, notesAnswer.questionId);
                AssessmentDAL.AddExamAttemptText(notesAnswer);
            }
            else if (notesAnswer.textType == 'N' && notesAnswer.examAttemptText != string.Empty)
            {
                log.Information("end method :{0} examId:{1} userId:{2} questionId:{3}", "AddNotesAnswerText", notesAnswer.examId, notesAnswer.userId, notesAnswer.questionId);
                AssessmentDAL.AddExamQuestionNotes(notesAnswer);
            }
        }

        public static bool MarkQuestionCheck(UpdateSkipAnswered updateMark)
        {
            return AssessmentDAL.AddOrDeleteMarkedQuestion(updateMark);
        }

        public static ExamDTO GetExamBYId(int? examId)
        {
            return AssessmentDAL.GetExamBYId(examId);
        }

        public static int SetTimeForQuestion(int? attemptId)
        {
            return AssessmentDAL.SetTimeForQuestion(attemptId);
        }

        public static void UpdateConsumedTime(UpdateSkipAnswered updateTime)
        {
            AssessmentDAL.UpdateConsumedTime(Convert.ToInt32(updateTime.examAttemptId), updateTime.consumedTimeDifference);
        }

        public static int SubmitExam(UpdateSkipAnswered submitExam)
        {
            return AssessmentDAL.SubmitExam(submitExam);
        }

        public static List<ViewProgressVM> ViewProgress(UpdateSkipAnswered viewProgress)
        {
            return AssessmentDAL.ViewProgress(viewProgress);
        }

        public static void SaveIsYourAnswer(UpdateSkipAnswered isYourAnswer)
        {
            AssessmentDAL.SaveIsYourAnswer(isYourAnswer);
        }

        public static int GetRightChoiceId(RightQuestionIdVM rightQuestionId)
        {
            return AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(rightQuestionId.RightQuestionId));
        }

        public static void UpdateTime(UpdateSkipAnswered updateTime)
        {
            AssessmentDAL.UpdateTime(Convert.ToInt32(updateTime.examAttemptId), updateTime.consumedTimeDifference);
        }

        public static List<string> GetNotes(UpdateSkipAnswered getNotes)
        {
            return AssessmentDAL.GetNotes(Convert.ToInt32(getNotes.currentQuestionId), Convert.ToInt32(getNotes.userId), Convert.ToInt32(getNotes.examId));
        }

        public static QuestionDetails GetExcerpt(int examId, int? questionId, int userId)
        {
            QuestionDetails questionDetails1 = new QuestionDetails();
            //log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);
            questionDetails1.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
            //log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);

            questionDetails1.ExcerptHeadings = GetExcerptHeadings(questionDetails1.BCSCExcerpt);

            return questionDetails1;
        }

        public static List<ExcerptHeading> GetExcerptHeadings(List<string> bCSCExcerpts)
        {
            List<ExcerptHeading> excerptHeadings = new List<ExcerptHeading>();
            int id = 1;
            log.Information("start method :{0} Id:{1}", "GetExcerptHeadings", id);
            foreach (string bCSCExcerpt in bCSCExcerpts)
            {
                ExcerptHeading excerptHeading = new ExcerptHeading();
                if (bCSCExcerpt != string.Empty)
                {
                    string pattern = string.Empty;
                    string pattern1 = string.Empty;
                    string pattern2 = string.Empty;
                    string str = bCSCExcerpt.Substring(0, 3);
                    string topicexcerpt = string.Empty;
                    if (str == "<p>" && !string.IsNullOrEmpty(bCSCExcerpt))
                    {
                        pattern = "<p><br /.*?>(.*?)<br /><br /></p>";
                        pattern1 = "<p><strong>(.*?)<br /><br /></p>";
                        pattern2 = "<p>(.*?)</p>"; //Added for HG-464

                        MatchCollection matche = Regex.Matches(bCSCExcerpt, pattern);
                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(bCSCExcerpt);
                        List<HtmlNode> chaptertitle = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "ul" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("bcsc-excerpt-custom-ul"))).ToList();
                        topicexcerpt = "<ul class=\"bcsc-excerpt-custom-ul\">" + chaptertitle[0].InnerHtml + "</ul>";

                        //MatchCollection matches = Regex.Matches(bCSCExcerpt, pattern);
                        MatchCollection matches = Regex.Matches(bCSCExcerpt, pattern2);
                        excerptHeading.HeadingId = id;
                        excerptHeading.HeadingText = matches[0].Value + topicexcerpt;
                        //excerptHeading.Excerpts = Regex.Replace(bCSCExcerpt, pattern, string.Empty);
                        excerptHeading.Excerpts = Regex.Replace(bCSCExcerpt, pattern2, string.Empty); //Added for HG-464
                        excerptHeading.Excerpts = excerptHeading.Excerpts.Replace(topicexcerpt, string.Empty);
                        topicexcerpt = string.Empty;
                        excerptHeadings.Add(excerptHeading);
                    }
                    else
                    {
                        pattern = "<br\\/><b.*?>(.*?)<\\/li><\\/ul><br />";
                        MatchCollection matches = Regex.Matches(bCSCExcerpt, pattern);
                        excerptHeading.HeadingId = id;
                        excerptHeading.HeadingText = matches[0].Value;
                        excerptHeading.Excerpts = Regex.Replace(bCSCExcerpt, pattern, string.Empty);
                        excerptHeadings.Add(excerptHeading);
                    }

                    id++;
                }
            }
            log.Information("end method :{0} Id:{1}", "GetExcerptHeadings", id);
            return excerptHeadings;
        }

        public static QuestionDetails GetExamQuestion_Search(int? examId, int? questionId, int? userId)
        {
            try
            {
                // Examination ExamQuestionList = new Examination();
                ExamDTO examObj = AssessmentBL.GetExamBYId(examId);
                List<Choice> choiceList = new List<Choice>();
                QuestionDetails questionDetails = new QuestionDetails();
                log.Information("start method :{0} ExamId:{1} userId:{2} questionId;{3}", "GetExamQuestion_Search", examId, userId, questionId);
                if (Convert.ToInt32(questionId) == 0)
                {
                    questionId = AssessmentDAL.GetFirstQuestion(examObj);
                }

                // get QuestionDetail with Next and Previous Id
                if (examId == 0 && examId != null)
                {
                    questionDetails = AssessmentDAL.GetQuestionDetails_Search(Convert.ToInt32(questionId));
                }
                else
                {
                    questionDetails = AssessmentDAL.GetQuestionDetails(Convert.ToInt32(examId), Convert.ToInt32(questionId));
                }

                // Get Question Choice by Question Id
                // choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId));
                // questionDetails.choiceList = choiceList;

                // Get Right Choice for Given Question Id
                questionDetails.rightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionId));

                // questionDetails.questionText = StripHTML(questionDetails.questionText);
                questionDetails.examStatus = examObj.ExamStatus;
                questionDetails.examAttemptId = examObj.ExamAttemptId;
                questionDetails.examTimeType = examObj.ExamTimeType;
                questionDetails.examMode = examObj.ExamMode;
                questionDetails.examAnswerToShow = examObj.ExamAnswerToShow;
                questionDetails.noOfQuestion = examObj.NoofQuestions;
                questionDetails.ExamName = examObj.ExamName;
                questionDetails.ExamType = examObj.ExamType;
                questionDetails.userId = examObj.UserId;

                // Get Question Disscussion on Question Id
                questionDetails.questionDiscussion = AssessmentDAL.GetQuestionDiscussion(Convert.ToInt32(questionId));

                // Get References from Question Table on base on Question Id
                questionDetails.references = AssessmentDAL.GetReferences(Convert.ToInt32(questionId));

                // Get RelatedContent from RelatedCOntent On Base on QuestionId
                questionDetails.RelatedContents = AssessmentDAL.GetRelatedContent(Convert.ToInt32(questionId));

                questionDetails.notes = AssessmentDAL.GetNotes(Convert.ToInt32(questionId), Convert.ToInt32(userId), Convert.ToInt32(examId));

                if (examId == 0 && examId != null)
                {
                    // Get IsAttemped this Question in this Exam
                    questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer_Search(Convert.ToInt32(questionId), Convert.ToInt32(userId));
                }
                else
                {
                    questionDetails.examAttemptAnswerDTO = AssessmentDAL.GetExamAttemptAnswer(Convert.ToInt32(questionId), Convert.ToInt32(examObj.ExamAttemptId));
                }
                // GetExamAttemptText
                questionDetails.examAttemptText = AssessmentDAL.GetExamAttemptAnswerText(Convert.ToInt32(examId), Convert.ToInt32(questionId));

                // Get Mark Status
                questionDetails.markCheck = AssessmentDAL.GetMarkStatus(Convert.ToInt32(examId), Convert.ToInt32(questionId), Convert.ToInt32(userId));
                questionDetails.examId = Convert.ToInt32(examId);
                questionDetails.CurrentExamAttemptTime = DateTime.Now;

                // Get Question Choice by Question Id
                if (examId == 0 && examId != null)
                {
                    choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptAnswerDTO.ExamAttemptId, questionDetails.NotRandomizeAnswer);
                    questionDetails.choiceList = choiceList;
                }
                else
                {
                    choiceList = AssessmentDAL.GetChoiceBYQuestionId(Convert.ToInt32(questionId), examId, questionDetails.examAttemptAnswerDTO.ChoiceId, questionDetails.examAttemptId, questionDetails.NotRandomizeAnswer);
                    questionDetails.choiceList = choiceList;
                }

                // Get PeerDetails for this Question
                // questionDetails.PeerDetails = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                if (examId == 0 && examId != null)
                {
                    var peerdata = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptAnswerDTO.ExamAttemptId);
                    questionDetails.PeerDetails = peerdata.Where(s => s.ChoiceId != 0).OrderBy(s => s.ChoiceOption).ToList();

                    // Question Count for this Question
                    //questionDetails.QuestionCount = AssessmentDAL.GetQuestionCount(Convert.ToInt32(examId));
                }
                else
                {
                    var peerdata = AssessmentDAL.GetPeerDetails(Convert.ToInt32(questionId), Convert.ToInt32(userId), questionDetails.examAttemptId);
                    questionDetails.PeerDetails = peerdata.Where(s => s.ChoiceId != 0).OrderBy(s => s.ChoiceOption).ToList();

                    // Question Count for this Question
                    questionDetails.QuestionCount = AssessmentDAL.GetQuestionCount(Convert.ToInt32(examId));
                }
                // Get BCSCExcerpt for this Question
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));

                questionDetails.ExcerptHeadings = GetExcerptHeadings(questionDetails.BCSCExcerpt);

                // Get Topic for this Question
                questionDetails.TopicName = AssessmentDAL.GetTopicByQuestion(Convert.ToInt32(questionId));
                log.Information("end method :{0} ExamId:{1} userId:{2}", "GetExamQuestion_Search", questionDetails.examId, questionDetails.userId);
                return questionDetails;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        /// <summary>
        /// This is Spaced Repetition Algorithm
        /// </summary>
        /// <param name="examObj"></param>
        /// <param name="examAttemptAnswerDTO"></param>
        public static void UpdateSpacedRepetitionDetails(ExamDTO examObj, UpdateSkipAnswered updateSkipAnswered, ref ExamAttemptAnswerDTO examAttemptAnswerDTO)
        {
            if (examAttemptAnswerDTO.IsRight == true && updateSkipAnswered.ButtonType == 3)
            {
                examAttemptAnswerDTO.Quality = 4;
            }
            else if (examAttemptAnswerDTO.IsRight == true && updateSkipAnswered.ButtonType == 4)
            {
                examAttemptAnswerDTO.Quality = 3;
            }
            else if (examAttemptAnswerDTO.IsRight == false && updateSkipAnswered.ButtonType == 3)
            {
                examAttemptAnswerDTO.Quality = 2;
            }
            else if (examAttemptAnswerDTO.IsRight == false && updateSkipAnswered.ButtonType == 4)
            {
                examAttemptAnswerDTO.Quality = 1;
            }
            else
            {
                examAttemptAnswerDTO.Quality = 0;
            }
            log.Information("start method :{0} ExamId:{1} userId:{2} Quality:{3}", "UpdateSpacedRepetitionDetails", updateSkipAnswered.examId, updateSkipAnswered.userId, examAttemptAnswerDTO.Quality);

            examAttemptAnswerDTO.SessionId = examObj.SessionId;

            // Set current round number for reporting purpose in future
            int attemptedQuestionCount = AssessmentDAL.GetAttemptedQuestionCount(examAttemptAnswerDTO.UserId, examObj.SessionId, examAttemptAnswerDTO.ExamAttemptId);

            if (attemptedQuestionCount == 29)
            {
                examAttemptAnswerDTO.RoundNumber = examObj.RoundNumber + 1;
                ExamDAL.UpdateExam(examObj.ExamId, examObj.UserId, examAttemptAnswerDTO.RoundNumber, "RoundNumber");
            }

            if (attemptedQuestionCount == 44)
            {
                examAttemptAnswerDTO.RoundNumber = examObj.RoundNumber + 1;
                ExamDAL.UpdateExam(examObj.ExamId, examObj.UserId, examAttemptAnswerDTO.RoundNumber, "RoundNumber");
            }
            else if (attemptedQuestionCount == 59)
            {
                examAttemptAnswerDTO.RoundNumber = examObj.RoundNumber + 1;
                ExamDAL.UpdateExam(examObj.ExamId, examObj.UserId, examAttemptAnswerDTO.RoundNumber, "RoundNumber");
            }
           else
            {
                examAttemptAnswerDTO.RoundNumber = examObj.RoundNumber;
            }

            if (examObj.RoundNumber == 1)
            {
                ResponseStatusVM rstatus = new ResponseStatusVM();
                examAttemptAnswerDTO.EasinessFactor = 2.5;
                //examAttemptAnswerDTO.NewEasinessFactor = 2.5;
                // examAttemptAnswerDTO.Iteration = 6;
                examAttemptAnswerDTO.NewEasinessFactor = examAttemptAnswerDTO.EasinessFactor + (0.1 - ((5 - examAttemptAnswerDTO.Quality) * (0.08 + ((5 - examAttemptAnswerDTO.Quality) * 0.02)))) + examAttemptAnswerDTO.Quality;
                examAttemptAnswerDTO.Iteration = ((1) * Convert.ToInt32(examAttemptAnswerDTO.NewEasinessFactor));
            }
            else if (examObj.RoundNumber > 1)
            {
                int easiness = AssessmentDAL.GetNewEasynessFactor(examAttemptAnswerDTO.UserId, examAttemptAnswerDTO.QuestionId, examAttemptAnswerDTO.ExamAttemptId);
                if (easiness == 0)
                {
                    examAttemptAnswerDTO.EasinessFactor = 2.5;
                }
                else
                {
                    examAttemptAnswerDTO.EasinessFactor = easiness;
                }

                examAttemptAnswerDTO.NewEasinessFactor = examAttemptAnswerDTO.EasinessFactor + (0.1 - ((5 - examAttemptAnswerDTO.Quality) * (0.08 + ((5 - examAttemptAnswerDTO.Quality) * 0.02)))) + examAttemptAnswerDTO.Quality;
                examAttemptAnswerDTO.Iteration = ((examObj.RoundNumber - 1) * Convert.ToInt32(examAttemptAnswerDTO.NewEasinessFactor));

            //examAttemptAnswerDTO.NewEasinessFactor = examAttemptAnswerDTO.EasinessFactor + (0.1 - ((5 - examAttemptAnswerDTO.Quality) * (0.08 + ((5 - examAttemptAnswerDTO.Quality) * 0.02)))); /*+ examAttemptAnswerDTO.Quality;*/
            //examAttemptAnswerDTO.Iteration = ((examObj.RoundNumber - 1) * Convert.ToInt32(examAttemptAnswerDTO.NewEasinessFactor));/* + examObj.RoundNumber;*/
            }

            log.Information("end method :{0} ExamId:{1} userId:{2} Quality:{3} RoundNumber:{4}", "UpdateSpacedRepetitionDetails", updateSkipAnswered.examId, updateSkipAnswered.userId, examAttemptAnswerDTO.Quality, examAttemptAnswerDTO.RoundNumber);
        }
    }
}
