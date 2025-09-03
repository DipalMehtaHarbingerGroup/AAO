namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using System.Data.SqlClient;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using Serilog;
    using System;
    using System.Configuration;
    using System.Linq;

    public static class IncorrectQuestionDetailsBL
    {
        private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private static Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

        public static List<QuestionDetails> IncorrectQuestionDetails(ReportsVM incorrectReportDetails)
        {
            List<QuestionDetails> reportQuestionDetailsList = new List<QuestionDetails>();
            List<IncorrectQuestionDetailsDTO> questionList = new List<IncorrectQuestionDetailsDTO>();
            questionList = IncorrectQuestionDetailsDAL.IncorrectQuestionDetails(incorrectReportDetails.UserId, incorrectReportDetails.SubspecialtyId, incorrectReportDetails.ExamStartDate, incorrectReportDetails.ExamCompletedDate, incorrectReportDetails.NoOfRecords, incorrectReportDetails.PageNo, incorrectReportDetails.Year);
            foreach (var item in questionList)
            {
                item.UserId = incorrectReportDetails.UserId;
                QuestionDetails reportQuestionDetails = new QuestionDetails();

                if (item.ExamType == 3)
                {
                    //reportQuestionDetails = AssessmentBL.GetExamQuestionForSR(item.ExamId, item.QuestionId, item.UserId);
                    reportQuestionDetails = GetExamQuestionForSR(item.ExamId, item.QuestionId, item.UserId);
                }
                else
                {
                    //reportQuestionDetails = AssessmentBL.GetExamQuestion(item.ExamId, item.QuestionId, item.UserId);
                    reportQuestionDetails = GetExamQuestion(item.ExamId, item.QuestionId, item.UserId);
                }

                reportQuestionDetails.QuestionCount = item.QuestionIdCount;
                reportQuestionDetails.serialNumber = item.Rownumber;
                reportQuestionDetails.SubSpeciality = item.Subspecialty;
                reportQuestionDetails.Section = item.Section;
                reportQuestionDetailsList.Add(reportQuestionDetails);
            }

            return reportQuestionDetailsList;
        }

        public static QuestionDetails GetExamQuestion(int? examId, int? questionId, int? userId)
        {
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            connection.Open();

            try
            {
                Log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetExamQuestion", examId, questionId, userId);
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
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);

                questionDetails.ExcerptHeadings = AssessmentBL.GetExcerptHeadings(questionDetails.BCSCExcerpt);

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
                log.Information("start method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);
                questionDetails.BCSCExcerpt = AssessmentDAL.GetBCSCExcerptText(Convert.ToInt32(questionId));
                log.Information("end method :{0} examId:{1} questionId:{2} userid:{3}", "GetBCSCExcerptText", examId, questionId, userId);

                questionDetails.ExcerptHeadings = AssessmentBL.GetExcerptHeadings(questionDetails.BCSCExcerpt);

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
    }
}
